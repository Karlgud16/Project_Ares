using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("PlayerStats")]
    public bool debugPlayer;
    [ReadOnly] public List<GameObject> Players;
    private GameObject[] debugPlayersAmount;
    public float PlayerMoveSpeed;
    [ReadOnly] public float DefaultPlayerMoveSpeed;
    public float PlayerJump;
    public float PlayerHealth;
    [ReadOnly] public float CurrentPlayerHealth;
    [ReadOnly] public float StartHealth;
    public float PlayerLightAttack;
    public float PlayerHeavyAttack;
    public float PlayerDodgeSpeed;
    public float PlayerDodgeDuration;

    [Header("PlayerStamina")]

    public float PlayerStamina;
    public float LightAttackStaminaDrain;
    public float HeavyAttackStaminaDrain;
    public float DodgeStaminaDrain;
    public float PlayerStaminaRegenWait;
    public float PlayerStaminaRegenSpeed;

    [Header("PlayerDeathCheck")]

    public bool PlayerIsDead;

    private void Start()
    {
        DefaultPlayerMoveSpeed = PlayerMoveSpeed;

        if (debugPlayer == true)
        {
            debugPlayersAmount = GameObject.FindGameObjectsWithTag("Player");
            if (debugPlayersAmount == null)
            {
                return;
            }
            else
            {
                foreach (GameObject player in debugPlayersAmount)
                {
                    Players.Add(player);
                }
            }
        }
    }
}
