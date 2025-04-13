//Handles the movement for all Base Enemies

using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyMovement : MonoBehaviour
{
    private NavMeshAgent _enemyNav;

    private BaseEnemyHealth _baseHealth;

    private Animator _animator;

    private BaseEnemyStartMove _moveTrigger;

    void Awake()
    {
        _enemyNav = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _baseHealth = GetComponent<BaseEnemyHealth>();
        _moveTrigger = transform.GetChild(3).GetComponent<BaseEnemyStartMove>();
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
                _enemyNav.speed = GameManager.Instance.BaseEnemyMoveSpeed;
                break;
            case "Brute":
                _enemyNav.speed = GameManager.Instance.BruteMoveSpeed;
                break;
            case "Mage":
                _enemyNav.speed = GameManager.Instance.MageMoveSpeed;
                break;
        }

        if (_baseHealth.CanMove == true && _moveTrigger.StartMove && GameManager.Instance.Player.GetComponent<PlayerMovement>().CanMove)
        {
            _enemyNav.SetDestination(GameManager.Instance.Player.transform.position);
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
                _enemyNav.speed = GameManager.Instance.BaseEnemyMoveSpeed;
                break;
            case "Brute":
                _enemyNav.speed = GameManager.Instance.BruteMoveSpeed;
                break;
            case "Mage":
                _enemyNav.speed = GameManager.Instance.MageMoveSpeed;
                break;
        }
    }
}
