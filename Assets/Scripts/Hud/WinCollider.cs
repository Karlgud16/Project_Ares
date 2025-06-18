//Check to see if the player touches the Win Collider

using UnityEngine;

public class WinCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && GameManager.Instance.GameWon != true)
        {
            GameManager.Instance.GameWon = true;
        }
    }

}
