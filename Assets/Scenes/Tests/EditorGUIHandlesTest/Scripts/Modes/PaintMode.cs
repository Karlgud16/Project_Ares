using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
[EditorTool("", typeof(EnemySpawnPointManager))]
public class PaintMode : SpawnPointToolMode
{
    protected override string IconName => "Brush";
    protected override string Tooltip => "Paint Points";

    private PointPaintOverlay overlay;

    public override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(PaintAddPoint);
        CreateListener<RightClickDownListener>(PaintRemovePoint);

        overlay = new PointPaintOverlay();
        currentSceneView.overlayCanvas.Add(overlay);
        overlay.displayed = true;

        overlay.clearPointsButton.RegisterCallback<MouseUpEvent>((evt) => {
            Undo.RecordObject(manager, "Deleted all points");
            manager.SpawnPoints.Clear(); 
            Debug.Log("cleared points"); 
            RebuildHandleList();
        });

        overlay.radiusField.RegisterCallback<ChangeEvent<float>>((evt) =>
        {
            overlay.radiusField.value = Mathf.Clamp(overlay.radiusField.value, 0, 10);
            Debug.Log(overlay.radiusField.value);
        });

    }

    public override void OnToolDeactivated()
    {
        currentSceneView.overlayCanvas.Remove(overlay);
    }

    public override void ToolGUI(EditorWindow window)
    {

    }

    private void PaintRemovePoint()
    {
        PointHandle handle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (handle != null)
        {
            Undo.RecordObject(manager, "Deleted point");
            manager.SpawnPoints.Remove(handle.pointObject);

            toolHandles.Remove(handle);
        }
    }

    private void PaintAddPoint()
    {
        if (MouseHitPos == Vector3.zero)
        {
            return;
        }

        SpawnPoint newPoint = new SpawnPoint(MouseHitPos, overlay.radiusField.value);

        Debug.Log("Point created at: " + MouseHitPos + "With radius: " + overlay.radiusField.value);

        Undo.RecordObject(manager, "Created new point");
        PointHandle newHandle = new PointHandle(newPoint);
        manager.SpawnPoints.Add(newPoint);
        toolHandles.Add(newHandle);

        EditorUtility.SetDirty(manager);
    }
}
#endif