//Handles all of the Base Enemies Attacks

using System.Collections;
using UnityEngine;

public class BaseEnemyAttack : MonoBehaviour
{
    private Animator _animator;

    private BaseEnemyHealth _health;

    [SerializeField] private bool _isAttacking;

    private ItemManager _itemManager;

    private PlayerManager _playerManager;

    private EnemyManager _enemyManager;

    void Awake()
    {
        _animator = transform.parent.parent.GetComponent<Animator>();
        _health = transform.parent.parent.GetComponent<BaseEnemyHealth>();
    }

    void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
        _enemyManager = GameManager.Instance.GetComponent<EnemyManager>();

        _isAttacking = false;
    }

    private void OnTriggerStay(Collider other)
    {
        //If the enemy is in the players collider and is not attacking and both the player and the enemy are not dead
        if (other.gameObject.tag == "Player" && _isAttacking == false && _health.IsDead == false && _playerManager.PlayerIsDead == false)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        //Play attack animation and set isAttacking to true. After animation is done, set isAttacking to false
        if (transform.root.gameObject.tag == "Brute")
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
        foreach (ItemList i in _itemManager.Items)
        {
            StartCoroutine(i.item.Coroutine(_itemManager, i.stacks));
        }
    }
}