using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSelectMenu : MonoBehaviour
{
    private PlayerInput _playerInput;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        var rootMenu = GameObject.Find("MainMenuUI").transform.Find("PlayerSelect").Find("PlayerSelectBackground");
        var uiHelp = rootMenu.Find("PressHelp");
        if(rootMenu != null)
        {
            if(_playerInput.playerIndex == 0)
            {
                var menu = Instantiate(PlayerConfigManager.Instance.PlayerSetupMenu, rootMenu.Find("TopLeftPos"));
                uiHelp.Find("TopLeft").gameObject.SetActive(false);
                _playerInput.uiInputModule = menu.transform.GetChild(0).GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectMenuController>().SetPlayerIndex(_playerInput.playerIndex);
            }
            else if (_playerInput.playerIndex == 1)
            {
                var menu = Instantiate(PlayerConfigManager.Instance.PlayerSetupMenu, rootMenu.Find("TopRightPos"));
                uiHelp.Find("TopRight").gameObject.SetActive(false);
                _playerInput.uiInputModule = menu.transform.GetChild(0).GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectMenuController>().SetPlayerIndex(_playerInput.playerIndex);
            }
            else if (_playerInput.playerIndex == 2)
            {
                var menu = Instantiate(PlayerConfigManager.Instance.PlayerSetupMenu, rootMenu.Find("BottomLeftPos"));
                uiHelp.Find("BottomLeft").gameObject.SetActive(false);
                _playerInput.uiInputModule = menu.transform.GetChild(0).GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectMenuController>().SetPlayerIndex(_playerInput.playerIndex);
            }
            else if (_playerInput.playerIndex == 3)
            {
                var menu = Instantiate(PlayerConfigManager.Instance.PlayerSetupMenu, rootMenu.Find("BottomRightPos"));
                uiHelp.Find("BottomRight").gameObject.SetActive(false);
                _playerInput.uiInputModule = menu.transform.GetChild(0).GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<PlayerSelectMenuController>().SetPlayerIndex(_playerInput.playerIndex);
            }
        }
    }
}
