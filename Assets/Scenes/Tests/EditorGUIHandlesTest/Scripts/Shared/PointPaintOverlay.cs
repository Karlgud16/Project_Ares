using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PointPaintOverlay : Overlay
{
    VisualElement overlayElement;
    public Button clearPointsButton;
    public Toggle hasRadiusToggle;
    public FloatField radiusField;

    public override VisualElement CreatePanelContent()
    {
        displayName = "Point Paint Settings";

        VisualTreeAsset tree = (VisualTreeAsset)AssetDatabase.LoadAssetAtPath("Assets/Scenes/Tests/EditorGUIHandlesTest/Resources/Editor/PointsPaintModeOverlay.uxml", typeof(VisualTreeAsset));
        overlayElement = tree.CloneTree();

        clearPointsButton = overlayElement.Q<Button>("ClearPointsButton");
        hasRadiusToggle = overlayElement.Q<Toggle>("HasRadiusToggle");
        radiusField = overlayElement.Q<FloatField>("RadiusField");

        return overlayElement;
    }

    public override void OnCreated()
    {
        displayName = "Point Paint Settings";
    }
}
