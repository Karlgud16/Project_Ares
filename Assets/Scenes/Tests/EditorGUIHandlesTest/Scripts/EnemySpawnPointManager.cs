using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EnemySpawnPointManager : MonoBehaviour
{
#if UNITY_EDITOR
    public List<SpawnPoint> SpawnPointsList => _spawnPointsList;

    // Total list of all points modified by tools.
    [SerializeField, HideInInspector]
    private List<SpawnPoint> _spawnPointsList = new List<SpawnPoint>();
#endif
    [SerializeField]
    private SpawnPointGroup[] bakedSpawnPoints = new SpawnPointGroup[0];

    [SerializeField] private GameObject P_Enemy;

    public void BakePoints()
    {
        if (_spawnPointsList.Count == 0)
        {
            bakedSpawnPoints = new SpawnPointGroup[0];
            return;
        }

        Dictionary<int, List<SpawnPoint>> groupedPoints = new Dictionary<int, List<SpawnPoint>>();

        foreach (SpawnPoint point in _spawnPointsList)
        {
            if (!groupedPoints.TryGetValue(point.pointGroup, out var p))
            {
                groupedPoints[point.pointGroup] = p = new List<SpawnPoint>();
            }
            p.Add(point);
        }

        var sortedGroupedPoints = groupedPoints.OrderBy(kv  => kv.Key).ToArray();

        bakedSpawnPoints = new SpawnPointGroup[sortedGroupedPoints.Length];

        for (int i = 0; i < sortedGroupedPoints.Length; i++)
        {
            SpawnPointGroup serializedGroup = new SpawnPointGroup();
            serializedGroup.group = sortedGroupedPoints[i].Value.ToArray();

            bakedSpawnPoints[i] = serializedGroup;
        }

        Debug.Log("Baked points into " + bakedSpawnPoints.Length + " groups.");

        if (true)
        {
            for (int i = 0; i < bakedSpawnPoints.Length; i++)
            {
                Debug.Log($"Group {i} count: {bakedSpawnPoints[i].group.Length}");
            }
        }

        EditorUtility.SetDirty(this);
    }

    [Serializable]
    private class SpawnPointGroup
    {
        public SpawnPoint[] group;

        public SpawnPoint this[int index] => group.Length > 0 ? group[index] : null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(bakedSpawnPoints.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnNextWave(int waveIndex)
    {
        int waveContentCount = 4;
        SpawnPointGroup pointGroup = bakedSpawnPoints[0];

        ObjectToSpawn[] enemiesToSpawn = new ObjectToSpawn[waveContentCount];

        for (int i = 0; i < pointGroup.group.Length; i++)
        {
            enemiesToSpawn[i] = new ObjectToSpawn(P_Enemy);
        }
    }

    private void SetPoissonSpawnPositions(SpawnPoint point, ObjectToSpawn[] objectsToSpawn)
    {
        List<ObjectToSpawn> finalpoissionPositions = new List<ObjectToSpawn>();
        int maxSearchAttempts = 10;

        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            ObjectToSpawn newObject = objectsToSpawn[i];


            for (int j = 0; j < maxSearchAttempts; j++)
            {
                newObject.spawnPosition = SelectNewRandomLocation(point);

                float nearestDistance = Mathf.Infinity;

                foreach (ObjectToSpawn existingObject in finalpoissionPositions)
                {
                    float minTotalDist = existingObject.boundSize + newObject.boundSize;
                    float dist = Vector3.Distance(existingObject.spawnPosition, newObject.spawnPosition);



                    if (dist < nearestDistance)
                    {
                        nearestDistance = dist;
                    }

                    while (checks < maxSearchAttempts)
                    {

                        checks++;
                    }

                }
            }

            newObject.spawnPosition = randomLocation;
            finalpoissionPositions.Add(newObject);
        }
    }

    private Vector3 SelectNewRandomLocation(SpawnPoint point)
    {
        Vector3 offset = UnityEngine.Random.insideUnitCircle * point.areaRadius;
        return point.center + offset;

    }

    private class ObjectToSpawn
    {
        public ObjectToSpawn(GameObject newObject)
        {
            obj = newObject;
            Renderer r = obj.GetComponent<Renderer>();
            boundSize = Mathf.Min(r.bounds.extents.x, r.bounds.extents.y);

            obj.transform.localScale = obj.transform.localScale * UnityEngine.Random.Range(0.5f, 1.5f);
        }

        public GameObject obj;
        public float boundSize;
        public Vector3 spawnPosition;

        private void Spawn()
        {
            Instantiate(obj, spawnPosition, Quaternion.identity);
        }
    }

}

[CustomEditor(typeof(EnemySpawnPointManager))]
public class EnemySpawnPointManagerEditor : Editor
{
    EnemySpawnPointManager manager;

    public static bool toolActive;

    private List<SpawnPoint> editorSpawnPoints = new List<SpawnPoint>();

    private void OnEnable()
    {
        manager = target as EnemySpawnPointManager;
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < manager.SpawnPointsList.Count; i++)
        {
            PointHandle handle = SpawnPointToolMode.toolHandles[i];

            handle.controlID = GUIUtility.GetControlID(FocusType.Passive);

            handle.DrawHandle();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        GUILayout.Button("Force bake points");

        if (EditorGUI.EndChangeCheck())
        {
            manager.BakePoints();
        }
    }
}

[Serializable]
public class SpawnPoint
{
    public Vector3 center;
    public bool hasRadius;
    public float areaRadius;

    public int pointGroup;

    public SpawnPoint(Vector3 position, float radius)
    {
        center = position;

        if (radius > 0)
        {
            areaRadius = radius;
            hasRadius = true;
        }
        else
        {
            areaRadius = 0;
            hasRadius = false;
        }
    }
}


