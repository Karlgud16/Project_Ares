using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads and checks user input based on currently stored conditions. I'm not sure 
/// this is a good way really, I think its just complex and I have no idea what
/// I was on at the time.
/// </summary>
#if UNITY_EDITOR
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
#endif
