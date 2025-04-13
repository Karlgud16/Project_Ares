//Handles all of the changes an item will make

using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [ReadOnly] public bool HermesSwitchSandals;
    private bool _hermesSwitchSandals;

    void Start()
    {
        HermesSwitchSandals = false;

        _hermesSwitchSandals = true;
    }

    private void Update()
    {
        if (HermesSwitchSandals && _hermesSwitchSandals)
        {
            GameManager.Instance.PlayerMoveSpeed = GameManager.Instance.PlayerMoveSpeed + GameManager.Instance.HermesSwiftSandalsMultiplier;
            _hermesSwitchSandals = false;
        }
    }
}
