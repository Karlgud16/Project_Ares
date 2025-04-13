using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyStartMove : MonoBehaviour
{
    [ReadOnly] public bool StartMove;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartMove = true;
        }
    }
}
