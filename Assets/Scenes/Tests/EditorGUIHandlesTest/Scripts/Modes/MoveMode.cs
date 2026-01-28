using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// Allows the user to drag points to move them.
/// </summary>
#if UNITY_EDITOR
[EditorTool("", typeof(SpawnPointPlacementController), toolPriority = -2)]
public class MoveMode : SpawnPointToolModeBase
{
    protected override string IconName => "Mover";
    protected override string Tooltip => "Move Points";

    private PointHandle selectedHandle;
    private Vector3 mouseGrabOffset;



    /// <summary>
    /// Allows the user to drag points to reposition while holding down left-click.
    /// </summary>
    private void SelectPointForMoving()
    {
        selectedHandle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (selectedHandle != null)
        {
            mouseGrabOffset = selectedHandle.pointObject.center - MouseHitPos;
            GUIUtility.hotControl = selectedHandle.controlID;
        }

    }

    /// <summary>
    /// Releases the point the user is holding upon letting left-click go.
    /// </summary>
    private void DeselectPointForMoving()
    {
        GUIUtility.hotControl = 0;
        selectedHandle = null;
    }



    protected sealed override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(SelectPointForMoving);
        CreateListener<LeftClickUpListener>(DeselectPointForMoving);
    }

    protected override void OnToolDeactivated()
    {
        if (selectedHandle != null)
        {
            GUIUtility.hotControl = 0;
            selectedHandle = null;
        }
    }

    protected override void DrawToolHandles()
    {
        if (selectedHandle != null)
        {
            Undo.RecordObject(pointController, "Moved point");
            selectedHandle.pointObject.center = MouseHitPos + mouseGrabOffset;
        }
    }
}
#endif