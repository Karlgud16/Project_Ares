using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{

    void Start()
    {
        if (transform.childCount == 0)
        {
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem(int attempt = 0)
    {
        if (GameManager.Instance.ItemList.Count == 0)
        {
            Debug.LogWarning("There are no items to spawn in.");
        }

        List<GameObject> validItems = new List<GameObject>();

        foreach(GameObject item in GameManager.Instance.ItemList)
        {
            if(!GameManager.Instance.ItemInventoryGameObject.Any(x => x.name == item.name))
            {
                validItems.Add(item);
            }
        }

        if(validItems.Count == 0)
        {
            Debug.LogWarning("All avaliable items have been spawned in");
        }

        int randomItem = Random.Range(0, validItems.Count);
        GameObject selectedItem = GameManager.Instance.ItemList[randomItem];

        Instantiate(selectedItem, transform);
    }
}
