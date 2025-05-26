//Handles the movement for all Base Enemies

using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyMovement : MonoBehaviour
{
    private NavMeshAgent _enemyNav;

    private BaseEnemyHealth _baseHealth;

    private Animator _animator;

    private BaseEnemyStartMove _moveTrigger;

    private GameObject TargetPlayer;

    private BaseEnemyFlip _baseEnemyFlip;

    private EnemyManager _enemyManager;

    void Awake()
    {
        _enemyNav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _baseHealth = GetComponent<BaseEnemyHealth>();
        _baseEnemyFlip = GetComponent<BaseEnemyFlip>();
        _moveTrigger = transform.GetChild(3).GetComponent<BaseEnemyStartMove>();
    }

    private void Start()
    {
        _enemyManager = GameManager.Instance.GetComponent<EnemyManager>();
    }

    void Update()
    {
        BaseEnemyFollow();
    }

    void BaseEnemyFollow()
    {
        switch (gameObject.tag) 
        {
            case "BaseEnemy":
                _enemyNav.speed = _enemyManager.BaseEnemyMoveSpeed;
                break;
            case "Brute":
                _enemyNav.speed = _enemyManager.BruteMoveSpeed;
                break;
            case "Mage":
                _enemyNav.speed = _enemyManager.MageMoveSpeed;
                break;
        }

        if (_baseHealth.CanMove == true && _moveTrigger.StartMove && _baseEnemyFlip.FindClosestPlayer().GetComponent<PlayerMovement>().CanMove)
        {
            _enemyNav.SetDestination(_baseEnemyFlip.FindClosestPlayer().transform.position);
        }

        if (transform.hasChanged)
        {
            _animator.SetBool("isMoving", true);
            transform.hasChanged = false;
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    //Changes the movement speed of the enemy (Debug)
    public void DebugMoveSpeed()
    {
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                _enemyNav.speed = _enemyManager.BaseEnemyMoveSpeed;
                break;
            case "Brute":
                _enemyNav.speed = _enemyManager.BruteMoveSpeed;
                break;
            case "Mage":
                _enemyNav.speed = _enemyManager.MageMoveSpeed;
                break;
        }
    }
}
