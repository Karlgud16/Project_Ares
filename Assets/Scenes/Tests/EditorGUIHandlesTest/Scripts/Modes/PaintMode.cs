using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;

#if UNITY_EDITOR
[EditorTool("", typeof(EnemySpawnPointManager))]
public class PaintMode : SpawnPointToolMode
{
    protected override string IconName => "Brush";
    protected override string Tooltip => "Paint Points";

    public override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(PaintAddPoint);
        CreateListener<RightClickDownListener>(PaintRemovePoint);

    }

    public override void OnToolDeactivated()
    {

    }

    public override void ToolGUI(EditorWindow window)
    {

    }

    private void PaintRemovePoint()
    {
        PointHandle handle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        Undo.RecordObject(manager, "Deleted point");
        manager.SpawnPoints.Remove(handle.pointObject);

        toolHandles.Remove(handle);
    }

    private void PaintAddPoint()
    {
        if (MouseHitPos == Vector3.zero)
        {
            return;
        }
        SpawnPoint newPoint = new SpawnPoint(MouseHitPos);

        Debug.Log("Point created at: " + MouseHitPos);

        Undo.RecordObject(manager, "Created new point");
        PointHandle newHandle = new PointHandle(newPoint);
        manager.SpawnPoints.Add(newPoint);
        toolHandles.Add(newHandle);

        EditorUtility.SetDirty(manager);
    }
}
#endif