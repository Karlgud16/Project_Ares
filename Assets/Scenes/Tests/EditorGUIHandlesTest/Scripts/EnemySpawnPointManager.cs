using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawnPointManager : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    public List<SpawnPoint> SpawnPoints => spawnPoints;

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
}

[System.Serializable]
public class SpawnPoint
{
    public Vector3 center;
    public bool hasRadius;
    public float radius;


    public SpawnPoint(Vector3 position)
    {
        center = position;
        radius = 1;
    }
}


