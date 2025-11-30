//Script that handles logic for the game state

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    public bool IsPaused;
    [ReadOnly] public bool PauseState;
    [ReadOnly] public bool CanPause;
    [ReadOnly] public float CurrentTimeScale;
    [ReadOnly] public bool FreeCam;
    [ReadOnly] public bool GameStarted;
    [ReadOnly] public bool GameWon;
    [ReadOnly] public bool HealthSet;
    private bool _startWonScreen;
    public float ComboTimer;

    public static GameManager Instance;

    private PlayerManager _playerManager;

    [ReadOnly] public MenuManager MenuManager;
    private EventSystem _eventSystem;

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
        _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    void Start()
    {
        CurrentTimeScale = Time.timeScale;
        FreeCam = false;
        CanPause = true;
        GameWon = false;
        _startWonScreen = false;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.name != "MainMenu")
        {
            MenuManager = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuManager>();
            CurrentTimeScale = Time.timeScale;
            FreeCam = false;
            CanPause = true;
            GameWon = false;
            _startWonScreen = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        PauseLogic();

        if(PauseState == true)
        {
            IsPaused = true;
            MenuManager.PauseMenu.SetActive(true);
            MenuManager.Hud.SetActive(false);
            _eventSystem.SetSelectedGameObject(MenuManager.ResumeButton);
            PauseState = false;
        }

        if(_playerManager.PlayerIsDead)
        {
            if(CanPause == true)
            {
                StartCoroutine(LoseMenu());
                CanPause = false;
            }
        }

        if (GameWon && _startWonScreen == false)
        {
            MenuManager.WonMenu.SetActive(true);
            MenuManager.Hud.SetActive(false);
            _eventSystem.SetSelectedGameObject(MenuManager.NextLevelButton);
            IsPaused = true;
            _startWonScreen = true;
        }
    }

    void PauseLogic()
    {
        //If IsPause is true and FreeCam is not true (PauseMenu, Console Menu)
        if (IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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

    IEnumerator LoseMenu()
    {
        yield return new WaitForSeconds(1.5f);
        MenuManager.LoseMenu.SetActive(true);
        MenuManager.Hud.SetActive(false);
        _eventSystem.SetSelectedGameObject(MenuManager.RestartButton);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ResetBools()
    {
        PauseState = false;
        CanPause = true;
        FreeCam = false;
        GameStarted = false;
        GameWon = false;
        HealthSet = false;
    }
}
