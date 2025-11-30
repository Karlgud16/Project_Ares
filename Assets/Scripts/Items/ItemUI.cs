//Handles adding the right item ui to the screen

using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Transform Background;

    private ItemManager _itemManager;

    void Awake()
    {
        Background = transform.GetChild(0);
    }

    void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();
    }

    public void AddToInventory(Item item)
    {
        GameObject _itemUI = Instantiate(_itemManager.ItemUIPrefab, Background);
        _itemUI.GetComponent<Image>().sprite = item.ItemSprite();
        _itemUI.GetComponent<Image>().color = Color.white;
        _itemUI.name = item.GetName();

        if(_itemUI.transform.childCount > 0)
        {
            foreach (Transform child in _itemUI.transform)
            {
                if(child.gameObject.name.Contains("Multiplier"))
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
}
