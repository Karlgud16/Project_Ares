using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSelectMenuController : MonoBehaviour
{
    private int _playerIndex;

    private bool _basicEnemy;
    private bool _basicEnemyToggle;
    private GameObject _basicEnemyUI;

    private GameObject _leftButton;
    private GameObject _rightButton;
    private GameObject _readyUpButton;
    private GameObject _readyText;

    private void Awake()
    {
        _basicEnemyUI = transform.GetChild(0).Find("Players").Find("Player0").gameObject;
        _leftButton = transform.GetChild(0).Find("LeftButton").gameObject;
        _rightButton = transform.GetChild(0).Find("RightButton").gameObject;
        _readyUpButton = transform.GetChild(0).Find("ReadyUpButton").gameObject;
        _readyText = transform.GetChild(0).Find("ReadyText").gameObject;
    }

    private void Start()
    {
        _leftButton.SetActive(true);
        _rightButton.SetActive(true);
        _readyUpButton.SetActive(true);
        _readyText.SetActive(false);

        _basicEnemyUI.SetActive(true);
    }

    private void Update()
    {
        if(_basicEnemyUI.activeInHierarchy && !_basicEnemy)
        {
            _basicEnemy = true;
        }
    }

    public void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
    }

    public void SetPlayer()
    {
        if (_basicEnemy == true)
        {
            PlayerConfigManager.Instance.SetPlayerType(_playerIndex, PlayerConfigManager.Instance.BaseEnemy);
            PlayerConfigManager.Instance.ReadyPlayer(_playerIndex);
            _basicEnemyUI.SetActive(false);
        }
        _leftButton.SetActive(false);
        _rightButton.SetActive(false);
        _readyUpButton.SetActive(false);
        _readyText.SetActive(true);
    }
}
