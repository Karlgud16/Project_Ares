//Script that handles the stats for the player and the enemies as well as gameplay values and logic

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    public bool IsPaused;
    [ReadOnly] public float CurrentTimeScale;
    [ReadOnly] public bool FreeCam;
    public float ComboTimer;

    public static GameManager Instance; 

    private PlayerManager _playerManager;

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

        _playerManager = GetComponent<PlayerManager>();
    }

    void Start()
    {
        CurrentTimeScale = Time.timeScale;
        FreeCam = false;
    }

    void Update()
    {
        PauseLogic();
    }

    void PauseLogic()
    {
        //If IsPause is true and FreeCam is not true (PauseMenu, Console Menu)
        if (IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            foreach(GameObject player in _playerManager.Players)
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
        else if (!IsPaused && FreeCam || _playerManager.CurrentPlayerHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = CurrentTimeScale;
            foreach (GameObject player in _playerManager.Players)
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
            foreach (GameObject player in _playerManager.Players)
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
