using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfig> _playerConfigs;

    [SerializeField] public GameObject BaseEnemy;

    [SerializeField] public GameObject PlayerSetupMenu;

    public static PlayerConfigManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("There should only be one 'PlayerConfigManager' at once (Stop the scene and delete other 'PlayerConfigManager's :))");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _playerConfigs = new List<PlayerConfig>();
        }
    }

    public List<PlayerConfig> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public void SetPlayerType(int index, GameObject player)
    {
        _playerConfigs[index].PlayerType = player;
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;
        if(_playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("TestScene");
        }
    }

    public void HandlePlayerInput(PlayerInput pi)
    {
        Debug.Log("Player " + pi.playerIndex + " joined");
        if(!_playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            _playerConfigs.Add(new PlayerConfig(pi));
            pi.transform.SetParent(transform);
        }
    }
}

public class PlayerConfig 
{
    public PlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public GameObject PlayerType { get; set; }
}