using UnityEngine;

[CreateAssetMenu(fileName = "P_EnemyData", menuName = "Scriptable Objects/P_EnemyData")]
public class P_EnemyData : ScriptableObject
{
    public P_EnemyBehaviour behaviour;

    public int spawnCost;

    [Space(10), Header("Prefab Controls")]
    public string enemyName;
    public Color color;
    [Range(0.1f, 3)] public float sizeMultiplier;

    private GameObject ApplyObjectChanges(GameObject enemyObject)
    {
        enemyObject.transform.localScale *= sizeMultiplier;
        enemyObject.GetComponent<Renderer>().material.color = color;
        enemyObject.transform.name = enemyName;

        return enemyObject;
    }
}
