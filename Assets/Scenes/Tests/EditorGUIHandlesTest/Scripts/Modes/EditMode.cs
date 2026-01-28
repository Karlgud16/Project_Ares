using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// Allows the user to edit data values on currently existing points.
/// </summary>
#if UNITY_EDITOR
[EditorTool("", typeof(SpawnPointPlacementController), toolPriority = -1)]
public sealed class ConfigMode : SpawnPointToolModeBase
{
    protected override string IconName => "Wrench";
    protected override string Tooltip => "Configure Points";

    private PointConfigOverlay overlay;

    private PointHandle selectedHandle;



    /// <summary>
    /// Selects a point when clicked and populates an overlay window with the point data for editing.
    /// </summary>
    private void SelectPointForEditing()
    {
        if (selectedHandle != null)
        {
            selectedHandle.isSelectedForEditing = false;
            overlay.ClearOverlayContent();
        }

        selectedHandle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (selectedHandle != null)
        {
            selectedHandle.isSelectedForEditing = true;

            currentSceneView.overlayCanvas.Add(overlay);
            overlay.PopulateOverlayContent(selectedHandle.pointObject, pointController);
            overlay.displayed = true;
        }
        else
        {
            currentSceneView.overlayCanvas.Remove(overlay);
        }
    }



    protected override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(SelectPointForEditing);

        overlay = new PointConfigOverlay();
    }

    protected override void OnToolDeactivated()
    {
        EditorUtility.SetDirty(pointController);

        if (selectedHandle != null)
        {
            selectedHandle.isSelectedForEditing = false;
            selectedHandle = null;
        }

        if (overlay != null)
        {
            currentSceneView.overlayCanvas.Remove(overlay);
        }
    }

    protected override void DrawToolHandles()
    {
        if (overlay.ShowMarkers)
        {
            foreach (PointHandle point in toolHandles)
            {
                Handles.BeginGUI();

                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                Handles.Label(point.pointObject.center + Vector3.one, point.pointObject.groupIndex.ToString(), style);
            }
        }
    }

}
#endif
