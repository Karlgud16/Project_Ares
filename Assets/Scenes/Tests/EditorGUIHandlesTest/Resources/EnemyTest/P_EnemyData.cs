using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData")]
public class P_EnemyData : ScriptableObject
{
    public int selectionWeighting = 10;
    public int spawnCost = 10;

    [Space(10), Header("Prefab Controls")]
    public string enemyName;
    public Color color;
    [Range(0.1f, 3)] public float sizeMultiplier;

    public GameObject ApplyObjectChanges(GameObject enemyObject)
    {
        enemyObject.transform.localScale *= sizeMultiplier;
        enemyObject.GetComponent<Renderer>().material.color = color;
        enemyObject.transform.name = enemyName;

        return enemyObject;
    }
}


