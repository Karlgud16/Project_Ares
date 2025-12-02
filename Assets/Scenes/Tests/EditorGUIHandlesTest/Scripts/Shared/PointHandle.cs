using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

public class PointHandle
{
    public int controlID;
    public SpawnPoint pointObject;
    public bool isSelectedForEditing;

    public void DrawHandle()
    {
        float controlRadius = pointObject.areaRadius > 0.25f ? pointObject.areaRadius : 0.25f;
        Handles.color = Color.clear;
        Handles.CircleHandleCap(controlID, pointObject.center, Quaternion.LookRotation(Vector3.up), controlRadius, EventType.Layout);

        Handles.color = HandleCenterColor();
        Handles.DrawSolidDisc(pointObject.center, Vector3.up, 0.25f);

        if (pointObject.hasRadius)
        {
            Handles.color = HandleRadiusColor();
            Handles.DrawWireDisc(pointObject.center, Vector3.up, pointObject.areaRadius);
        }
    }

    private Color HandleCenterColor()
    {
        if (!EnemySpawnPointManagerEditor.toolActive)
        {
            return Color.cyan;
        }

        if (GUIUtility.hotControl == controlID || HandleUtility.nearestControl == controlID || isSelectedForEditing)
        {
            // Orange.
            return new Color(0.92f, 0.63f, 0.2f);
        }

        return Color.green;
    }

    private Color HandleRadiusColor()
    {
        if (!EnemySpawnPointManagerEditor.toolActive)
        {
            return Color.cyan;
        }

        return Color.yellow;

    }

    public PointHandle(SpawnPoint newPointObject)
    {
        pointObject = newPointObject;
    }
}
