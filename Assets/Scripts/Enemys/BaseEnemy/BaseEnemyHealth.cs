//Handles the Health of all enemies

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemyHealth : MonoBehaviour
{
    private Section _section;

    private Animator _animator;

    private Slider _healthBar;

    [ReadOnly] public bool CanMove;

    [ReadOnly] public bool IsDead;

    [ReadOnly] [SerializeField] public float CurrentHealth;

    private SummonEnemies _summonEnemies;

    private BaseEnemyFlip _enemyFlip;

    private HealthSystem _healthSystem;

    private bool _toggleAnubisAnkh;

    private ItemManager _itemManager;

    private PlayerManager _playerManager;

    private EnemyManager _enemyManager;

    void Awake()
    {
        if(transform.parent != null)
        {
            _section = transform.parent.GetComponent<Section>();
        }
        _animator = GetComponent<Animator>();
        _healthBar = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        _enemyFlip = GetComponent<BaseEnemyFlip>();
        _healthSystem = GameObject.FindGameObjectWithTag("healthSystem").GetComponent<HealthSystem>();

        if (gameObject.tag == "Summoner")
        {
            _summonEnemies = GetComponent<SummonEnemies>();
        }
    }

    void Start()
    {
        _enemyManager = GameManager.Instance.GetComponent<EnemyManager>();
        _enemyManager.AmountOfEnemies.Add(gameObject);

        _itemManager = GameManager.Instance.GetComponent<ItemManager>();
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();

        //Applies the right health towards the certain enemy
        switch (gameObject.tag) 
        {
            case "BaseEnemy":
                CurrentHealth = _enemyManager.BaseEnemyHealth;
                _enemyManager.ListOfBaseEnemies.Add(gameObject);
                break;
            case "Summoner":
                CurrentHealth = _enemyManager.SummonerHealth;
                break;
            case "Brute":
                CurrentHealth = _enemyManager.BruteHealth;
                break;
            case "Mage":
                CurrentHealth = _enemyManager.MageHealth;
                break;
        }
        CanMove = true;
        IsDead = false;
        _healthBar.maxValue = CurrentHealth;
        _toggleAnubisAnkh = true;
    }

    void Update()
    {
        _healthBar.value = CurrentHealth;
        if (CurrentHealth <= 0)
        {
            if (gameObject.tag == "Summoner" && _summonEnemies != null)
            {
                KillEnemySpawned();
                _summonEnemies.CanSummon = false;
            }
            StartCoroutine(Death());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "lightAttack" && !IsDead)
        {
            CurrentHealth -= _playerManager.PlayerLightAttack;
            _enemyManager.gameObject.GetComponent<ComboSystem>().ComboAmount++;
            _enemyManager.gameObject.GetComponent<ComboSystem>().ComboTimer = 0;
            _animator.SetTrigger("Hit");
        }
        else if (other.gameObject.tag == "heavyAttack" && !IsDead)
        {
            CurrentHealth -= _playerManager.PlayerHeavyAttack;
            _enemyManager.gameObject.GetComponent<ComboSystem>().ComboAmount++;
            _animator.SetTrigger("Hit");
        }
    }

    public IEnumerator Death()
    {
        _animator.SetTrigger("Death");
        if(_section != null)
        {
            _section.EnemyCount--;
        }
        _enemyFlip.canFlip = false;
        CanMove = false;
        IsDead = true;
        if(_enemyManager.GetComponent<ItemManager>().AnubisAnkh == true && !_playerManager.PlayerIsDead && _toggleAnubisAnkh)
        {
            _healthSystem.PlayerCurrentHealth += _playerManager.PlayerHealth / 10f;
            if(_healthSystem.PlayerCurrentHealth > _playerManager.PlayerHealth)
            {
                _healthSystem.PlayerCurrentHealth = _playerManager.PlayerHealth;
            }
            _toggleAnubisAnkh = false;
        }
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                yield return new WaitForSeconds(_enemyManager.BaseEnemySecondsUntilDelete);
                break;
            case "Summoner":
                yield return new WaitForSeconds(_enemyManager.SummonerSecondsUntilDelete);
                break;
            case "Brute":
                yield return new WaitForSeconds(_enemyManager.BruteSecondsUntilDelete);
                break;
            case "Mage":
                yield return new WaitForSeconds(_enemyManager.MageSecondsUntilDelete);
                break;
        }
        Instantiate(_itemManager.HealthPickupObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void KillEnemySpawned()
    {
        //Sets all of the enemies health spawned by the summoner to 0
        switch (_summonEnemies.AmountOfEnemies)
        {
            case 1:
                foreach (Transform child in _summonEnemies.SpawnPoints[0].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                return;
            case 2:
                foreach (Transform child in _summonEnemies.SpawnPoints[0].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                foreach (Transform child in _summonEnemies.SpawnPoints[1].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                return;
            case 3:
                foreach (Transform child in _summonEnemies.SpawnPoints[0].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                foreach (Transform child in _summonEnemies.SpawnPoints[1].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                foreach (Transform child in _summonEnemies.SpawnPoints[2].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                return;
            case 4:
                foreach (Transform child in _summonEnemies.SpawnPoints[0].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                foreach (Transform child in _summonEnemies.SpawnPoints[1].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                foreach (Transform child in _summonEnemies.SpawnPoints[2].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                foreach (Transform child in _summonEnemies.SpawnPoints[3].transform)
                {
                    child.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
                }
                return;
        }
    }

    void OnDestroy()
    {
        _enemyManager.AmountOfEnemies.Remove(gameObject);
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                _enemyManager.ListOfBaseEnemies.Remove(gameObject);
                break;
            case "Summoner":
                _enemyManager.ListOfSummoners.Remove(gameObject);
                break;
            case "Brute":
                _enemyManager.ListOfBrutes.Remove(gameObject);
                break;
            case "Mage":
                _enemyManager.ListOfMages.Remove(gameObject);
                break;
        }
    }

    //Resets the enemies health and the max value of the enemy health bar
    public void DebugChangeHealth()
    {
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                CurrentHealth = _enemyManager.BaseEnemyHealth;
                _healthBar.maxValue = _enemyManager.BaseEnemyHealth;
                break;
            case "Summoner":
                CurrentHealth = _enemyManager.SummonerHealth;
                _healthBar.maxValue = _enemyManager.SummonerHealth;
                break;
            case "Brute":
                CurrentHealth = _enemyManager.BruteHealth;
                _healthBar.maxValue = _enemyManager.BruteHealth;
                break;
            case "Mage":
                CurrentHealth = _enemyManager.MageHealth;
                _healthBar.maxValue = _enemyManager.MageHealth;
                break;
        }
    }
}
