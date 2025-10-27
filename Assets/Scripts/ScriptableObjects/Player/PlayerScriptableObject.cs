//Handles the values of the player
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 2)]
public class PlayerScriptableObject : ScriptableObject
{
    [Tooltip("Player move speed")]
    public float Speed;
    [Tooltip("Player jump height")]
    public float Jump;
    [Tooltip("How much HP does the light Attack take off an enemy")]
    public float LightAttack;
    [Tooltip("How much HP does the heavy Attack take off an enemy")]
    public float HeavyAttack;
    [Tooltip("How fast the player can dash when doing a dodge")]
    public float DodgeSpeed;
    [Tooltip("How long the player dodges for")]
    public float DodgeDuration;
    [Tooltip("How much stamina does the player start off with")]
    public float Stamina;
    [Tooltip("How much stamina is taken off if the player does a light attack")]
    public float LightAttackDrain;
    [Tooltip("How much stamina is taken off if the player does a heavy attack")]
    public float HeavyAttackDrain;
    [Tooltip("How much stamina is taken off if the player does a dodge")]
    public float DodgeStaminaDrain;
    [Tooltip("How long it takes for the stamina to regenerate")]
    public float StaminaWait; 
    [Tooltip("How quick the stamina takes to regenerate")]
    public float StaminaRegen;
}
