//If an item is picked in the item spawner, delete the spawner

using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [ReadOnly] public int ItemCount;

    [ReadOnly] public bool itemPicked;

    void Start()
    {
        itemPicked = false;
    }

    void Update()
    {
        if (itemPicked)
        {
            Destroy(gameObject);
        }
    }
}
