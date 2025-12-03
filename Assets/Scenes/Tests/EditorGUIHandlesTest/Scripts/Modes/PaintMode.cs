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

    private bool newPointHasRadius;

    public override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(PaintAddPoint);
        CreateListener<RightClickDownListener>(PaintRemovePoint);

        overlay = new PointPaintOverlay();
        currentSceneView.overlayCanvas.Add(overlay);
        overlay.displayed = true;

        overlay.clearPointsButton.RegisterCallback<MouseUpEvent>(evt => ClearAllExistingPoints());

        overlay.radiusField.RegisterCallback<ChangeEvent<float>>(evt => AdjustNewPointRadius());

        overlay.hasRadiusToggle.RegisterCallback<MouseUpEvent>(evt => ToggleNewPointHasRadius());

    }

    private void ToggleNewPointHasRadius()
    {
        newPointHasRadius = overlay.radiusField.enabledSelf = overlay.hasRadiusToggle.value;
    }

    private void AdjustNewPointRadius()
    {
        overlay.radiusField.value = Mathf.Clamp(overlay.radiusField.value, 1, 5);
    }

    private void ClearAllExistingPoints()
    {
        Undo.RecordObject(manager, "Deleted all points");
        manager.SpawnPoints.Clear();
        Debug.Log("Cleared all existing points");
        RebuildHandleList();
    }

    protected override void DrawToolHandles()
    {
        if (MouseHitPos != Vector3.zero)
        {
            Handles.color = Color.yellow;

            if (newPointHasRadius)
            {
                Handles.DrawWireDisc(MouseHitPos, new Vector3(0, 1), overlay.radiusField.value);
            }

            Handles.DrawLine(MouseHitPos, new Vector3(MouseHitPos.x, MouseHitPos.y + 0.7f, MouseHitPos.z));
        }
    }

    public override void OnToolDeactivated()
    {
        overlay.radiusField.UnregisterCallback<ChangeEvent<float>>(evt => AdjustNewPointRadius());
        overlay.clearPointsButton.UnregisterCallback<MouseUpEvent>(evt => ClearAllExistingPoints());
        overlay.hasRadiusToggle.UnregisterCallback<MouseUpEvent>(evt => ToggleNewPointHasRadius());

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

        float radius = newPointHasRadius ? overlay.radiusField.value : 0;

        SpawnPoint newPoint = new SpawnPoint(MouseHitPos, radius);

        Debug.Log("Point created at: " + MouseHitPos + " With radius: " + overlay.radiusField.value);

        Undo.RecordObject(manager, "Created new point");
        PointHandle newHandle = new PointHandle(newPoint);
        manager.SpawnPoints.Add(newPoint);
        toolHandles.Add(newHandle);

        EditorUtility.SetDirty(manager);
    }
}
#endif