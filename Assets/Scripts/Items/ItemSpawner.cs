using System.Collections;
using System.Collections.Generic;
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
