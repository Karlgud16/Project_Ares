using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [Header("BaseEnemyStats")]
    public float BaseEnemyMoveSpeed;
    [ReadOnly] public float DefaultBaseEnemyMoveSpeed;
    public float BaseEnemyHealth;
    public float BaseEnemyAttack;
    public float BaseEnemySecondsUntilDelete;
    public GameObject BaseEnemyPrefab;
    public int AmountOfBaseEnemiesAtOnce;
    [ReadOnly] public int AmountOfBaseEnemies;
    [ReadOnly] public List<GameObject> ListOfBaseEnemies;

    [Header("SummonerStats")]
    public float SummonerHealth;
    public float SummonerSecondsUntilDelete;
    [ReadOnly] public List<GameObject> ListOfSummoners;

    [Header("BruteStats")]
    public float BruteMoveSpeed;
    [ReadOnly] public float DefaultBruteMoveSpeed;
    public float BruteHealth;
    public float BruteBaseAttack;
    public float BruteSecondsUntilDelete;
    [ReadOnly] public List<GameObject> ListOfBrutes;

    [Header("MageStats")]
    public float MageMoveSpeed;
    public float MageHealth;
    public float MageSecondsUntilDelete;
    public GameObject Projectile;
    public float ProjectileSpeed;
    public float ProjectileAttack;
    public float ProjectileSecondsUntilDelete;
    [ReadOnly] public List<GameObject> ListOfMages;

    [Header("AmountOfEnemies")]
    [ReadOnly] public List<GameObject> AmountOfEnemies;

    void Start()
    {
        DefaultBaseEnemyMoveSpeed = BaseEnemyMoveSpeed;
        DefaultBruteMoveSpeed = BruteMoveSpeed;
    }

    void Update()
    {
        AmountOfBaseEnemies = GameObject.FindGameObjectsWithTag("BaseEnemy").Length;
    }
}
