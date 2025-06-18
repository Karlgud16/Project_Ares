//Handles the stats for the mini bosses

using UnityEngine;

public class MiniBossManager : MonoBehaviour
{
    [Header("Borrek")]
    public float BorrekHealth;
    public float BorrekMoveSpeed;
    [ReadOnly] public float DefaultBorrekMoveSpeed;
    public float BorrekAttack;
    public float BorrekSecondsUntilDelete;
}
