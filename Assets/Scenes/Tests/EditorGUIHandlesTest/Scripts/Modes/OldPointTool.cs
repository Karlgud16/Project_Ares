using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEngine;

//[EditorTool("Spawn Points Editor", typeof(EnemySpawnPointManager))]
public class OldSpawnTool : EditorTool, IDrawSelectedHandles, ISupportsOverlays
{
    EnemySpawnPointManager spawnManager;

    SceneView currentSceneView = SceneView.lastActiveSceneView;

    bool editModeActive;
    bool paintModeActive;
    bool isEnteringPlacementHover = true;

    private List<PointHandle> toolHandles = new List<PointHandle>();

    public enum ToolModes
    {
        edit,
        paint,
    }

    public ToolModes toolMode;

    private Texture2D pointCursorTex;

    PointConfigOverlay toolOverlay;

    private void OnEnable()
    {
        spawnManager = (EnemySpawnPointManager)target;

        spawnManager.SpawnPoints.Clear();

        if (spawnManager.SpawnPoints.Count <= 0)
        {
            toolMode = ToolModes.paint;
        }
        else
        {
            toolMode = ToolModes.edit;
        }

        pointCursorTex = Resources.Load<Texture2D>("ToolPointer");
        Cursor.SetCursor(pointCursorTex, new Vector2(9.3f, 11.6f), CursorMode.Auto);

        RebuildHandleList();

        Undo.undoRedoPerformed += RebuildHandleList;

        ToolManager.activeToolChanged += ToggleOverlay;
    }

    private void ToggleOverlay()
    {
        if (ToolManager.IsActiveTool(this))
        {
            SceneView.lastActiveSceneView.overlayCanvas.Add(toolOverlay);
        }
        else
        {
            SceneView.lastActiveSceneView.overlayCanvas.Remove(toolOverlay);
        }
    }

    private void OnDisable()
    {
        spawnManager = null;
        toolOverlay = null;
        Cursor.SetCursor(null, new Vector2(9.3f, 11.6f), CursorMode.Auto);
    }   

    public override void OnToolGUI(EditorWindow window)
    {
        if (window is not SceneView)
        {
            return;
        }

        currentSceneView = (SceneView)window;

        int winW = 300;
        int winH = 50;

        float screenX = (window.position.width / 2) - (winW / 3f);
        int screenY = 20;

        Rect windowSize = new Rect(screenX, screenY, winW, winH);

        editModeActive = toolMode == ToolModes.edit;
        paintModeActive = toolMode == ToolModes.paint;

        bool editModeTrigger = false;
        bool paintModeTrigger = false;

        GUILayout.BeginArea(windowSize);

        EditorGUI.BeginChangeCheck();

        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            if (GUILayout.Toggle(editModeActive, "Edit", EditorStyles.miniButtonLeft, GUILayout.Width(winW / 2.5f)))
            {
                if (!editModeActive)
                {
                    editModeTrigger = true;
                }
            }

            if (GUILayout.Toggle(paintModeActive, "Place", EditorStyles.miniButtonRight, GUILayout.Width(winW / 2.5f)))
            {
                if (!paintModeActive)
                {
                    paintModeTrigger = true;
                }
            }
        }

        GUILayout.EndArea();

        if (EditorGUI.EndChangeCheck())
        {
            if (editModeTrigger)
            {
                toolMode = ToolModes.edit;
                Debug.Log("Edit Mode");
            }

            if (paintModeTrigger)
            {
                toolMode = ToolModes.paint;
                Debug.Log("Paint Mode");
            }

        }

        Rect sceneViewRect = new Rect(0, 0, window.position.width, window.position.height);

