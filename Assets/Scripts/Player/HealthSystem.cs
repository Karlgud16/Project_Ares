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

    void Start()
    {
        CanTakeDamage = true;
        debugSetHealth = false;
    }

    void Update()
    {
        if (GameManager.Instance.debugPlayer == true && debugSetHealth == false)
        {
            switch (GameManager.Instance.Players.Count)
            {
                case 1:
                    PlayerCurrentHealth = GameManager.Instance.PlayerHealth;
                    GameManager.Instance.StartHealth = PlayerCurrentHealth;
                    debugSetHealth = true;
                    Debug.Log("There is 1 player in the game");
                    break;
                case 2:
                    PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 2;
                    GameManager.Instance.StartHealth = PlayerCurrentHealth;
                    debugSetHealth = true;
                    Debug.Log("There is 2 players in the game");
                    break;
                case 3:
                    PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 3;
                    GameManager.Instance.StartHealth = PlayerCurrentHealth;
                    debugSetHealth = true;
                    Debug.Log("There is 3 players in the game");
                    break;
                case 4:
                    PlayerCurrentHealth = GameManager.Instance.PlayerHealth * 4;
                    GameManager.Instance.StartHealth = PlayerCurrentHealth;
                    debugSetHealth = true;
                    Debug.Log("There is 4 players in the game");
                    break;
                case 0:
                    Debug.Log("There is 0 players in the game");
                    debugSetHealth = true;
                    break;
            }
        }

        GameManager.Instance.CurrentPlayerHealth = PlayerCurrentHealth;

        //If player health is less than or equal to 0
        if (PlayerCurrentHealth <= 0)
        {
            if(GameManager.Instance.debugPlayer == true)
            {
                foreach (GameObject player in GameManager.Instance.Players)
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
            else
            {
                foreach (GameObject player in Players)
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
}
