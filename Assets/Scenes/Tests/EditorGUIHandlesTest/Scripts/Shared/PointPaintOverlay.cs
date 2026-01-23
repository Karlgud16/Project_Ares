using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine.UIElements;

public class PointPaintOverlay : Overlay
{
    public Button clearPointsButton {  get; private set; }
    public Toggle hasRadiusToggle { get; private set; }
    public FloatField radiusField { get; private set; }


    private VisualElement overlayElement;

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
