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

    [Header("Camera")]
    [ReadOnly] public bool CameraIsLocked;
    [Range(0, 1)]public float SmoothTime = 0.3f;
    public float MinZoom = 5f;
    public float MaxZoom = 12f;
    public float ZoomLimiter = 50f; // Max expected distance between players
    public Vector2 DeadZone = new Vector2(1f, 1f); // X and Y deadzone
    public Vector3 CameraOffset;
    public BoxCollider LevelBounds;
    public GameObject PlayerBounds;
    public float LeashLimitLeft = 10f; // Distance behind camera center on X axis
    public float LeashPushSpeed = 5f;  // Speed to move players forward

    [Header("Items")]
    public List<GameObject> ItemList;
    public List<ItemScriptableObject> ItemInventory;
    public List<GameObject> ItemInventoryGameObject;
    public List<GameObject> ItemHUDInventory;
    public GameObject ItemUIPrefab;
    public GameObject HealthPickupObject;
    public ItemScriptableObject HermesSwiftSandals;
    public ItemScriptableObject ValkyriesWingedBoots;
    public ItemScriptableObject AnubisAnkh;
    public ItemScriptableObject CursedSpurs;
    public ItemScriptableObject SlimeArmour;

    [Header("ItemsStats")]
    public GameObject ItemSpawner;
    public float HealthPickup;
    public float HealthPickupMultiplier;
    public float HermesSwiftSandalsMultiplier;
    public float ValkyriesWingedBootsMultiplier;
    public float AnubisAnkhHealth;
    [Range(0.01f, 0.99f)] public float SlimeArmourMultiplier;
    public float SlimeArmourSecondsUntilNormalSpeed;

    [Header("PlayerStats")]
    public bool debugPlayer;
    [ReadOnly] public List<GameObject> Players;
    private GameObject[] debugPlayersAmount;
    public float PlayerMoveSpeed;
    [ReadOnly] public float DefaultPlayerMoveSpeed;
    public float PlayerJump;
    public float PlayerHealth;
    [ReadOnly] public float CurrentPlayerHealth;
    [ReadOnly] public float StartHealth;
    public float PlayerLightAttack;
    public float PlayerHeavyAttack;
    public float PlayerDodgeSpeed;
    public float PlayerDodgeDuration;
    public float PlayerStamina;
    public float LightAttackStaminaDrain;
    public float HeavyAttackStaminaDrain;
    public float DodgeStaminaDrain;
    public float PlayerStaminaRegenWait;
    public float PlayerStaminaRegenSpeed;
    public bool PlayerIsDead;

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
    }

    void Start()
    {
        CurrentTimeScale = Time.timeScale;
        FreeCam = false;

        DefaultPlayerMoveSpeed = PlayerMoveSpeed;
        DefaultBaseEnemyMoveSpeed = BaseEnemyMoveSpeed;
        DefaultBruteMoveSpeed = BruteMoveSpeed;

        if(debugPlayer == true)
        {
            debugPlayersAmount = GameObject.FindGameObjectsWithTag("Player");
            if (debugPlayersAmount == null)
            {
                return;
            }
            else
            {
                foreach (GameObject player in debugPlayersAmount)
                {
                    Players.Add(player);
                }
            }

        }
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
            foreach(GameObject player in Players)
            {
                player.GetComponent<PlayerMovement>().CanMove = false;
                player.GetComponent<PlayerMovement>().CanJump = false;
                player.GetComponent<PlayerMovement>().CanBlock = false;
                player.GetComponent<PlayerMovement>().DodgeToggle = false;
                player.GetComponent<PlayerAttack>().CanLightAttack = false;
                player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
            }
        }
        //Else if IsPaused is not true and FreeCam is true(Player is using FreeCam) or the player has less than/equal to 0 health
        else if (!IsPaused && FreeCam || CurrentPlayerHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = CurrentTimeScale;
            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerMovement>().CanMove = false;
                player.GetComponent<PlayerMovement>().CanJump = false;
                player.GetComponent<PlayerMovement>().CanBlock = false;
                player.GetComponent<PlayerMovement>().DodgeToggle = false;
                player.GetComponent<PlayerAttack>().CanLightAttack = false;
                player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
            }
        }
        //Else if IsPaused is not true and FreeCam is not true (Normal Game Play)
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = CurrentTimeScale;
            foreach (GameObject player in Players)
            {
                player.GetComponent<PlayerMovement>().CanMove = true;
                player.GetComponent<PlayerMovement>().CanJump = true;
                player.GetComponent<PlayerMovement>().CanBlock = true;
                player.GetComponent<PlayerMovement>().DodgeToggle = true;
                player.GetComponent<PlayerAttack>().CanLightAttack = true;
                player.GetComponent<PlayerAttack>().CanHeavyAttack = true;
            }
        }
    }
}
