using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class PointConfigOverlay : Overlay
{
    private VisualElement overlayElement;

    private Toggle hasRadiusToggle;
    private FloatField radiusField;
    private Toggle showMarkersToggle;
    private IntegerField pointGroupField;

    private bool showMarkers;
    public bool ShowMarkers => showMarkers;

    private SpawnPoint _point;
    private SpawnPointPlacementController _manager;

    public override VisualElement CreatePanelContent()
    {
        displayName = "Configure Point Data";

        VisualTreeAsset tree = (VisualTreeAsset)AssetDatabase.LoadAssetAtPath("Assets/Scenes/Tests/EditorGUIHandlesTest/Resources/Editor/PointsEditModeOverlay.uxml", typeof(VisualTreeAsset));

        Debug.Log(tree);
        overlayElement = tree.CloneTree();


        return overlayElement;
    }

    public void PopulateOverlayContent(SpawnPoint point, SpawnPointPlacementController manager)
    {
        hasRadiusToggle = overlayElement.Q<Toggle>("HasRadiusToggle");
        radiusField = overlayElement.Q<FloatField>("RadiusField");
        showMarkersToggle = overlayElement.Q<Toggle>("ShowPointGroupMarkers");
        pointGroupField = overlayElement.Q<IntegerField>("PointGroupField");

        showMarkersToggle.value = showMarkers;
        hasRadiusToggle.value = point.hasRadius;
        radiusField.value = point.areaRadius;
        radiusField.enabledSelf = point.hasRadius;
        pointGroupField.value = point.groupIndex;

        _point = point;
        _manager = manager;

        hasRadiusToggle.RegisterCallback<MouseUpEvent>(evt => ChangeRadiusToggleValue());

        radiusField.RegisterCallback<ChangeEvent<float>>(evt => ChangeRadiusValue());

        showMarkersToggle.RegisterCallback<MouseUpEvent>(evt => ChangeMarkersToggleValue());

        pointGroupField.RegisterCallback<ChangeEvent<int>>(evt => ChangeGroupValue());
    }

    public void ClearOverlayContent()
    {
        hasRadiusToggle.UnregisterCallback<MouseUpEvent>(evt => ChangeRadiusToggleValue());
        radiusField.UnregisterCallback<ChangeEvent<float>>(evt => ChangeRadiusValue());
    }

    public override void OnCreated()
    {
        displayName = "Point Config";
    }


    private void ChangeMarkersToggleValue()
    {
        showMarkers = showMarkersToggle.value;
    }

    private void ChangeRadiusToggleValue()
    {
        Undo.RecordObject(_manager, "Changed point data");

        _point.hasRadius = radiusField.enabledSelf = hasRadiusToggle.value;

        EditorUtility.SetDirty(_manager);
    }

    private void ChangeRadiusValue()
    {
        Undo.RecordObject(_manager, "Changed point data");

        _point.areaRadius = radiusField.value = Mathf.Clamp(radiusField.value, 1, 5);

        EditorUtility.SetDirty(_manager);
    }

    private void ChangeGroupValue()
    {
        Debug.Log(pointGroupField.value);
        Undo.RecordObject(_manager, "Changed point group");

        _point.groupIndex = pointGroupField.value = Mathf.Clamp(pointGroupField.value, 1, 30);

        EditorUtility.SetDirty(_manager);
    }


}
