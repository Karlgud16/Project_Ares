using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PointConfigOverlay : Overlay
{
    VisualElement overlayElement;

    public override VisualElement CreatePanelContent()
    {
        displayName = "Configure Point Data";

        VisualTreeAsset tree = (VisualTreeAsset)AssetDatabase.LoadAssetAtPath("Assets/Scenes/Tests/EditorGUIHandlesTest/Resources/Editor/PointsEditModeOverlay.uxml", typeof(VisualTreeAsset));
        overlayElement = tree.CloneTree();

        return overlayElement;
    }

    public override void OnCreated()
    {
        displayName = "Point Config";
    }
}
