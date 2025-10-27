using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 3)]
public class EnemyScriptableObject : ScriptableObject
{
    public float Move;
    public float Health;
    public float Attack;
    public float SecondsUntilDelete;
}
