using KaimiraGames;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnDirector
{
    public int creditBalance;

    private EnemyGroupByCost[] enemyGroups;


    public P_EnemyData[] GetNextWave()
    {
        if (enemyGroups.Length == 0)
        {
            return null;
        }

        int minCost = enemyGroups[0].groupCost;

        if (creditBalance < minCost)
        {
            return null;
        }

        List<WeightedListItem<P_EnemyData>> affordableEnemies = new();

        foreach (EnemyGroupByCost g in enemyGroups)
        {
            if (g.groupCost > creditBalance)
            {
                break;
            }

            foreach (P_EnemyData e in g.group)
            {
                WeightedListItem<P_EnemyData> item = new(e, e.selectionWeighting);

                affordableEnemies.Add(item);
            }
        }

        WeightedList<P_EnemyData> weightedListForSelection = new(affordableEnemies);

        List<P_EnemyData> resultWave = new();

        while (creditBalance > minCost)
        {
            P_EnemyData data = weightedListForSelection.Next();

            if (data.spawnCost < creditBalance)
            {
                resultWave.Add(data);

                creditBalance -= data.spawnCost;

                Debug.Log(creditBalance);
            }
        }

        return resultWave.ToArray();
    }

    public EnemySpawnDirector()
    {
        enemyGroups = FetchEnemyDataFiles();
    }

    private EnemyGroupByCost[] FetchEnemyDataFiles()
    {
        // Gather enemy SO location.
        string resourceDir = $"{Directory.GetCurrentDirectory()}\\Assets\\Scenes\\Tests\\EditorGUIHandlesTest\\Resources\\EnemyTest";
        string[] enemyDataClasses = Directory.GetDirectories(resourceDir);

        int classCount = enemyDataClasses.Length;

        if (classCount == 0)
        {
            return null;
        }

        Dictionary<int, List<P_EnemyData>> groupByCost = new Dictionary<int, List<P_EnemyData>>();


        for (int i = 0; i < classCount; i++)
        {
            // Gather the enemy SO's.
            string enemyClass = "EnemyTest\\" + enemyDataClasses[i].Split('\\').Last();
            P_EnemyData[] enemyDatas = Resources.LoadAll<P_EnemyData>(enemyClass);

            // Group and sort the enemies based on cost.

            foreach (P_EnemyData data in enemyDatas)
            {
                if (!groupByCost.TryGetValue(data.spawnCost, out var d))
                {
                    groupByCost[data.spawnCost] = d = new List<P_EnemyData>();
                }
                d.Add(data);
            }
        }

        var sortedEnemyCosts = groupByCost.OrderBy(kv => kv.Key).ToArray();

        EnemyGroupByCost[] result = new EnemyGroupByCost[sortedEnemyCosts.Length];

        for (int j = 0; j < sortedEnemyCosts.Length; j++)
        {
            EnemyGroupByCost costGroup = new EnemyGroupByCost();

            costGroup.groupCost = sortedEnemyCosts[j].Key;
            costGroup.group = sortedEnemyCosts[j].Value.ToArray();

            result[j] = costGroup;
        }

        return result;
    }

    private P_EnemyData GetRandomEnemy(P_EnemyData[] array)
    {
        P_EnemyData result = null;

        int maxWeight = 0;

        foreach (P_EnemyData data in array)
        {
            maxWeight += data.selectionWeighting;
        }

        int randomValue = new System.Random().Next(0, maxWeight);



        return result;
    }


    private class EnemyGroupByCost
    {
        public int groupCost;
        public P_EnemyData[] group;

        public P_EnemyData this[int index] => group.Length > 0 ? group[index] : null;
    }
}
