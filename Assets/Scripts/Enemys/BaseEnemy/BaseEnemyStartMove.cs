//Starts the enemy movement if the player is close enough

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
