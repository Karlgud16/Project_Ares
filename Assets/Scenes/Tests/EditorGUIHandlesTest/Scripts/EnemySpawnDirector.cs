using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnDirector
{
    [SerializeField] private int creditBalance;

    [SerializeField] private Dictionary<P_EnemyData, EnemyTypeGroup> typeGroups;

    public GameObject[] GetNextWave()
    {
        GameObject[] newWave;
        return null;
    }

    public EnemySpawnDirector()
    {
        FetchEnemyDataFiles();
    }

    private P_EnemyData[] FetchEnemyDataFiles()
    {
        string resourceDir = $"{Directory.GetCurrentDirectory()}\\Assets\\Scenes\\Tests\\EditorGUIHandlesTest\\Resources\\EnemyTest";

        string[] enemyDataTypes = Directory.GetDirectories(resourceDir);

        int typeCount = enemyDataTypes.Length;

        for (int i = 0; i < typeCount; i++)
        {
            P_EnemyData[] datas = Resources.LoadAll<P_EnemyData>(enemyDataTypes[i]);

            var dataType = datas[0].GetType();

            Debug.Log(dataType);
        }

        return null;
    }

    private class EnemyTypeGroup
    {
        public P_EnemyData[] group;

        public P_EnemyData this[int index] => group.Length > 0 ? group[index] : null;
    }
}
