using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{

    private ItemManager _itemManager;

    [ReadOnly][SerializeField] private GameObject _selectedItem;

    [ReadOnly][SerializeField] private GameObject _otherSpawn;

    void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();

        if(gameObject.transform.root.GetChild(0).name != gameObject.name)
        {
            _otherSpawn = gameObject.transform.root.GetChild(0).gameObject;
        }
        else if(gameObject.transform.root.GetChild(1).name != gameObject.name)
        {
            _otherSpawn = gameObject.transform.root.GetChild(1).gameObject;
        }

        if (transform.childCount == 0)
        {
            SpawnRandomItem();
        }
    }

    private void Update()
    {
        if (_selectedItem.name == _otherSpawn.transform.GetChild(0).name)
        {
            Destroy(_selectedItem);
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        if (_itemManager.ItemList.Count == 0)
        {
            Debug.LogWarning("There are no items to spawn in.");
            return;
        }

        int randomItem = Random.Range(0, _itemManager.ItemList.Count);
        _selectedItem = _itemManager.ItemList[randomItem];

        Instantiate(_selectedItem, transform);
    }
}
