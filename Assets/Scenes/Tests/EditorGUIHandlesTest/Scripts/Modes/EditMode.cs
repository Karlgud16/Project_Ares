using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


#if UNITY_EDITOR
[EditorTool("", typeof(EnemySpawnPointManager))]
public class ConfigMode : SpawnPointToolMode
{
    protected override string IconName => "Wrench";
    protected override string Tooltip => "Configure Points";

    private PointConfigOverlay overlay;

    private PointHandle selectedHandle;

    public override void OnToolActivated()
    {
        CreateListener<LeftClickDownListener>(SelectPointForEditing);

        overlay = new PointConfigOverlay();

    }

    private void SelectPointForEditing()
    {
        if (selectedHandle != null)
        {
            selectedHandle.isSelectedForEditing = false;
        }

        selectedHandle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (selectedHandle != null)
        {

            selectedHandle.isSelectedForEditing = true;

            currentSceneView.overlayCanvas.Add(overlay);
            overlay.PopulateOverlayContents(selectedHandle.pointObject, MarkValueChanged);
            overlay.displayed = true;
        }
        else
        {
            EditorUtility.SetDirty(manager);
            currentSceneView.overlayCanvas.Remove(overlay);
        }
    }

    private void MarkValueChanged()
    {
        EditorUtility.SetDirty(manager);
    }

    public override void OnToolDeactivated()
    {
        EditorUtility.SetDirty(manager);

        if (selectedHandle != null)
        {
            selectedHandle.isSelectedForEditing = false;
            selectedHandle = null;
            currentSceneView.overlayCanvas.Remove(overlay);
        }
    }

    public override void ToolGUI(EditorWindow window)
    {
        //throw new System.NotImplementedException();
    }
} 
#endif
