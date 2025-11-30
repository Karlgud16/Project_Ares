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

    [ReadOnly] public float CurrentHealth;

    private SummonEnemies _summonEnemies;

    private BaseEnemyFlip _enemyFlip;

    private HealthSystem _healthSystem;

    private bool _toggleAnubisAnkh;

    private ItemManager _itemManager;

    private PlayerManager _playerManager;

    private EnemyManager _enemyManager;

    private MiniBossManager _miniBossManager;

    void Awake()
    {
        if(transform.parent != null)
        {
            _section = transform.parent.GetComponent<Section>();
        }
        _animator = GetComponent<Animator>();
        _healthBar = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        _enemyFlip = GetComponent<BaseEnemyFlip>();

        if (gameObject.tag == "Summoner")
        {
            _summonEnemies = GetComponent<SummonEnemies>();
        }
    }

    void Start()
    {
        _healthSystem = GameManager.Instance.GetComponent<HealthSystem>();
        _enemyManager = GameManager.Instance.GetComponent<EnemyManager>();
        _enemyManager.AmountOfEnemies.Add(gameObject);

        _miniBossManager = GameManager.Instance.GetComponent<MiniBossManager>();

        _itemManager = GameManager.Instance.GetComponent<ItemManager>();
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();

        //Applies the right health towards the certain enemy
        switch (gameObject.name)
        {
            case string a when a.Contains("BaseEnemy"):
                CurrentHealth = _enemyManager.BaseEnemy.Health;
                _enemyManager.ListOfBaseEnemies.Add(gameObject);
                break;
            case string a when a.Contains("Summoner"):
                CurrentHealth = _enemyManager.Summoner.Health;
                break;
            case string a when a.Contains("Brute"):
                CurrentHealth = _enemyManager.Brute.Health;
                break;
            case string a when a.Contains("Mage"):
                CurrentHealth = _enemyManager.Mage.Health;
                break;
            case string a when a.Contains("Borrek"):
                CurrentHealth = _miniBossManager.Borrek.Health;
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
            CurrentHealth -= _playerManager.DefaultPlayer.LightAttack;
            _enemyManager.gameObject.GetComponent<ComboSystem>().ComboAmount++;
            _enemyManager.gameObject.GetComponent<ComboSystem>().ComboTimer = 0;
            _animator.SetTrigger("Hit");
        }
        else if (other.gameObject.tag == "heavyAttack" && !IsDead)
        {
            CurrentHealth -= _playerManager.DefaultPlayer.HeavyAttack;
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
        foreach (ItemList i in _itemManager.Items)
        {
            i.item.OnEnemyDeath(_itemManager, this, i.stacks);
        }

        switch (gameObject.name)
        {
            case string a when a.Contains("BaseEnemy"):
                yield return new WaitForSeconds(_enemyManager.BaseEnemy.SecondsUntilDelete);
                break;
            case string a when a.Contains("Summoner"):
                yield return new WaitForSeconds(_enemyManager.Summoner.SecondsUntilDelete);
                break;
            case string a when a.Contains("Brute"):
                yield return new WaitForSeconds(_enemyManager.Brute.SecondsUntilDelete);
                break;
            case string a when a.Contains("Mage"):
                yield return new WaitForSeconds(_enemyManager.Mage.SecondsUntilDelete);
                break;
            case string a when a.Contains("Borrek"):
                yield return new WaitForSeconds(_miniBossManager.Borrek.SecondsUntilDelete);
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
                CurrentHealth = _enemyManager.BaseEnemy.Health;
                _healthBar.maxValue = _enemyManager.BaseEnemy.Health;
                break;
            case "Summoner":
                CurrentHealth = _enemyManager.Summoner.Health;
                _healthBar.maxValue = _enemyManager.Summoner.Health;
                break;
            case "Brute":
                CurrentHealth = _enemyManager.Brute.Health;
                _healthBar.maxValue = _enemyManager.Brute.Health;
                break;
            case "Mage":
                CurrentHealth = _enemyManager.Mage.Health;
                _healthBar.maxValue = _enemyManager.Mage.Health;
                break;
        }
    }
}
