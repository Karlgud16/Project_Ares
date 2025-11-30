//Handles the spawning of the players in each level

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField][ReadOnly] private List<Transform> _playerSpawns;

    [SerializeField] private GameObject _playerSpawnsObject;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        _playerSpawns.Clear();

        _playerSpawnsObject = GameObject.FindGameObjectWithTag("PlayerSpawns");
        foreach (Transform spawn in _playerSpawnsObject.transform)
        {
            _playerSpawns.Add(spawn);
        }
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        //Setting the players health if the user is not using the debug player
        if (GetComponent<PlayerManager>().debugPlayer == false && GameManager.Instance.HealthSet == false)
        {
            GetComponent<HealthSystem>().Players.Clear();
            GetComponent<PlayerManager>().Players.Clear();

            var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
            for (int i = 0; i < playerConfigs.Length; i++)
            {
                var player = Instantiate(PlayerConfigManager.Instance.BaseEnemy, _playerSpawns[i].position, _playerSpawns[i].rotation);
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
                player.GetComponent<PlayerMovement>().PlayerID = i;
                GetComponent<HealthSystem>().Players.Add(player);
                GetComponent<PlayerManager>().Players.Add(player);
            }

            if (!GetComponent<GameManager>().HealthSet)
            {
                //Set how much health the player/s should have
                switch (GetComponent<PlayerManager>().Players.Count)
                {
                    case 1:
                        GetComponent<HealthSystem>().PlayerCurrentHealth = GetComponent<PlayerManager>().PlayerHealth;
                        Debug.Log("There is 1 player in the game");
                        break;
                    case 2:
                        GetComponent<HealthSystem>().PlayerCurrentHealth = GetComponent<PlayerManager>().PlayerHealth * 2;
                        GetComponent<PlayerManager>().CurrentPlayerHealth = GetComponent<PlayerManager>().PlayerHealth * 2;
                        Debug.Log("There is 2 players in the game");
                        break;
                    case 3:
                        GetComponent<HealthSystem>().PlayerCurrentHealth = GetComponent<PlayerManager>().PlayerHealth * 3;
                        GetComponent<PlayerManager>().CurrentPlayerHealth = GetComponent<PlayerManager>().PlayerHealth * 3;
                        Debug.Log("There is 3 players in the game");
                        break;
                    case 4:
                        GetComponent<HealthSystem>().PlayerCurrentHealth = GetComponent<PlayerManager>().PlayerHealth * 4;
                        GetComponent<PlayerManager>().CurrentPlayerHealth = GetComponent<PlayerManager>().PlayerHealth * 4;
                        Debug.Log("There is 4 players in the game");
                        break;
                    case 0:
                        Debug.Log("There is 0 players in the game");
                        break;
                }

                GetComponent<PlayerManager>().StartHealth = GetComponent<HealthSystem>().PlayerCurrentHealth;
                GameManager.Instance.GameStarted = true;
                GameManager.Instance.HealthSet = true;
            }
        }
    }
}
