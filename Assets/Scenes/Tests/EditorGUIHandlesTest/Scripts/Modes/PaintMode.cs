using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Allows the user to place and remove points in a spawn zone.
/// </summary>
#if UNITY_EDITOR
[EditorTool("", typeof(SpawnPointPlacementController), toolPriority = -3)]
public sealed class PaintMode : SpawnPointToolModeBase
{
    protected override string IconName => "Brush";
    protected override string Tooltip => "Paint Points";

    private PointPaintOverlay overlay;

    private bool newPointHasRadius;

    private Bounds spawnZoneBounds;

    private bool isValidPlaceTarget;



    /// <summary>
    /// Places a new point in the scene where the user left-clicks.
    /// </summary>
    private void PaintAddPoint()
    {
        if (!isValidPlaceTarget)
        {
            return;
        }

        float radius = newPointHasRadius ? overlay.radiusField.value : 0;

        SpawnPoint newPoint = new SpawnPoint(MouseHitPos, radius);

        Debug.Log("Point created at: " + MouseHitPos + " With radius: " + overlay.radiusField.value);

        Undo.RecordObject(pointController, "Created new point");
        PointHandle newHandle = new PointHandle(newPoint);
        pointController.SpawnPointsList.Add(newPoint);
        toolHandles.Add(newHandle);

        EditorUtility.SetDirty(pointController);
    }

    /// <summary>
    /// Removes a hovered point in the scene that the user right-clicks.
    /// </summary>
    private void PaintRemovePoint()
    {
        PointHandle handle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (handle != null)
        {
            Undo.RecordObject(pointController, "Deleted point");
            pointController.SpawnPointsList.Remove(handle.pointObject);

            toolHandles.Remove(handle);
        }
    }


    // Additional controls for creating points in an overlay.

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
        Undo.RecordObject(pointController, "Deleted all points");
        pointController.SpawnPointsList.Clear();
        Debug.Log("Cleared all existing points");
        RebuildHandleList();
    }




    protected override void ToolGUI(EditorWindow window)
    {
        isValidPlaceTarget = spawnZoneBounds.Contains(MouseHitPos);
    }

    protected override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(PaintAddPoint);
        CreateListener<RightClickDownListener>(PaintRemovePoint);

        overlay = new PointPaintOverlay();
        currentSceneView.overlayCanvas.Add(overlay);
        overlay.displayed = true;

        overlay.clearPointsButton.RegisterCallback<MouseUpEvent>(evt => ClearAllExistingPoints());

        overlay.radiusField.RegisterCallback<ChangeEvent<float>>(evt => AdjustNewPointRadius());

        overlay.hasRadiusToggle.RegisterCallback<MouseUpEvent>(evt => ToggleNewPointHasRadius());

        spawnZoneBounds = target.GetComponent<BoxCollider>().bounds;
    }

    protected override void OnToolDeactivated()
    {
        if (overlay != null)
        {
            overlay.radiusField.UnregisterCallback<ChangeEvent<float>>(evt => AdjustNewPointRadius());
            overlay.clearPointsButton.UnregisterCallback<MouseUpEvent>(evt => ClearAllExistingPoints());
            overlay.hasRadiusToggle.UnregisterCallback<MouseUpEvent>(evt => ToggleNewPointHasRadius());

            currentSceneView.overlayCanvas.Remove(overlay);
        }
    }

    protected override void DrawToolHandles()
    {
        if (MouseHitPos != Vector3.zero)
        {
            if (isValidPlaceTarget)
            {
                Handles.color = Color.green;
            }
            else
            {
                Handles.color = Color.red;
            }

            if (newPointHasRadius)
            {
                Handles.DrawWireDisc(MouseHitPos, new Vector3(0, 1), overlay.radiusField.value);
            }

            Handles.DrawLine(MouseHitPos, new Vector3(MouseHitPos.x, MouseHitPos.y + 0.7f, MouseHitPos.z));
        }
    }
}
#endif