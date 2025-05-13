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
        GameManager.Instance.AmountOfEnemies.Add(gameObject);

        //Applies the right health towards the certain enemy
        switch (gameObject.tag) 
        {
            case "BaseEnemy":
                CurrentHealth = GameManager.Instance.BaseEnemyHealth;
                GameManager.Instance.ListOfBaseEnemies.Add(gameObject);
                break;
            case "Summoner":
                CurrentHealth = GameManager.Instance.SummonerHealth;
                break;
            case "Brute":
                CurrentHealth = GameManager.Instance.BruteHealth;
                break;
            case "Mage":
                CurrentHealth = GameManager.Instance.MageHealth;
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
            CurrentHealth -= GameManager.Instance.PlayerLightAttack;
            GameManager.Instance.gameObject.GetComponent<ComboSystem>().ComboAmount++;
            GameManager.Instance.gameObject.GetComponent<ComboSystem>().ComboTimer = 0;
            _animator.SetTrigger("Hit");
        }
        else if (other.gameObject.tag == "heavyAttack" && !IsDead)
        {
            CurrentHealth -= GameManager.Instance.PlayerHeavyAttack;
            GameManager.Instance.gameObject.GetComponent<ComboSystem>().ComboAmount++;
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
        if(GameManager.Instance.GetComponent<ItemManager>().AnubisAnkh == true && !GameManager.Instance.PlayerIsDead && _toggleAnubisAnkh)
        {
            _healthSystem.PlayerCurrentHealth += GameManager.Instance.PlayerHealth / 10f;
            if(_healthSystem.PlayerCurrentHealth > GameManager.Instance.PlayerHealth)
            {
                _healthSystem.PlayerCurrentHealth = GameManager.Instance.PlayerHealth;
            }
            _toggleAnubisAnkh = false;
        }
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                yield return new WaitForSeconds(GameManager.Instance.BaseEnemySecondsUntilDelete);
                break;
            case "Summoner":
                yield return new WaitForSeconds(GameManager.Instance.SummonerSecondsUntilDelete);
                break;
            case "Brute":
                yield return new WaitForSeconds(GameManager.Instance.BruteSecondsUntilDelete);
                break;
            case "Mage":
                yield return new WaitForSeconds(GameManager.Instance.MageSecondsUntilDelete);
                break;
        }
        Instantiate(GameManager.Instance.HealthPickupObject, transform.position, Quaternion.identity);
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
        GameManager.Instance.AmountOfEnemies.Remove(gameObject);
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                GameManager.Instance.ListOfBaseEnemies.Remove(gameObject);
                break;
            case "Summoner":
                GameManager.Instance.ListOfSummoners.Remove(gameObject);
                break;
            case "Brute":
                GameManager.Instance.ListOfBrutes.Remove(gameObject);
                break;
            case "Mage":
                GameManager.Instance.ListOfMages.Remove(gameObject);
                break;
        }
    }

    //Resets the enemies health and the max value of the enemy health bar
    public void DebugChangeHealth()
    {
        switch (gameObject.tag)
        {
            case "BaseEnemy":
                CurrentHealth = GameManager.Instance.BaseEnemyHealth;
                _healthBar.maxValue = GameManager.Instance.BaseEnemyHealth;
                break;
            case "Summoner":
                CurrentHealth = GameManager.Instance.SummonerHealth;
                _healthBar.maxValue = GameManager.Instance.SummonerHealth;
                break;
            case "Brute":
                CurrentHealth = GameManager.Instance.BruteHealth;
                _healthBar.maxValue = GameManager.Instance.BruteHealth;
                break;
            case "Mage":
                CurrentHealth = GameManager.Instance.MageHealth;
                _healthBar.maxValue = GameManager.Instance.MageHealth;
                break;
        }
    }
}
