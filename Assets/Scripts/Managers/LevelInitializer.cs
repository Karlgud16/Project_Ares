//Handles the spawning of the players in each level

using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawns;
    [SerializeField] private GameObject _playerPrefab;

    private HealthSystem _healthSystem;

    private PlayerManager _playerManager;

    private void Awake()
    {
        _healthSystem = GameObject.FindGameObjectWithTag("healthSystem").GetComponent<HealthSystem>();
    }

    void Start()
    {
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();

        if (_playerManager.debugPlayer == false)
        {
            var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
            for (int i = 0; i < playerConfigs.Length; i++)
            {
                var player = Instantiate(PlayerConfigManager.Instance.BaseEnemy, _playerSpawns[i].position, _playerSpawns[i].rotation, gameObject.transform);
                player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
                player.GetComponent<PlayerMovement>().PlayerID = i;
                _healthSystem.Players.Add(player);
                _playerManager.Players.Add(player);
            }

            //Set how much health the player/s should have
            switch (_playerManager.Players.Count)
            {
                case 1:
                    _healthSystem.PlayerCurrentHealth = _playerManager.PlayerHealth;
                    _playerManager.StartHealth = _healthSystem.PlayerCurrentHealth;
                    GameManager.Instance.GameStarted = true;
                    Debug.Log("There is 1 player in the game");
                    break;
                case 2:
                    _healthSystem.PlayerCurrentHealth = _playerManager.PlayerHealth * 2;
                    _playerManager.StartHealth = _healthSystem.PlayerCurrentHealth;
                    GameManager.Instance.GameStarted = true;
                    Debug.Log("There is 2 players in the game");
                    break;
                case 3:
                    _healthSystem.PlayerCurrentHealth = _playerManager.PlayerHealth * 3;
                    _playerManager.StartHealth = _healthSystem.PlayerCurrentHealth;
                    GameManager.Instance.GameStarted = true;
                    Debug.Log("There is 3 players in the game");
                    break;
                case 4:
                    _healthSystem.PlayerCurrentHealth = _playerManager.PlayerHealth * 4;
                    _playerManager.StartHealth = _healthSystem.PlayerCurrentHealth;
                    GameManager.Instance.GameStarted = true;
                    Debug.Log("There is 4 players in the game");
                    break;
                case 0:
                    Debug.Log("There is 0 players in the game");
                    break;
            }
        }
    }
}
