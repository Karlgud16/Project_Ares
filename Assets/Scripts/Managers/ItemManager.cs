//Handles all of the changes an item will make

using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [ReadOnly] public bool HermesSwitchSandals;
    private bool _hermesSwitchSandals;
    [ReadOnly] public bool ValkyriesWingedBootsBool;
    private bool _valkyriesWingedBoots;
    [ReadOnly] public bool AnubisAnkhBool;
    [ReadOnly] public bool CursedSpursBool;
    [ReadOnly] public bool SlimeArmourBool;

    [Header("Items")]
    public List<GameObject> ItemList;
    public List<ItemScriptableObject> ItemInventory;
    public List<GameObject> ItemHUDInventory;
    public GameObject ItemUIPrefab;
    public GameObject HealthPickupObject;
    public ItemScriptableObject HermesSwiftSandals;
    public ItemScriptableObject ValkyriesWingedBoots;
    public ItemScriptableObject AnubisAnkh;
    public ItemScriptableObject CursedSpurs;
    public ItemScriptableObject SlimeArmour;

    [Header("ItemsStats")]
    public GameObject ItemSpawner;
    public float HealthPickup;
    public float HealthPickupMultiplier;
    [Range(1, 2)] public float HermesSwiftSandalsMultiplier;
    [Range(1, 2)] [SerializeField] private float _hermesSameItemMultiplier;
    [Range(1, 2)] public float ValkyriesWingedBootsMultiplier;
    [Range(1, 2)] [SerializeField]private float _ValkyriesSameItemMultiplier;
    [Range(1, 2)] public float CursedSpursMultiplier;
    [Range(1, 2)] [SerializeField]private float _cursedSpursSameItemMultiplier;
    public float AnubisAnkhHealth;
    [Range(0.01f, 0.99f)] public float SlimeArmourMultiplier;
    [Range(0, 1)][SerializeField] private float _SlimeArmourSameItemMultiplier;
    public float SlimeArmourSecondsUntilNormalSpeed;

    private PlayerManager _playerManager;

    void Start()
    {
        HermesSwitchSandals = false;
        ValkyriesWingedBootsBool = false;
        AnubisAnkhBool = false;
        CursedSpursBool = false;
        SlimeArmourBool = false;

        _hermesSwitchSandals = true;
        _valkyriesWingedBoots = true;

        _playerManager = GameManager.Instance.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        HermesSwitchSandalsCheck();
        ValkyriesWingedBootsCheck();
        CursedSpursCheck();
    }

    void HermesSwitchSandalsCheck()
    {
        if (HermesSwitchSandals && _hermesSwitchSandals)
        {
            _playerManager.PlayerMoveSpeed = _playerManager.PlayerMoveSpeed * HermesSwiftSandalsMultiplier;
            _playerManager.DefaultPlayerMoveSpeed = _playerManager.PlayerMoveSpeed * HermesSwiftSandalsMultiplier;
            _hermesSwitchSandals = false;
        }
    }

    void ValkyriesWingedBootsCheck()
    {
        if (ValkyriesWingedBoots && _valkyriesWingedBoots)
        {
            _playerManager.PlayerJump = _playerManager.PlayerJump + ValkyriesWingedBootsMultiplier;
            _valkyriesWingedBoots = false;
        }
    }

    void CursedSpursCheck()
    {
        if (CursedSpursBool)
        {
            if (_playerManager.CurrentPlayerHealth > _playerManager.PlayerHealth * 0.75)
            {
                _playerManager.PlayerMoveSpeed = _playerManager.DefaultPlayerMoveSpeed * 1f * CursedSpursMultiplier;
            }
            else if (_playerManager.CurrentPlayerHealth < _playerManager.PlayerHealth * 0.75 && _playerManager.CurrentPlayerHealth > _playerManager.PlayerHealth * 0.5)
            {
                _playerManager.PlayerMoveSpeed = _playerManager.DefaultPlayerMoveSpeed * 1.1f * CursedSpursMultiplier;
            }
            else if (_playerManager.CurrentPlayerHealth < _playerManager.PlayerHealth * 0.5 && _playerManager.CurrentPlayerHealth > _playerManager.PlayerHealth * 0.25)
            {
                _playerManager.PlayerMoveSpeed = _playerManager.DefaultPlayerMoveSpeed * 1.2f * CursedSpursMultiplier;
            }
            else if (_playerManager.CurrentPlayerHealth < _playerManager.PlayerHealth * 0.25)
            {
                _playerManager.PlayerMoveSpeed = _playerManager.DefaultPlayerMoveSpeed * 1.3f * CursedSpursMultiplier;
            }
        }
    }

    public void SameItemCheck(string itemName)
    {
        switch (itemName)
        {
            case "Hermes' Swift Sandals":
                HermesSwiftSandalsMultiplier = HermesSwiftSandalsMultiplier * _hermesSameItemMultiplier;
                _hermesSwitchSandals = true;
                break;
            case "Valkyrie's Winged Boots":
                ValkyriesWingedBootsMultiplier = ValkyriesWingedBootsMultiplier * _ValkyriesSameItemMultiplier;
                _valkyriesWingedBoots = true;
                break;
            case "Anubis Ankh":
                break;
            case "Cursed Spurs":
                CursedSpursMultiplier = CursedSpursMultiplier * _cursedSpursSameItemMultiplier;
                break;
            case "Slime Armour":
                SlimeArmourMultiplier = SlimeArmourMultiplier * _SlimeArmourSameItemMultiplier;
                break;
            default:
                break;
        }
    }
}
