using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawns;
    [SerializeField] private GameObject _playerPrefab;

    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GameObject.FindGameObjectWithTag("healthSystem").GetComponent<HealthSystem>();
    }

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(PlayerConfigManager.Instance.BaseEnemy, _playerSpawns[i].position, _playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
            _healthSystem.Players.Add(player);
            GameManager.Instance.Players.Add(player);
        }

        //Set how much health the player/s should have
        switch (GameManager.Instance.Players.Count)
        {
            case 1:
                _healthSystem.PlayerCurrentHealth = GameManager.Instance.PlayerHealth;
                Debug.Log("There is 1 player in the game");
                break;
            case 2:
                _healthSystem.PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 2;
                Debug.Log("There is 2 players in the game");
                break;
            case 3:
                _healthSystem.PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 3;
                Debug.Log("There is 3 players in the game");
                break;
            case 4:
                _healthSystem.PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 4;
                Debug.Log("There is 4 players in the game");
                break;
            case 0:
                Debug.Log("There is 0 players in the game");
                break;
        }
    }
}
