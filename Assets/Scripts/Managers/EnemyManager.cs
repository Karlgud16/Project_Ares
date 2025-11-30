//Handles all of the enemy stats

using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [Header("BaseEnemyStats")]
    public GameObject BaseEnemyPrefab;
    public int AmountOfBaseEnemiesAtOnce;
    [ReadOnly] public int AmountOfBaseEnemies;
    [ReadOnly] public List<GameObject> ListOfBaseEnemies;
    public EnemyScriptableObject BaseEnemy;
    [ReadOnly] public float CurrentBaseEnemyMove;
    [ReadOnly] public float DefaultBaseEnemyMove;

    [Header("SummonerStats")]
    [ReadOnly] public List<GameObject> ListOfSummoners;
    public EnemyScriptableObject Summoner;

    [Header("BruteStats")]
    [ReadOnly] public List<GameObject> ListOfBrutes;
    public EnemyScriptableObject Brute;
    [ReadOnly] public float CurrentBruteMove;
    [ReadOnly] public float DefaultBruteMove;

    [Header("MageStats")]
    public GameObject Projectile;
    public float ProjectileSpeed;
    public float ProjectileAttack;
    public float ProjectileSecondsUntilDelete;
    [ReadOnly] public List<GameObject> ListOfMages;
    public EnemyScriptableObject Mage;
    public float CurrentMageMove;

    [Header("AmountOfEnemies")]
    [ReadOnly] public List<GameObject> AmountOfEnemies;

    void Start()
    {
        CurrentBaseEnemyMove = BaseEnemy.Move;
        CurrentBruteMove = Brute.Move;
    }

    void Update()
    {
        if(AmountOfBaseEnemies != GameObject.FindGameObjectsWithTag("BaseEnemy").Length)
        {
            AmountOfBaseEnemies = GameObject.FindGameObjectsWithTag("BaseEnemy").Length;
        }
    }
}
