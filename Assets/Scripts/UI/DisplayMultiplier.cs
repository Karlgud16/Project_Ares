//Handles the multiplier text for the items

using TMPro;
using UnityEngine;

public class DisplayMultiplier : MonoBehaviour
{
    [ReadOnly] public int Multiplier;

    [SerializeField] private TextMeshProUGUI _multiplierText;

    private void Awake()
    {
        _multiplierText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Multiplier = 1;
    }

    private void Update()
    {
        if(Multiplier >= 2 && _multiplierText.text != Multiplier.ToString() + "x")
        {
            _multiplierText.text = Multiplier.ToString() + "x";
        }
    }
}
