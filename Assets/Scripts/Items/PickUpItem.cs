//Handles when the item is picked up
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    private ItemUI _itemUI;

    [ReadOnly][SerializeField] private ItemSpawner _spawner;

    private ItemManager _itemManager;

    private Item _item;
    [SerializeField] private ItemsNameList _itemDrop;

    private void Awake()
    {
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

        _item = AssignItem(_itemDrop);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && other.GetComponent<PlayerMovement>().CanInteract)
        {
            if(_spawner != null)
            {
                _spawner.itemPicked = true;
            }
            
            Debug.Log(_item.GetName());
            AddItem(_itemManager);
            Destroy(gameObject);
        }
    }

    private void AddItem(ItemManager item)
    {
        foreach (ItemList i in item.Items)
        {
            if(i.name == _item.GetName())
            {
                i.stacks++;

                if(_item is IInstantiableItem instantiable)
                {
                    instantiable.OnInstantiated(_itemManager, i.stacks);
                }

                UpdateTextMultiplier(i.stacks);
                return;
            }
        }
        if (_item is IInstantiableItem instantiableNew)
        {
            instantiableNew.OnInstantiated(_itemManager, 1);
        }
        item.Items.Add(new ItemList(_item, _item.GetName(), 1));
        AddToUIInventory();
    }

    private Item AssignItem(ItemsNameList itemToAssign)
    {
        switch (itemToAssign) 
        {
            case ItemsNameList.HermesSwiftSandals:
                return new HermesSwiftSandals();
            case ItemsNameList.ValkyriesWingedBoots:
                return new ValkyriesWingedBoots();
            case ItemsNameList.CursedSpurs:
                return new CursedSpurs();
            case ItemsNameList.SlimeArmour:
                return new SlimeArmour();
            case ItemsNameList.AnubisAnkh:
                return new AnubisAnkh();
            default:
                return new HermesSwiftSandals();
        }
    }

    public void AddToUIInventory()
    {
        GameObject _itemUI = Instantiate(_itemManager.ItemUIPrefab, _itemManager.ItemUIBackground);
        _itemUI.GetComponent<Image>().sprite = _item.ItemSprite();
        _itemUI.GetComponent<Image>().color = Color.white;
        _itemUI.name = _item.GetName();

        if (_itemUI.transform.childCount > 0)
        {
            foreach (Transform child in _itemUI.transform)
            {
                if (child.gameObject.name.Contains("Multiplier"))
                {
                    return;
                }
                else
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void UpdateTextMultiplier(int stacks)
    {
        foreach(Transform child in _itemManager.ItemUIBackground)
        {
            if(child.name == _item.GetName())
            {
                child.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + stacks;
            }
        }
    }
}

public enum ItemsNameList
{
    HermesSwiftSandals,
    ValkyriesWingedBoots,
    CursedSpurs,
    SlimeArmour,
    AnubisAnkh
}
