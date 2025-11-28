using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

#if UNITY_EDITOR

public abstract class SpawnPointToolMode : EditorTool, IDrawSelectedHandles
{
    protected EnemySpawnPointManager manager => target as EnemySpawnPointManager;

    protected SpawnToolInputRouter inputRouter;

    protected SceneView currentSceneView => SceneView.lastActiveSceneView;

    protected abstract string IconName { get; }
    protected abstract string Tooltip { get; }

    private string IconFullPath => "Editor/Icons/" + IconName;

    protected List<InputListener> inputHandlers { get; } = new List<InputListener>();

    protected Texture2D Icon;

    private GUIContent toolIcon => new GUIContent(Resources.Load<Texture2D>(IconFullPath), Tooltip);

    public sealed override GUIContent toolbarIcon => toolIcon;

    private Vector3 mouseHitPos;
    protected Vector3 MouseHitPos { get => mouseHitPos; }

    public readonly static List<PointHandle> toolHandles = new List<PointHandle>();

    public void OnEnable()
    {
        inputRouter = new SpawnToolInputRouter();
        
        RebuildHandleList();

        Undo.undoRedoPerformed += RebuildHandleList;

        Selection.activeObject = target;
    }

    public void OnDisable()
    {
        Undo.undoRedoPerformed -= RebuildHandleList;
    }

    public sealed override void OnActivated()
    {
        OnToolActivated();

        currentSceneView.ShowNotification(new GUIContent(GetType().Name.Replace("Mode", " ") + "Mode"), 0.2f);

        foreach (IInputHandler handler in inputHandlers)
        {
            inputRouter.RegisterInput(handler);
        }
    }

    public sealed override void OnWillBeDeactivated()
    {
        OnToolDeactivated();
        inputRouter.DeregisterInputs();
    }

    public virtual void OnToolDeactivated()
    {

    }

    public abstract void OnToolActivated();

    public sealed override void OnToolGUI(EditorWindow window)
    {
        Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out RaycastHit hit);

        mouseHitPos = hit.point;

        inputRouter.RouteInput(Event.current);

        ToolGUI(window);

        window.Repaint();
    }

    public virtual void ToolGUI(EditorWindow window)
    {

    }

    public void OnDrawHandles()
    {
        if (ToolManager.activeToolType != GetType())
        {
            return;
        }

        DrawHandles();

        if (MouseHitPos != Vector3.zero)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(MouseHitPos, new Vector3(0, 1), 0.5f);
            Handles.DrawLine(MouseHitPos, new Vector3(MouseHitPos.x, MouseHitPos.y + 0.5f, MouseHitPos.z));
        }

    }

    protected virtual void DrawHandles()
    {

    }

    private void RebuildHandleList()
    {
        toolHandles.Clear();
        for (int i = 0; i < manager.SpawnPoints.Count; i++)
        {
            toolHandles.Add(new PointHandle(manager.SpawnPoints[i]));
        }
    }

    protected void CreateListener<T>(Action action) where T : InputListener, new()
    {
        InputListener input = new T();
        input.ActionOnInput = action;
        inputHandlers.Add(input);
    }
}

public abstract class InputListener : IInputHandler
{
    protected abstract InputConditions desiredConditions { get; }

    public Action ActionOnInput;

    public bool CheckInputDesired(Event evt)
    {
        if (desiredConditions.IsDesiredInput(evt))
        {
            ActionOnInput();
            return true;
        }
        return false;
    }

    protected struct InputConditions
    {
        public InputConditions(EventType desiredEvent)
        {
            eventType = desiredEvent;
            mouseButton = -1;
        }

        public InputConditions(EventType desiredEvent, int desiredMouseButton)
        {
            eventType = desiredEvent;
            mouseButton = desiredMouseButton;
        }

        public readonly bool IsDesiredInput(Event evt)
        {
            if (mouseButton != -1)
            {
                if (evt.button != mouseButton)
                {
                    return false;
                }
            }

            if (evt.type != eventType)
            {
                return false;
            }

            return true;
        }

        private EventType eventType;
        private int mouseButton;
    }

}

public class LeftClickListener : InputListener
{
    protected override InputConditions desiredConditions => new InputConditions(EventType.MouseDown, 0);
}

public class RightClickListener : InputListener
{
    protected override InputConditions desiredConditions => new InputConditions(EventType.MouseDown, 1);
}

[EditorTool("", typeof(EnemySpawnPointManager))]
public class PaintMode : SpawnPointToolMode
{
    protected override string IconName => "Brush";
    protected override string Tooltip => "Paint Points";

    public override void OnToolActivated()
    {
        CreateListener<LeftClickListener>(PaintAddPoint);
        CreateListener<RightClickListener>(PaintRemovePoint);
    }

    public override void OnToolDeactivated()
    {

    }

    public override void ToolGUI(EditorWindow window)
    {

    }

    private void PaintRemovePoint()
    {
        PointHandle handle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        Undo.RecordObject(manager, "Deleted point");
        manager.SpawnPoints.Remove(handle.pointObject);

        toolHandles.Remove(handle);
    }

    private void PaintAddPoint()
    {
        if (MouseHitPos == Vector3.zero)
        {
            return;
        }
        SpawnPoint newPoint = new SpawnPoint(MouseHitPos);

        Debug.Log("Point created at: " + MouseHitPos);

        Undo.RecordObject(manager, "Created new point");
        PointHandle newHandle = new PointHandle(newPoint);
        manager.SpawnPoints.Add(newPoint);
        toolHandles.Add(newHandle);

        EditorUtility.SetDirty(manager);
    }
}

[EditorTool("", typeof(EnemySpawnPointManager))]
public class MoveMode : SpawnPointToolMode
{
    protected override string IconName => "Mover";
    protected override string Tooltip => "Move Points";

    private PointHandle selectedHandle;

    public override void OnToolActivated()
    {
        CreateListener<LeftClickListener>(SelectPointForMoving);
    }

    protected override void DrawHandles()
    {
        if (selectedHandle != null)
        {
            Debug.Log("Among us");
            Debug.DrawLine(selectedHandle.pointObject.center, selectedHandle.pointObject.center * 3);
        }
    }

    private void SelectPointForMoving()
    {
        selectedHandle = toolHandles.Find(h => h.controlID == HandleUtility.nearestControl);

        if (selectedHandle == null)
        {
            Debug.Log("bye");
        }

    }
}

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