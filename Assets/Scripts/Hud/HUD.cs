//Script that handles the HUD of the player

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    Slider _healthBar;

    void Start()
    {
        _healthBar = gameObject.transform.GetChild(0).GetComponent<Slider>();
    }

    void Update()
    {
        //Checks if the max value is not equal to what the player health should be (Debug Console)
        if(_healthBar.maxValue != GameManager.Instance.PlayerHealth)
        {
            _healthBar.maxValue = GameManager.Instance.PlayerHealth;
        }

        _healthBar.value = GameManager.Instance.CurrentPlayerHealth;
    }
}
