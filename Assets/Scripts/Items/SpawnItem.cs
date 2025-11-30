//Handles the spawning of items in the item spawner

using UnityEngine;

public class SpawnItem : MonoBehaviour
{

    private ItemManager _itemManager;

    [ReadOnly][SerializeField] private GameObject _selectedItem;

    [ReadOnly][SerializeField] private GameObject _otherSpawn;

    void Start()
    {
        _itemManager = GameManager.Instance.GetComponent<ItemManager>();

        //Gets the opposite spawn game object
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
        int randomItem = Random.Range(0, _itemManager.ItemDrops.Count);
        _selectedItem = _itemManager.ItemDrops[randomItem];

        Instantiate(_selectedItem, transform);
    }
}
