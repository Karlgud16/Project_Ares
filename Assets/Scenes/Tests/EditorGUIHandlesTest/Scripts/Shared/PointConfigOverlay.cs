using UnityEditor.Overlays;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEditor.EditorTools;

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

    public void PopulateOverlayContents(SpawnPoint point, Action valueChangeCallback)
    {
        Debug.Log("apple");

        Toggle hasRadiusToggle = overlayElement.Q<Toggle>("HasRadiusToggle");
        FloatField radiusField = overlayElement.Q<FloatField>("RadiusField");

        hasRadiusToggle.value = point.hasRadius;
        radiusField.value = point.areaRadius;

        hasRadiusToggle.RegisterCallback<MouseUpEvent>(evt =>
        {
            Debug.Log("apple3");

            if (hasRadiusToggle.value)
            {
                point.hasRadius = true;
                radiusField.enabledSelf = true;
            }
            else
            {
                point.hasRadius = false;
                radiusField.enabledSelf = false;
            }

            valueChangeCallback();
        });
    }

    public override void OnCreated()
    {
        displayName = "Point Config";
    }
}
