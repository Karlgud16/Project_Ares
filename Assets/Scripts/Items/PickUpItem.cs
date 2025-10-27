//Handles when the item is picked up

using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] [ReadOnly] ItemScriptableObject scriptItem;

    private ItemUI _itemUI;

    [ReadOnly][SerializeField] private ItemSpawner _spawner;

    private ItemManager _itemManager;

    private void Awake()
    {
        _itemUI = GameObject.FindGameObjectWithTag("HUD").transform.Find("OwnedItems").GetComponent<ItemUI>();

        if (transform.root.name.Contains("ItemSpawner"))
        {
            _spawner = transform.root.GetComponent<ItemSpawner>();
        }
        else
        {
            return;
        }
    }

    private void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();

        if (gameObject.name.Contains("Hermes' Swift Sandals"))
        {
            scriptItem = _itemManager.HermesSwiftSandals;
        }
        else if (gameObject.name.Contains("Valkyrie's Winged Boots"))
        {
            scriptItem = _itemManager.ValkyriesWingedBoots;
        }
        else if (gameObject.name.Contains("Anubis Ankh"))
        {
            scriptItem = _itemManager.AnubisAnkh;
        }
        else if (gameObject.name.Contains("Cursed Spurs"))
        {
            scriptItem = _itemManager.CursedSpurs;
        }
        else if (gameObject.name.Contains("Slime Armour"))
        {
            scriptItem = _itemManager.SlimeArmour;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerMovement>().CanInteract)
        {
            foreach (Transform child in _itemUI.Background)
            {
                if (child.gameObject.name == scriptItem.ItemName)
                {
                    GameObject text = child.GetChild(0).gameObject;
                    text.GetComponent<DisplayMultiplier>().Multiplier++;
                    //_itemManager.SameItemCheck(scriptItem.ItemName);
                    Destroy(gameObject);
                    return;
                }
            }
            Debug.Log(scriptItem.ItemName);
            _itemUI.AddToInventory(scriptItem);
            _itemManager.ItemInventory.Add(scriptItem);
            switch (scriptItem.ItemName) 
            {
                case "Hermes' Swift Sandals":
                    _itemManager.HermesSwitchSandals = true;
                    break;
                case "Valkyrie's Winged Boots":
                    _itemManager.ValkyriesWingedBootsBool = true;
                    break;
                case "Anubis Ankh":
                    _itemManager.AnubisAnkhBool = true;
                    break;
                case "Cursed Spurs":
                    _itemManager.CursedSpursBool = true;
                    break;
                case "Slime Armour":
                    _itemManager.SlimeArmourBool = true;
                    break;
                default:
                    Debug.LogError("Item does not exist in script manager :( ( Make sure the case in PickUpItem is the same as the ItemScriptableObject.ItemName :) )");
                    break;
            }

            if(_spawner != null)
            {
                _spawner.itemPicked = true;
            }

            Destroy(gameObject);
        }
    }
}
