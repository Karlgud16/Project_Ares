//Script that handles the HUD of the player

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    [SerializeField] public Slider Player1Stamina;
    [SerializeField] public Slider Player2Stamina;
    [SerializeField] public Slider Player3Stamina;
    [SerializeField] public Slider Player4Stamina;

    [SerializeField] private PlayerManager _playerManager;

    void Awake()
    {
        _healthBar = gameObject.transform.GetChild(0).GetComponent<Slider>();

        Player1Stamina = gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Slider>();
        Player2Stamina = gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Slider>();
        Player3Stamina = gameObject.transform.GetChild(3).transform.GetChild(0).GetComponent<Slider>();
        Player4Stamina = gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<Slider>();
    }

    private void Start()
    {
        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();

        GameManager.Instance.GetComponent<PlayerManager>().Player1StaminaSlider = Player1Stamina;
        GameManager.Instance.GetComponent<PlayerManager>().Player2StaminaSlider = Player2Stamina;
        GameManager.Instance.GetComponent<PlayerManager>().Player3StaminaSlider = Player3Stamina;
        GameManager.Instance.GetComponent<PlayerManager>().Player4StaminaSlider = Player4Stamina;
    }

    void Update()
    {
        //Sets the Max Health value to the Starting Player Health
        if(_healthBar.maxValue != _playerManager.StartHealth)
        {
            _healthBar.maxValue = _playerManager.StartHealth;
        }

        _healthBar.value = _playerManager.CurrentPlayerHealth;
    }
}
