//Handles all of the Base Enemies Attacks

using System.Collections;
//Handles the Base Enemy Attack

using UnityEngine;

public class BaseEnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private BaseEnemyHealth _health;

    [SerializeField] private bool _isAttacking;

    [SerializeField] private ItemManager _itemManager;

    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private EnemyManager _enemyManager;

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
        //If the enemy is in the players collider and is not attacking
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
        if (GameManager.Instance.GetComponent<ItemManager>().SlimeArmour)
        {
            StartCoroutine(SlimeArmour());
        }
    }

    //Slows down the enemy when the player has the Slime Armour Item
    IEnumerator SlimeArmour()
    {
        _enemyManager.BaseEnemyMoveSpeed = _enemyManager.DefaultBaseEnemyMoveSpeed * _itemManager.SlimeArmourMultiplier;
        _enemyManager.BruteMoveSpeed = _enemyManager.DefaultBruteMoveSpeed * _itemManager.SlimeArmourMultiplier;
        yield return new WaitForSeconds(_itemManager.SlimeArmourSecondsUntilNormalSpeed);
        _enemyManager.BaseEnemyMoveSpeed = _enemyManager.DefaultBaseEnemyMoveSpeed;
        _enemyManager.BruteMoveSpeed = _enemyManager.DefaultBruteMoveSpeed;
    }
}