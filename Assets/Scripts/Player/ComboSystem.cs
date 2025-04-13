//Handles all of the combo's that the player would do when attacking

using TMPro;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{

    [ReadOnly] public float ComboTimer;

    private TextMeshProUGUI _comboText;

    [ReadOnly] public int ComboAmount;

    void Awake()
    {
        _comboText = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        ComboTimer = 0;
        _comboText.enabled = false;
    }

    void Update()
    {
        if (ComboAmount > 0)
        {
            _comboText.enabled = true;
            _comboText.text = "Combo: " + ComboAmount.ToString();

            ComboTimer += Time.deltaTime;

            if(ComboTimer >= GameManager.Instance.ComboTimer)
            {
                _comboText.enabled = false;
                ComboTimer = 0;
                ComboAmount = 0;
            }
        }
    }
}
