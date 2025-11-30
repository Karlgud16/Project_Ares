//Handles all of the Game Menu logic

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [ReadOnly] public GameObject PauseMenu;
    [ReadOnly] public GameObject ResumeButton;
    [ReadOnly] public GameObject LoseMenu;
    [ReadOnly] public GameObject RestartButton;
    [ReadOnly] public GameObject WonMenu;
    [ReadOnly] public GameObject NextLevelButton;
    [ReadOnly] public GameObject Hud;

    void Awake()
    {
        PauseMenu = transform.GetChild(0).gameObject;
        ResumeButton = transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
        Hud = GameObject.FindGameObjectWithTag("HUD");
        LoseMenu = transform.GetChild(1).gameObject;
        WonMenu = transform.GetChild(2).gameObject;
        RestartButton = transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        PauseMenu.SetActive(false);
        Hud.SetActive(true);
        LoseMenu.SetActive(false);
        WonMenu.SetActive(false);
    }

    public void Resume()
    {
        GameManager.Instance.IsPaused = false;
        PauseMenu.SetActive(false);
        Hud.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Destroy(GameManager.Instance.gameObject);
        Destroy(PlayerConfigManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        GameManager.Instance.GetComponent<ItemManager>().Items.Clear();
        Time.timeScale = 1.0f;
        GameManager.Instance.GetComponent<GameManager>().ResetBools();
        GameManager.Instance.GetComponent<PlayerManager>().PlayerIsDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        DontDestroyOnLoad(PlayerConfigManager.Instance);
        DontDestroyOnLoad(GameManager.Instance);
        GameManager.Instance.IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
