//Handles the Main Menu Logic

using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject _playerConfigManager;
    [SerializeField] private GameObject _localCOOp;
    [SerializeField] private GameObject _mainMenu;

    private void Start()
    {
        _playerConfigManager.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void LocalCo_Op()
    {
        _mainMenu.SetActive(false);
        _localCOOp.SetActive(true);
        _playerConfigManager.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
