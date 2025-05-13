//Handles when the item is picked up

using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] [ReadOnly] ItemScriptableObject scriptItem;

    private ItemUI _itemUI;

    private ItemSpawner _spawner;

    private void Awake()
    {
        _itemUI = GameObject.FindGameObjectWithTag("HUD").transform.Find("OwnedItems").GetComponent<ItemUI>();

        if (transform.parent.transform.parent.name.Contains("ItemSpawner"))
        {
            _spawner = transform.parent.transform.parent.GetComponent<ItemSpawner>();
        }
    }

    private void Start()
    {
        if (gameObject.name.Contains("Hermes' Swift Sandals"))
        {
            scriptItem = GameManager.Instance.HermesSwiftSandals;
        }
        else if (gameObject.name.Contains("Valkyrie's Winged Boots"))
        {
            scriptItem = GameManager.Instance.ValkyriesWingedBoots;
        }
        else if (gameObject.name.Contains("Anubis Ankh"))
        {
            scriptItem = GameManager.Instance.AnubisAnkh;
        }
        else if (gameObject.name.Contains("Cursed Spurs"))
        {
            scriptItem = GameManager.Instance.CursedSpurs;
        }
        else if (gameObject.name.Contains("Slime Armour"))
        {
            scriptItem = GameManager.Instance.SlimeArmour;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerMovement>().CanInteract)
        {
            Debug.Log(scriptItem.ItemName);
            GameManager.Instance.ItemInventory.Add(scriptItem);
            GameManager.Instance.ItemInventoryGameObject.Add(scriptItem.ItemObject);
            _itemUI.AddToInventory(scriptItem);
            switch (scriptItem.ItemName) 
            {
                case "Hermes' Swift Sandals":
                    GameManager.Instance.GetComponent<ItemManager>().HermesSwitchSandals = true;
                    break;
                case "Valkyrie's Winged Boots":
                    GameManager.Instance.GetComponent<ItemManager>().ValkyriesWingedBoots = true;
                    break;
                case "Anubis Ankh":
                    GameManager.Instance.GetComponent<ItemManager>().AnubisAnkh = true;
                    break;
                case "Cursed Spurs":
                    GameManager.Instance.GetComponent<ItemManager>().CursedSpurs = true;
                    break;
                case "Slime Armour":
                    GameManager.Instance.GetComponent<ItemManager>().SlimeArmour = true;
                    break;
                default:
                    Debug.LogError("Item does not exist in script manager :( ( Make sure the case in PickUpItem is the same as the ItemScriptableObject.ItemName :) )");
                    break;
            }

            if(_spawner != null)
            {
                _spawner.ItemCount--;
            }

            Destroy(gameObject);
        }
    }
}
