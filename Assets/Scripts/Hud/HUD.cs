//Script that handles the HUD of the player

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Slider _healthBar;

    private PlayerManager _playerManager;

    void Start()
    {
        _healthBar = gameObject.transform.GetChild(0).GetComponent<Slider>();

        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if(_healthBar.maxValue != _playerManager.StartHealth)
        {
            _healthBar.maxValue = _playerManager.StartHealth;
        }
        _healthBar.value = _playerManager.CurrentPlayerHealth;
    }
}
