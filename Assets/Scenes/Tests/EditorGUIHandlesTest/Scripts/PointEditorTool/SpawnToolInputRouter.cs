using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Overlays;
using UnityEngine;

public sealed class SpawnToolInputRouter
{
    public List<IInputHandler> inputHandlers = new List<IInputHandler>();

    public void RegisterInput(IInputHandler handler)
    {
        inputHandlers.Add(handler);
    }

    public void DeregisterInputs()
    {
        inputHandlers.Clear();
    }

    public bool RouteInput(Event e)
    {
        foreach (IInputHandler handler in inputHandlers)
        {
            if (handler.CheckInputDesired(e))
            {
                e.Use();
                return true;
            }
        }
        return false;
    }


}

public interface IInputHandler
{
    bool CheckInputDesired(Event evt);
}

