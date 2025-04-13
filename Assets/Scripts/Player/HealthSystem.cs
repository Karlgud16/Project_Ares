//Handles all of the health for all of the players

using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private GameObject[] _players;
    private Animator[] _animators;

    [ReadOnly] public float PlayerCurrentHealth;

    [ReadOnly] public bool CanTakeDamage;
    void Awake()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Start()
    {
        CanTakeDamage = true;

        //Set how much health the player/s should have
        switch (_players.Length)
        {
            case 1:
                PlayerCurrentHealth = GameManager.Instance.PlayerHealth;
                Debug.Log("There is 1 player in the game");
                break;
            case 2:
                PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 2;
                Debug.Log("There is 2 players in the game");
                break;
            case 3:
                PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 3;
                Debug.Log("There is 3 players in the game");
                break;
            case 4:
                PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 4;
                Debug.Log("There is 4 players in the game");
                break;
            case 0:
                Debug.Log("There is 0 players in the game");
                break;
        }
    }

    void Update()
    {
        GameManager.Instance.CurrentPlayerHealth = PlayerCurrentHealth;

        //If player health is less than or equal to 0
        if (PlayerCurrentHealth <= 0)
        {
            foreach (var player in _players)
            {
                //Play death animation and disable the players actions
                player.GetComponent<Animator>().SetTrigger("Death");
                GameManager.Instance.PlayerIsDead = true;
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
