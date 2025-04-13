//Script that handles the stats for the player and the enemies as well as gameplay values and logic

using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    public bool IsPaused;
    [ReadOnly] public float CurrentTimeScale;
    [ReadOnly] public bool FreeCam;
    public float ComboTimer;

    [Header("Items")]
    public List<ItemScriptableObject> ItemInventory;
    public GameObject ItemUIPrefab;
    public ItemScriptableObject HermesSwiftSandals;

    [Header("ItemsStats")]
    public float HealthPickup;
    public float HealthPickupMultiplier;
    public float HermesSwiftSandalsMultiplier;

    [Header("PlayerStats")]
    [ReadOnly] public GameObject Player;
    public float PlayerMoveSpeed;
    public float PlayerJump;
    public float PlayerHealth;
    [ReadOnly] public float CurrentPlayerHealth;
    public float PlayerLightAttack;
    public float PlayerHeavyAttack;
    public float PlayerDodgeSpeed;
    public float PlayerDodgeDuration;
    public float PlayerDodgeCooldown;
    public bool PlayerIsDead;

    [Header("BaseEnemyStats")]
    public float BaseEnemyMoveSpeed;
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

    public static GameManager Instance; 

    void Awake()
    {
        //Make itself an instance so it is easier to referance in scripts, deletes itself if the script is already in the scene
        if (Instance == null) 
        {
            Debug.Log("GameManager Made!");
            DontDestroyOnLoad(gameObject); 
            Instance = this;
        }
        else if (Instance != this) 
        {
            Debug.Log("Deleted a Game Manager as there should only be one Game Manager");
            Destroy(gameObject);
        }

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        CurrentTimeScale = Time.timeScale;
        FreeCam = false;
    }

    void Update()
    {
        PauseLogic();

        AmountOfBaseEnemies = GameObject.FindGameObjectsWithTag("BaseEnemy").Length; 
    }

    void PauseLogic()
    {
        //If IsPause is true and FreeCam is not true (PauseMenu, Console Menu)
        if (IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            Player.GetComponent<PlayerMovement>().CanMove = false;
            Player.GetComponent<PlayerMovement>().CanJump = false;
            Player.GetComponent<PlayerMovement>().CanBlock = false;
            Player.GetComponent<PlayerMovement>().DodgeToggle = false;
            Player.GetComponent<PlayerAttack>().CanLightAttack = false;
            Player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
        }
        //Else if IsPaused is not true and FreeCam is true(Player is using FreeCam) or the player has less than/equal to 0 health
        else if (!IsPaused && FreeCam || CurrentPlayerHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = CurrentTimeScale;
            Player.GetComponent<PlayerMovement>().CanMove = false;
            Player.GetComponent<PlayerMovement>().CanJump = false;
            Player.GetComponent<PlayerMovement>().CanBlock = false;
            Player.GetComponent<PlayerMovement>().DodgeToggle = false;
            Player.GetComponent<PlayerAttack>().CanLightAttack = false;
            Player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
        }
        //Else if IsPaused is not true and FreeCam is not true (Normal Game Play)
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = CurrentTimeScale;
            Player.GetComponent<PlayerMovement>().CanMove = true;
            Player.GetComponent<PlayerMovement>().CanJump = true;
            Player.GetComponent<PlayerMovement>().CanBlock = true;
            Player.GetComponent<PlayerMovement>().DodgeToggle = true;
            Player.GetComponent<PlayerAttack>().CanLightAttack = true;
            Player.GetComponent<PlayerAttack>().CanHeavyAttack = true;
        }
    }
}
