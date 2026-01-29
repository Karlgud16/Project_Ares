using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

/// <summary>
/// Oversees spawning across the level using the director.
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance { get; private set; }

    [SerializeField] private SpawnZoneController activeZoneController;

    private ObjectPool<GameObject> pool;

    private EnemySpawnDirector director;

    private List<GameObject> currentEnemies = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        director = new EnemySpawnDirector();

        pool = new ObjectPool<GameObject>(
            createFunc: OnEnemyCreate,
            actionOnRelease: OnEnemyRelease,
            actionOnGet: OnEnemyGet);
    }

    public void TriggerSpawnInCurrentZone()
    {
        if (currentEnemies.Count != 0)
        {
            for (int i = 0; i < currentEnemies.Count; ++i)
            {
                ReleasePooledEnemy(currentEnemies[i]);

            }

            currentEnemies.Clear();
        }

        if (activeZoneController != null)
        {
            GameObject[] spawnWave = CreatePooledObjects(director.GetNextWave());

            for (int i = 0; i < spawnWave.Length; i++)
            {
                currentEnemies.Add(spawnWave[i]);
            }
            Debug.Log(currentEnemies.Count);

            activeZoneController.TriggerSpawn(spawnWave);
        }
    }

    public void ReleasePooledEnemy(GameObject enemy)
    {
        pool.Release(enemy);
    }

    public void SetDirectorCreditBalance(int newCreditBalance)
    {
        director.creditBalance = newCreditBalance;
    }

    public void RegisterZoneEntry(SpawnZoneController zoneController)
    {
        activeZoneController = zoneController;
        Debug.Log("Zone entry", zoneController.gameObject);
    }

    private GameObject[] CreatePooledObjects(P_EnemyData[] dataList)
    {
        GameObject[] result = new GameObject[dataList.Length];

        for (int i = 0; i < dataList.Length; i++)
        {
            pool.Get(out GameObject newEnemy);
            dataList[i].ApplyObjectChanges(newEnemy);

            result[i] = newEnemy;
        }

        return result;
    }

    private GameObject OnEnemyCreate()
    {
        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        return gameObject;
    }

    private void OnEnemyGet(GameObject enemy)
    {
        enemy.SetActive(true);
    }

    private void OnEnemyRelease(GameObject enemy)
    {
        enemy.transform.localScale = Vector3.one;

        enemy.GetComponent<Renderer>().material.color = Color.white;

        enemy.SetActive(false);
    }
}

[CustomEditor(typeof(EnemySpawnManager))]
public class EnemySpawnManagerEditor : Editor
{
    private EnemySpawnDirector director;

    private int creditSetting = 1000;

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            creditSetting = EditorGUILayout.IntField("Director credit balance", creditSetting);

            EditorGUI.BeginChangeCheck();

            GUILayout.Button("Trigger spawn wave");

            if (EditorGUI.EndChangeCheck())
            {
                EnemySpawnManager manager = target as EnemySpawnManager;

                manager.SetDirectorCreditBalance(creditSetting);


                manager.TriggerSpawnInCurrentZone();
            }

        }
    }
}
