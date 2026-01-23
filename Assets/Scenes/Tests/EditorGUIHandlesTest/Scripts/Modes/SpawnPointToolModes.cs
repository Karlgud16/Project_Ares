using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// Base class used to make the different spawn point tool modes.
/// </summary>
#if UNITY_EDITOR
public abstract class SpawnPointToolModeBase : EditorTool, IDrawSelectedHandles
{
    // Yikes. I really wish I commented earlier why I had to do this because it was something stupid,
    // but now I'm not really sure how fucked it is :(
    public readonly static List<PointHandle> toolHandles = new List<PointHandle>();

    protected EnemySpawnPointController pointController => target as EnemySpawnPointController;

    protected SpawnToolInputRouter inputRouter;

    protected SceneView currentSceneView => SceneView.lastActiveSceneView;

    protected Vector3 MouseHitPos { get => mouseHitPos; }

    protected abstract string IconName { get; }
    protected abstract string Tooltip { get; }



    private string IconFullPath => "Editor/Icons/" + IconName;

    protected List<InputListener> inputHandlers { get; } = new List<InputListener>();

    protected Texture2D Icon;

    private GUIContent toolIcon => new GUIContent(Resources.Load<Texture2D>(IconFullPath), Tooltip);

    public sealed override GUIContent toolbarIcon => toolIcon;

    private Vector3 mouseHitPos;



    /// <summary>
    /// Called after tool selected for tool mode init.
    /// </summary>
    protected abstract void OnToolActivated();

    /// <summary>
    /// Called after tool deselected or selection changed for tool deinit.
    /// </summary>
    protected virtual void OnToolDeactivated()
    {

    }

    /// <summary>
    /// Called everytime the current scene view is focused and repainted.
    /// </summary>
    /// <param name="window">The current scene view.</param>
    protected virtual void ToolGUI(EditorWindow window)
    {

    }

    /// <summary>
    /// Called everyime the handles are repainted for mode specific visual handles.
    /// </summary>
    protected virtual void DrawToolHandles()
    {

    }

    /// <summary>
    /// Updates the serialized list of points stored in the EnemySpawnPointController.
    /// </summary>
    protected void RebuildHandleList()
    {
        toolHandles.Clear();
        for (int i = 0; i < pointController.SpawnPointsList.Count; i++)
        {
            toolHandles.Add(new PointHandle(pointController.SpawnPointsList[i]));
        }
    }

    /// <summary>
    /// Creates a new callback delegate that listens for the type of desired input. Listeners are automatically deregistered upon tool deselection.
    /// </summary>
    /// <typeparam name="T">Desired input type.</typeparam>
    /// <param name="action">The callback method for when the desired input is seen.</param>
    protected void CreateListener<T>(Action action) where T : InputListener, new()
    {
        InputListener input = new T();
        input.ActionOnInput = action;
        inputHandlers.Add(input);
    }

    public sealed override void OnToolGUI(EditorWindow window)
    {
        if (window is not SceneView)
        {
            return;
        }

        Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore);

        mouseHitPos = hit.point;

        inputRouter.RouteInput(Event.current);

        ToolGUI(window);

        window.Repaint();
    }

    public sealed override void OnActivated()
    {
        Undo.undoRedoPerformed += RebuildHandleList;
        EnemySpawnPointControllerEditor.toolActive = true;
        OnToolActivated();

        currentSceneView.ShowNotification(new GUIContent(GetType().Name.Replace("Mode", " ") + "Mode"), 0.2f);

        foreach (IInputHandler handler in inputHandlers)
        {
            inputRouter.RegisterInput(handler);
        }
    }

    public sealed override void OnWillBeDeactivated()
    {
        Undo.undoRedoPerformed -= RebuildHandleList;
        EnemySpawnPointControllerEditor.toolActive = false;

        OnToolDeactivated();
        inputRouter.DeregisterInputs();

        pointController.BakePoints();
    }

    public void OnEnable()
    {
        inputRouter = new SpawnToolInputRouter();

        RebuildHandleList();
    }

    public void OnDrawHandles()
    {
        if (ToolManager.activeToolType != GetType())
        {
            return;
        }

        DrawToolHandles();
    }
}
#endif