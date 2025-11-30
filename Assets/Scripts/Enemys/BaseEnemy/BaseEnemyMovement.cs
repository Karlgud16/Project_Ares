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

    private MiniBossManager _miniBossManager;

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
        _miniBossManager = GameManager.Instance.GetComponent<MiniBossManager>();

        if (gameObject.name.Contains("Borrek"))
        {
            _animator.speed = 0.75f;
        }
    }

    void Update()
    {
        BaseEnemyFollow();
    }

    void BaseEnemyFollow()
    {
        //Sets the enemy move speed by comparing the name of the game object
        switch (gameObject.name)
        {
            case string a when a.Contains("BaseEnemy"):
                _enemyManager.CurrentBaseEnemyMove = _enemyManager.BaseEnemy.Move;
                _enemyManager.DefaultBaseEnemyMove = _enemyManager.BaseEnemy.Move;
                _enemyNav.speed = _enemyManager.CurrentBaseEnemyMove;
                break;
            case string a when a.Contains("Brute"):
                _enemyManager.CurrentBruteMove = _enemyManager.Brute.Move;
                _enemyManager.DefaultBruteMove = _enemyManager.Brute.Move;
                _enemyNav.speed = _enemyManager.CurrentBaseEnemyMove;
                break;
            case string a when a.Contains("Mage"):
                _enemyManager.CurrentMageMove = _enemyManager.Mage.Move;
                _enemyNav.speed = _enemyManager.CurrentMageMove;
                break;
            case string a when a.Contains("Borrek"):
                _miniBossManager.CurrentBorrekMove = _miniBossManager.Borrek.Move;
                _enemyNav.speed = _miniBossManager.CurrentBorrekMove;
                break;
        }

        //Move to the closest player if it can do so
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
        switch (gameObject.name)
        {
            case string a when a.Contains("BaseEnemy"):
                _enemyNav.speed = _enemyManager.BaseEnemy.Move;
                break;
            case string a when a.Contains("Brute"):
                _enemyNav.speed = _enemyManager.Brute.Move;
                break;
            case string a when a.Contains("Mage"):
                _enemyNav.speed = _enemyManager.Mage.Move;
                break;
            case string a when a.Contains("Borrek"):
                _enemyNav.speed = _miniBossManager.Borrek.Move;
                break;
        }
    }
}
