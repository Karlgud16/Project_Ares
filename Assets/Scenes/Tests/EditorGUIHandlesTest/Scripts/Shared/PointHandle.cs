using UnityEditor;
using UnityEngine;

public class PointHandle
{
    public int controlID;
    public SpawnPoint pointObject;

    public void DrawHandle()
    {
        float controlRadius = pointObject.radius > 0.25f ? pointObject.radius : 0.25f;
        Handles.color = Color.clear;
        Handles.CircleHandleCap(controlID, pointObject.center, Quaternion.LookRotation(Vector3.up), controlRadius, EventType.Layout);

        Handles.color = GUIUtility.hotControl == controlID || HandleUtility.nearestControl == controlID ? new Color(1f, 0.6470588f, 0f, 1f) : Color.green;
        Handles.DrawSolidDisc(pointObject.center, Vector3.up, 0.25f);

        if (!pointObject.hasRadius)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(pointObject.center, Vector3.up, pointObject.radius);
        }
    }

    public PointHandle(SpawnPoint newPointObject)
    {
        pointObject = newPointObject;
    }
}
