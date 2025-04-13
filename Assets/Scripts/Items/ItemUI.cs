//Handles adding the right item ui to the screen

using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private Transform _background;

    void Start()
    {
        _background = transform.GetChild(0);
    }

    public void AddToInventory(ItemScriptableObject item)
    {
        GameObject _itemUI = Instantiate(GameManager.Instance.ItemUIPrefab, _background);
        _itemUI.GetComponent<Image>().sprite = item.ItemSprite;
    }
}
