//Handles the summoning of enemies on the summoner

using System.Collections.Generic;
using UnityEngine;

public class SummonEnemies : MonoBehaviour
{
    [Range(1, 4)] public int AmountOfEnemies;

    [SerializeField][Range(1, 10)] private float _timeBetweenSummon;

    [SerializeField][ReadOnly] public List<GameObject> SpawnPoints;

    private GameObject _spawnPointIndex;

    [ReadOnly] public bool StartSummon;
    [ReadOnly] public bool CanSummon;

    private BaseEnemyHealth _health;

    private float _timer;

    private Animator _animator;

    void Awake()
    {
        _spawnPointIndex = gameObject.transform.GetChild(1).gameObject;
        _health = transform.root.GetComponent<BaseEnemyHealth>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartSummon = false;
        CanSummon = false;

        //Gets all of the spawnpoints of the summoner and disables them
        foreach (Transform child in _spawnPointIndex.transform)
        {
            SpawnPoints.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        //Activates the spawn points depending on how many the summoner can spawn at once
        switch (AmountOfEnemies)
        {
            case 1:
                SpawnPoints[0].gameObject.SetActive(true);
                break;
            case 2:
                SpawnPoints[0].gameObject.SetActive(true);
                SpawnPoints[1].gameObject.SetActive(true);
                break;
            case 3:
                SpawnPoints[0].gameObject.SetActive(true);
                SpawnPoints[1].gameObject.SetActive(true);
                SpawnPoints[2].gameObject.SetActive(true);
                break;
            case 4:
                SpawnPoints[0].gameObject.SetActive(true);
                SpawnPoints[1].gameObject.SetActive(true);
                SpawnPoints[2].gameObject.SetActive(true);
                SpawnPoints[3].gameObject.SetActive(true);
                break;
        }
    }


    void Update()
    {
        //Stops summoning if the max amount of base enemies has been reached
        if (GameManager.Instance.AmountOfBaseEnemies >= GameManager.Instance.AmountOfBaseEnemiesAtOnce)
        {
            CanSummon = false;
        }
        else
        {
            CanSummon = true;
        }

        //If the summoner has triggered its summon, it can summon and the summoner and player is not dead
        if (StartSummon && CanSummon && !_health.IsDead && !GameManager.Instance.PlayerIsDead)
        {
            _animator.SetBool("isSummoning", true);

            //Spawns enemies in every certain amount of time
            if (_timer < _timeBetweenSummon)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                SummonEnemy();
                _timer = 0;
            }
        }
    }

    void SummonEnemy()
    {
        //Spawns in a base enemy depending on how many spawn points are active
        switch (AmountOfEnemies)
        {
            case 1:
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[0].transform);
                break;
            case 2:
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[0].transform);
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[1].transform);
                break;
            case 3:
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[0].transform);
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[1].transform);
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[2].transform);
                break;
            case 4:
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[0].transform);
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[1].transform);
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[2].transform);
                Instantiate(GameManager.Instance.BaseEnemyPrefab, SpawnPoints[3].transform);
                break;
        }
    }
}
