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

    public override void OnToolActivated()
    {
        overlay = new PointConfigOverlay();
        currentSceneView.overlayCanvas.Add(overlay);
        overlay.displayed = true;
    }

    public override void OnToolDeactivated()
    {
        currentSceneView.overlayCanvas.Remove(overlay);
    }

    public override void ToolGUI(EditorWindow window)
    {
        //throw new System.NotImplementedException();
    }
} 
#endif
