//Handles when the item is picked up

using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    ItemScriptableObject scriptItem;

    private ItemUI _itemUI;

    private void Awake()
    {
        _itemUI = GameObject.FindGameObjectWithTag("HUD").transform.Find("OwnedItems").GetComponent<ItemUI>();
    }

    private void Start()
    {
        if (gameObject.name.Contains("Hermes' Swift Sandals"))
        {
            scriptItem = GameManager.Instance.HermesSwiftSandals;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.GetComponent<PlayerMovement>().CanInteract)
            {
                Debug.Log(scriptItem.ItemName);
                GameManager.Instance.ItemInventory.Add(scriptItem);
                _itemUI.AddToInventory(scriptItem);

                if(gameObject.name.Contains("Hermes' Swift Sandals"))
                {
                    GameManager.Instance.GetComponent<ItemManager>().HermesSwitchSandals = true;
                }

                Destroy(gameObject);
            }
        }
    }
}
