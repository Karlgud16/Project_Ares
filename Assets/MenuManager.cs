using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [ReadOnly] public GameObject PauseMenu;
    [ReadOnly] public GameObject ResumeButton;

    [ReadOnly] public GameObject Hud;

    void Awake()
    {
        PauseMenu = transform.GetChild(0).gameObject;
        ResumeButton = transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
        Hud = GameObject.FindGameObjectWithTag("HUD");
    }

    private void Start()
    {
        PauseMenu.SetActive(false);
        Hud.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene("MainMenu");
    }
}
