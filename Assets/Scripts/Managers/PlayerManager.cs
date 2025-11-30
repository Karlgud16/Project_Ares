//Handles the stats for the player

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("PlayerStats")]
    public bool debugPlayer;
    [ReadOnly] public List<GameObject> Players;
    private GameObject[] debugPlayersAmount;
    public float PlayerHealth;
    [ReadOnly] public float CurrentPlayerHealth;
    [ReadOnly] public float StartHealth;
    public PlayerScriptableObject DefaultPlayer;
    [ReadOnly] public float DefaultPlayerMoveSpeed;

    [Header("Player UI Stamina")]
    [ReadOnly] public Slider Player1StaminaSlider;
    [ReadOnly] public Slider Player2StaminaSlider;
    [ReadOnly] public Slider Player3StaminaSlider;
    [ReadOnly] public Slider Player4StaminaSlider;

    [Header("PlayerDeathCheck")]
    public bool PlayerIsDead;

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void Start()
    {
        DefaultPlayerMoveSpeed = DefaultPlayer.Speed;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        PlayerIsDead = false;

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