        // Cursor change.
        if (Event.current.type == EventType.Repaint)
        {
            if (!isEnteringPlacementHover && toolMode == ToolModes.paint)
            {
                EditorGUIUtility.AddCursorRect(sceneViewRect, MouseCursor.CustomCursor);
            }
        }
    }



    public void OnDrawHandles()
    {
        if (!ToolManager.IsActiveTool(this))
        {
            return;
        }

        Event currentEvent = Event.current;

        Vector2 screenMousePos = currentEvent.mousePosition;
        screenMousePos.y = currentSceneView.camera.pixelHeight - screenMousePos.y;

        Ray screenToWorldRay = currentSceneView.camera.ScreenPointToRay(screenMousePos);

        bool isHoveringPlacement = Physics.Raycast(screenToWorldRay, out RaycastHit mouseHit);

        int HotControl = GUIUtility.hotControl;

        if (toolMode == ToolModes.edit)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.modifiers == EventModifiers.None)
            {
                if (currentEvent.button == 0)
                {
                    EditSelectPoint(toolHandles.Find(h => h.controlID == HandleUtility.nearestControl));
                }
            }
        }
        else if (toolMode == ToolModes.paint)
        {
            // Draw placement brush handle.
            if (isHoveringPlacement)
            {
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(mouseHit.point, new Vector3(0, 1), 0.5f);
                Handles.DrawLine(mouseHit.point, new Vector3(mouseHit.point.x, mouseHit.point.y + 0.5f, mouseHit.point.z));
            }

            // Repaint on mouse move and entry/exit logic.
            if (currentEvent.type == EventType.MouseMove)
            {
                if (isHoveringPlacement)
                {
                    if (isEnteringPlacementHover)
                    {
                        isEnteringPlacementHover = false;
                        Debug.Log("Enter");
                    }
                }
                else
                {
                    if (!isEnteringPlacementHover)
                    {
                        Debug.Log("Exit");
                    }
                    isEnteringPlacementHover = true;
                }

                currentSceneView.Repaint();
            }

            if(currentEvent.isMouse && currentEvent.modifiers == EventModifiers.None)
            {
                ProcessMouseEvent(currentEvent, toolMode);
            }
            // Point control mouse events.
            if (currentEvent.type == EventType.MouseDown && currentEvent.modifiers == EventModifiers.None)
            {
                if (currentEvent.button == 0)
                {
                    if (isHoveringPlacement)
                    {
                        SpawnPoint newPoint = new SpawnPoint(mouseHit.point);

                        Debug.Log(newPoint.center - mouseHit.point);

                        PaintAddPoint(new PointHandle(newPoint));

                        currentEvent.Use();

                        EditorUtility.SetDirty(spawnManager);
                    }
                }
                else if (currentEvent.button == 1)
                {
                    GUIUtility.hotControl = HandleUtility.nearestControl;
                }
                currentEvent.Use();
            }
            else if (currentEvent.type == EventType.MouseUp && currentEvent.modifiers == EventModifiers.None)
            {
                if (currentEvent.button == 1)
                {
                    PaintRemovePoint(toolHandles.Find(h => h.controlID == HandleUtility.nearestControl));
                    GUIUtility.hotControl = 0;
                }
            }
        }

        int[] ids = new int[spawnManager.SpawnPoints.Count];

        for (int i = 0; i < spawnManager.SpawnPoints.Count; i++)
        {
            PointHandle handle = toolHandles[i];

            handle.controlID = GUIUtility.GetControlID(FocusType.Passive);

            handle.DrawHandle();

            ids[i] = handle.controlID;
        }

        if (ids.Length != 0)
        {
            string sToPrint = null;

            for (int y = 0; y < ids.Length; y++)
            {
                int id = ids[y];

                if (y != 0)
                {
                    sToPrint += ", ";
                }
                sToPrint += id == HandleUtility.nearestControl ? $"({id})" : id.ToString();
            }
        }

    }

    private void PaintRemovePoint(PointHandle handle)
    {
        Undo.RecordObject(spawnManager, "Deleted point");
        spawnManager.SpawnPoints.Remove(handle.pointObject);
     
        toolHandles.Remove(handle);
    }

    private void PaintAddPoint(PointHandle handle)
    {
        Undo.RecordObject(spawnManager, "Created new point");
        spawnManager.SpawnPoints.Add(handle.pointObject);
        toolHandles.Add(handle);
    }

    private void RebuildHandleList()
    {
        toolHandles.Clear();
        for (int i = 0; i < spawnManager.SpawnPoints.Count; i++)
        {
            toolHandles.Add(new PointHandle(spawnManager.SpawnPoints[i]));
        }
    }

    private void EditSelectPoint(PointHandle handle)
    {
        Debug.Log("Popup Edit Menu");
        
    }

    private void DrawToolWindow()
    {

    }

    // Later...
    private void ProcessMouseEvent(Event mouseEvent, ToolModes mode)
    {
        if (mouseEvent.type == EventType.MouseDown)
        {
            if (mouseEvent.button == 0)
            {
                switch (mode)
                {
                    case ToolModes.edit:
                        break;
                    case ToolModes.paint:
                        break;
                }
            }
            else if (mouseEvent.button == 1)
            {

            }
        }
        else if (mouseEvent.type == EventType.MouseUp)
        {
            if (mouseEvent.button == 0)
            {

            }
            else if (mouseEvent.button == 1)
            {

            }
        }
        else if (mouseEvent.type == EventType.MouseDrag)
        {

        }
    }
}