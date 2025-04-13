//Handles all of the Base Enemies Attacks

using System.Collections;
using UnityEngine;

public class BaseEnemyAttack : MonoBehaviour
{
    private Animator _animator;

    private BaseEnemyHealth _health;

    private GameManager _gameMan;

    private bool _isAttacking;
    void Awake()
    {
        _animator = transform.parent.parent.GetComponent<Animator>();
        _health = transform.parent.parent.GetComponent<BaseEnemyHealth>();
    }

    void Start()
    {
        _isAttacking = false;
    }

    private void OnTriggerStay(Collider other)
    {
        //If the enemy is in the players collider and is not attacking
        if (other.gameObject.tag == "Player" && _isAttacking == false && _health.IsDead == false && GameManager.Instance.PlayerIsDead == false)
        {
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        //Play attack animation and set isAttacking to true. After animation is done, set isAttacking to false

        if(transform.root.gameObject.tag == "Brute")
        {
            int randomNum = Random.Range(1, 4);
            if (randomNum >= 2)
            {
                _animator.SetTrigger("Attack");
            }
            else
            {
                _animator.SetTrigger("Attack2");
            }
        }
        else
        {
            _animator.SetTrigger("Attack");
        }
        _isAttacking = true;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        _isAttacking = false;
    }
}
