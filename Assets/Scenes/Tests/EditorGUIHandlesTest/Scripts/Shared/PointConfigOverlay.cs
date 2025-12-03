using System;
using System.Drawing;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PointConfigOverlay : Overlay
{
    private VisualElement overlayElement;

    private Toggle hasRadiusToggle;
    private FloatField radiusField;
    private Toggle showMarkersToggle;

    private bool showMarkers;
    public bool ShowMarkers => showMarkers;

    private SpawnPoint _point;
    private EnemySpawnPointManager _manager;

    public override VisualElement CreatePanelContent()
    {
        displayName = "Configure Point Data";

        VisualTreeAsset tree = (VisualTreeAsset)AssetDatabase.LoadAssetAtPath("Assets/Scenes/Tests/EditorGUIHandlesTest/Resources/Editor/PointsEditModeOverlay.uxml", typeof(VisualTreeAsset));
        overlayElement = tree.CloneTree();


        return overlayElement;
    }

    public void PopulateOverlayContent(SpawnPoint point, EnemySpawnPointManager manager)
    {

        hasRadiusToggle = overlayElement.Q<Toggle>("HasRadiusToggle");
        radiusField = overlayElement.Q<FloatField>("RadiusField");
        showMarkersToggle = overlayElement.Q<Toggle>("ShowPointGroupMarkers");


        showMarkersToggle.value = showMarkers;
        hasRadiusToggle.value = point.hasRadius;
        radiusField.value = point.areaRadius;
        radiusField.enabledSelf = point.hasRadius;

        _point = point;
        _manager = manager;

        hasRadiusToggle.RegisterCallback<MouseUpEvent>(evt => ChangeRadiusToggleValue());

        radiusField.RegisterCallback<ChangeEvent<float>>(evt => ChangeRadiusValue());

        showMarkersToggle.RegisterCallback<MouseUpEvent>(evt => ChangeMarkersToggleValue());
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

    public void ClearOverlayContent()
    {
        hasRadiusToggle.UnregisterCallback<MouseUpEvent>(evt => ChangeRadiusToggleValue());
        radiusField.UnregisterCallback<ChangeEvent<float>>(evt => ChangeRadiusValue());
    }

    public override void OnCreated()
    {
        displayName = "Point Config";
    }
}
