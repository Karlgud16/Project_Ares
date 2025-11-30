//Handles local co-op logic (saves how many players there are and their player type)

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfig> _playerConfigs;

    public GameObject BaseEnemy;

    public GameObject PlayerSetupMenu;

    [SerializeField] private GameObject _gameManager;

    public static PlayerConfigManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("There should only be one 'PlayerConfigManager' (Stop the scene and delete other 'PlayerConfigManager's :))");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _playerConfigs = new List<PlayerConfig>();
        }
    }

    /// <summary>
    /// Returns the list of Player Configs
    /// </summary>
    public List<PlayerConfig> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    /// <summary>
    /// Sets what type of Character the player is
    /// </summary>
    /// <param name="index"></param>
    /// <param name="player"></param>
    public void SetPlayerType(int index, GameObject player)
    {
        _playerConfigs[index].PlayerType = player;
    }

    /// <summary>
    /// Sets IsReady to True to that specific player
    /// </summary>
    /// <param name="index"></param>
    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady = true;
        if(_playerConfigs.All(p => p.IsReady == true))
        {
            var manager = Instantiate(_gameManager);
            DontDestroyOnLoad(manager);
            SceneManager.LoadScene("LevelLookInspo 1");
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