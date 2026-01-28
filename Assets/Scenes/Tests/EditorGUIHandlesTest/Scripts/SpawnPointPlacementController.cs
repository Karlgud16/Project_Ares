using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Controls all point placement and serialization.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class SpawnPointPlacementController : MonoBehaviour
{
    #if UNITY_EDITOR
    public List<SpawnPoint> SpawnPointsList => _spawnPointsList;

    // Total list of all points modified by tools.
    [SerializeField, HideInInspector]
    private List<SpawnPoint> _spawnPointsList = new List<SpawnPoint>();
    #endif

    [SerializeField]
    private SpawnPointGroup[] bakedSpawnPoints = new SpawnPointGroup[0];



    /// <summary>
    /// Compiles the total list of points into a serialized jagged array. 
    /// </summary>
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
            if (!groupedPoints.TryGetValue(point.groupIndex, out var p))
            {
                groupedPoints[point.groupIndex] = p = new List<SpawnPoint>();
            }
            p.Add(point);
        }

        var sortedGroupedPoints = groupedPoints.OrderBy(kv => kv.Key).ToArray();

        bakedSpawnPoints = new SpawnPointGroup[sortedGroupedPoints.Length];

        for (int i = 0; i < sortedGroupedPoints.Length; i++)
        {
            SpawnPointGroup serializedGroup = new SpawnPointGroup();
            serializedGroup.group = sortedGroupedPoints[i].Value.ToArray();

            bakedSpawnPoints[i] = serializedGroup;
        }

        EditorUtility.SetDirty(this);

#if UNITY_EDITOR
        Debug.Log("Baked points into " + bakedSpawnPoints.Length + " groups.");

        for (int i = 0; i < bakedSpawnPoints.Length; i++)
        {
            Debug.Log($"Group {i} count: {bakedSpawnPoints[i].group.Length}");
        }
#endif
    }

    /// <summary>
    /// Plain class to replicate a native jagged array, which are not serialized by Unity.
    /// </summary>
    [Serializable]
    private class SpawnPointGroup
    {
        public SpawnPoint[] group;

        public SpawnPoint this[int index] => group.Length > 0 ? group[index] : null;
    }
}



[CustomEditor(typeof(SpawnPointPlacementController))]
public class EnemySpawnPointControllerEditor : Editor
{
    private SpawnPointPlacementController controller;

    // Used to control 'active' vs 'inactive but selected' point handle rendering. Unity's ToolManager doesn't
    // do polymorphism apparently. This whole class needs a new coat of paint at some point I think.
    public static bool toolActive;


    /// <summary>
    /// Controls renderering of existing point handles. This is tied to a custom inspector because it's easier 
    /// and more reliable for me to control it's state by just riding off the inspector.
    /// </summary>
    private void OnSceneGUI()
    {
        for (int i = 0; i < controller.SpawnPointsList.Count; i++)
        {
            // See I think it can just read from the current points and this is old hacky code, but at this point
            // I'm quite tired, and it works. So it can stay for now, sorry.
            PointHandle handle = SpawnPointToolModeBase.toolHandles[i];

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
            controller.BakePoints();
        }
    }

    private void OnEnable()
    {
        controller = target as SpawnPointPlacementController;
    }

}

/// <summary>
/// Container used to hold all serializedruntime point data.
/// </summary>
[Serializable]
public class SpawnPoint
{
    public Vector3 center;
    public bool hasRadius;
    public float areaRadius;

    public int groupIndex;

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


