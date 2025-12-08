using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawnPointManager : MonoBehaviour
{
#if UNITY_EDITOR
    public List<SpawnPoint> SpawnPoints => _spawnPoints;

    // Total list of all points modified by tools.
    [SerializeField, HideInInspector]
    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

    private int _spawnPointsCount = 0;
#endif



    private SpawnPoint[][] savedSpawnPoints = new SpawnPoint[0][];

    private SpawnPoint[] this[int groupID] => groupID < savedSpawnPoints.Length ? savedSpawnPoints[groupID] : null;
    private int groupCount => savedSpawnPoints.Length;

    public void ValidatePointList()
    {
        if (_spawnPoints.Count == 0) return;

        int[] sizes = new int[_spawnPoints.Count];

        for (int i = 0; i < _spawnPoints.Count; i++)
        {

        }

    }

    private void Awake()
    {
        if (_spawnPoints.Count > 0)
        {
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        for (int i = 0; i < manager.SpawnPoints.Count; i++)
        {
            PointHandle handle = SpawnPointToolMode.toolHandles[i];

            handle.controlID = GUIUtility.GetControlID(FocusType.Passive);

            handle.DrawHandle();
        }

    }

    private void OnValidate()
    {
        
    }
}

[Serializable]
public class SpawnPoint
{
    public Vector3 center;
    public bool hasRadius;
    public float areaRadius;

    public int pointGroup = 0;

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


