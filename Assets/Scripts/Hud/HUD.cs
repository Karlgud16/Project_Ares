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
        if(_healthBar.maxValue != GameManager.Instance.StartHealth)
        {
            _healthBar.maxValue = GameManager.Instance.StartHealth;
        }
        _healthBar.value = GameManager.Instance.CurrentPlayerHealth;
    }
}
