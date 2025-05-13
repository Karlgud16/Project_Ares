//Handles all of the changes an item will make

using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [ReadOnly] public bool HermesSwitchSandals;
    private bool _hermesSwitchSandals;

    [ReadOnly] public bool ValkyriesWingedBoots;
    private bool _valkyriesWingedBoots;

    [ReadOnly] public bool AnubisAnkh;

    [ReadOnly] public bool CursedSpurs;

    [ReadOnly] public bool SlimeArmour;

    void Start()
    {
        HermesSwitchSandals = false;
        ValkyriesWingedBoots = false;
        AnubisAnkh = false;
        CursedSpurs = false;
        SlimeArmour = false;

        _hermesSwitchSandals = true;
        _valkyriesWingedBoots = true;
    }

    private void Update()
    {
        if (HermesSwitchSandals && _hermesSwitchSandals)
        {
            GameManager.Instance.PlayerMoveSpeed = GameManager.Instance.PlayerMoveSpeed + GameManager.Instance.HermesSwiftSandalsMultiplier;
            GameManager.Instance.DefaultPlayerMoveSpeed = GameManager.Instance.PlayerMoveSpeed + GameManager.Instance.HermesSwiftSandalsMultiplier;
            _hermesSwitchSandals = false;
        }

        if (ValkyriesWingedBoots && _valkyriesWingedBoots)
        {
            GameManager.Instance.PlayerJump = GameManager.Instance.PlayerJump + GameManager.Instance.ValkyriesWingedBootsMultiplier;
            _valkyriesWingedBoots = false;
        }
    }
}
