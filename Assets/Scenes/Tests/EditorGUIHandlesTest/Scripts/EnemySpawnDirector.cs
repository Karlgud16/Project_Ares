using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnDirector
{
    [SerializeField] private int creditBalance;

    [SerializeField] private P_EnemyData[] enemyList;

    public GameObject[] GetNextWave()
    {
        if (enemyList.Length == 0)
        {
            return null;
        }

        int minCost = enemyList[0].spawnCost;
        if (creditBalance < minCost)
        {
            return null;
        }

        List<GameObject> newWave = new List<GameObject>();

        while (creditBalance > minCost)
        {


            minCost = enemyList[0].spawnCost;
        }

        return newWave.ToArray();
    }

    public EnemySpawnDirector()
    {
        FetchEnemyDataFiles();
    }

    private EnemyGroupByCost[] FetchEnemyDataFiles()
    {

        // Gather enemy SO location.
        string resourceDir = $"{Directory.GetCurrentDirectory()}\\Assets\\Scenes\\Tests\\EditorGUIHandlesTest\\Resources\\EnemyTest";
        string[] enemyDataClasses = Directory.GetDirectories(resourceDir);

        int classCount = enemyDataClasses.Length;

        for (int i = 0; i < classCount; i++)
        {
            // Gather the enemy SO's.
            string enemyClass = "EnemyTest\\" + enemyDataClasses[i].Split('\\').Last();
            P_EnemyData[] enemyDatas = Resources.LoadAll<P_EnemyData>(enemyClass);

            // Group and sort the enemies based on cost.
            Dictionary<int, List<P_EnemyData>> groupByCost = new Dictionary<int, List<P_EnemyData>>();

            foreach (P_EnemyData data in enemyDatas)
            {
                if (!groupByCost.TryGetValue(data.spawnCost, out var d))
                {
                    groupByCost[data.spawnCost] = d = new List<P_EnemyData>();
                }
                d.Add(data);
            }

            var sortedEnemyCosts = groupByCost.OrderBy(kv  => kv.Value).ToArray();

            EnemyGroupByCost[] result = new EnemyGroupByCost[sortedEnemyCosts.Length];

            for (int j = 0; j < sortedEnemyCosts.Length; j++)
            {
                EnemyGroupByCost costGroup = new EnemyGroupByCost();


            }

            result = null;
        }

        return null;
    }

    private int SelectRandomEntry(int maxWeight)
    {
        int result = 0;
        int total = 0;
        int randomValue = Random.Range(0, maxWeight + 1);

        return result;
    }

    private class EnemyGroupByCost
    {
        public P_EnemyData[] group;

        public P_EnemyData this[int index] => group.Length > 0 ? group[index] : null;
    }
}
