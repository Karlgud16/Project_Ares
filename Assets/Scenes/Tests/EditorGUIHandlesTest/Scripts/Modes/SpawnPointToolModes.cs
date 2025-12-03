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
    }

    public void OnDisable()
    {
        Undo.undoRedoPerformed -= RebuildHandleList;
    }

    public sealed override void OnActivated()
    {
        EnemySpawnPointManagerEditor.toolActive = true;
        OnToolActivated();

        currentSceneView.ShowNotification(new GUIContent(GetType().Name.Replace("Mode", " ") + "Mode"), 0.2f);

        foreach (IInputHandler handler in inputHandlers)
        {
            inputRouter.RegisterInput(handler);
        }
    }

    public sealed override void OnWillBeDeactivated()
    {
        EnemySpawnPointManagerEditor.toolActive = false;

        OnToolDeactivated();
        inputRouter.DeregisterInputs();
    }

    public virtual void OnToolDeactivated()
    {

    }

    public abstract void OnToolActivated();

    public sealed override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView sceneView))
        {
            return;
        }

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

        DrawToolHandles();
    }

    protected virtual void DrawToolHandles()
    {

    }

    protected void RebuildHandleList()
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
#endif