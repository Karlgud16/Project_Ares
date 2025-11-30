//Handles all of the health for all of the players

using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public List<GameObject> Players;
    private Animator[] _animators;

    [ReadOnly] public float PlayerCurrentHealth;

    [ReadOnly] public bool CanTakeDamage;

    private bool debugSetHealth;

    private PlayerManager _playerManager;

    void Start()
    {
        CanTakeDamage = true;
        debugSetHealth = false;

        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if(_playerManager.CurrentPlayerHealth != PlayerCurrentHealth)
        {
            _playerManager.CurrentPlayerHealth = PlayerCurrentHealth;
        }

        //Setting the players health if the user is using the debug player
        if (_playerManager.debugPlayer == true && debugSetHealth == false && GameManager.Instance.HealthSet == false)
        {
            switch (_playerManager.Players.Count)
            {
                case 1:
                    PlayerCurrentHealth = _playerManager.PlayerHealth;
                    Debug.Log("There is 1 player in the game");
                    break;
                case 2:
                    PlayerCurrentHealth = _playerManager.PlayerHealth * 2;
                    Debug.Log("There is 2 players in the game");
                    break;
                case 3:
                    PlayerCurrentHealth = _playerManager.PlayerHealth * 3;
                    Debug.Log("There is 3 players in the game");
                    break;
                case 4:
                    PlayerCurrentHealth = _playerManager.PlayerHealth * 4;
                    Debug.Log("There is 4 players in the game");
                    break;
                case 0:
                    Debug.Log("There is 0 players in the game");
                    debugSetHealth = true;
                    GameManager.Instance.GameStarted = true;
                    break;
            }

            _playerManager.StartHealth = PlayerCurrentHealth;
            _playerManager.CurrentPlayerHealth = PlayerCurrentHealth;
            debugSetHealth = true;
            GameManager.Instance.GameStarted = true;
            GameManager.Instance.HealthSet = true;
        }

        //If player health is less than or equal to 0
        if (PlayerCurrentHealth <= 0 && GameManager.Instance.HealthSet)
        {
            if(_playerManager.debugPlayer == true)
            {
                foreach (GameObject player in _playerManager.Players)
                {
                    //Play death animation and disable the players actions
                    player.GetComponent<Animator>().SetTrigger("Death");
                    _playerManager.PlayerIsDead = true;
                    player.GetComponent<PlayerMovement>().CanMove = false;
                    player.GetComponent<PlayerMovement>().CanBlock = false;
                    player.GetComponent<PlayerMovement>().CanJump = false;
                    player.GetComponent<PlayerMovement>().CanDodge = false;
                    player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
                    player.GetComponent<PlayerAttack>().CanLightAttack = false;
                }
            }
            else
            {
                foreach (GameObject player in Players)
                {
                    //Play death animation and disable the players actions
                    player.GetComponent<Animator>().SetTrigger("Death");
                    _playerManager.PlayerIsDead = true;
                    player.GetComponent<PlayerMovement>().CanMove = false;
                    player.GetComponent<PlayerMovement>().CanBlock = false;
                    player.GetComponent<PlayerMovement>().CanJump = false;
                    player.GetComponent<PlayerMovement>().CanDodge = false;
                    player.GetComponent<PlayerAttack>().CanHeavyAttack = false;
                    player.GetComponent<PlayerAttack>().CanLightAttack = false;
                }
            }
        }
    }
}
