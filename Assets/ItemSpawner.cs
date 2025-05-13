using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [ReadOnly] public int ItemCount;

    void Start()
    {
        ItemCount = 2;
    }

    void Update()
    {
        if (ItemCount >= 1)
        {
            Destroy(gameObject);
        }
    }
}
