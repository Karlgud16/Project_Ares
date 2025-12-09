using System;
using System.Collections.Generic;
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
    private SpawnPoint[][] bakedSpawnPoints = new SpawnPoint[0][];

    private SpawnPoint[] this[int groupID] => groupID < bakedSpawnPoints.Length ? bakedSpawnPoints[groupID] : null;

    public void BakePoints()
    {
        var timingWatch = System.Diagnostics.Stopwatch.StartNew();
        if (_spawnPointsList.Count == 0)
        {
            bakedSpawnPoints = new SpawnPoint[0][];
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

        bakedSpawnPoints = new SpawnPoint[sortedGroupedPoints.Length][];

        for (int i = 0; i < sortedGroupedPoints.Length; i++)
        {
            bakedSpawnPoints[i] = sortedGroupedPoints[i].Value.ToArray();
        }

        Debug.Log("Baked points into " + bakedSpawnPoints.Length + " groups.");

        if (bakedSpawnPoints.Length > 1)
        {
            for (int i = 0; i < bakedSpawnPoints.Length; i++)
            {
                Debug.Log($"Group {i} count: {bakedSpawnPoints[i].Length}");
            }

        }
    }

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < bakedSpawnPoints[0].Length; i++)
        {
            for(int j = 0; j < 10; j++)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

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


