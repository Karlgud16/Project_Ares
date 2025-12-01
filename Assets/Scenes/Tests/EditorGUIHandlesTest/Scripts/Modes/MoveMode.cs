using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

#if UNITY_EDITOR
[EditorTool("", typeof(EnemySpawnPointManager))]
public class MoveMode : SpawnPointToolMode
{
    protected override string IconName => "Mover";
    protected override string Tooltip => "Move Points";

    private PointHandle selectedHandle;
    private Vector3 mouseGrabOffset;



    public override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(SelectPointForMoving);
        CreateListener<LeftClickUpListener>(DeselectPointForMoving);
    }

    public override void OnToolDeactivated()
    {
        GUIUtility.hotControl = 0;
        selectedHandle = null;
    }

    protected override void DrawToolHandles()
    {
        if (selectedHandle != null)
        {
            Undo.RecordObject(manager, "Moved point");
            selectedHandle.pointObject.center = MouseHitPos + mouseGrabOffset;
        }
    }

    private void SelectPointForMoving()
    {
        selectedHandle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (selectedHandle != null)
        {
            mouseGrabOffset = selectedHandle.pointObject.center - MouseHitPos;
            GUIUtility.hotControl = selectedHandle.controlID;
        }

    }

    private void DeselectPointForMoving()
    {
        GUIUtility.hotControl = 0;
        selectedHandle = null;
    }

    private struct PointMovementArrow
    {
        public int controlID;
        public Vector3 oldPosition;
        public Vector3 position;

    }
}
#endif