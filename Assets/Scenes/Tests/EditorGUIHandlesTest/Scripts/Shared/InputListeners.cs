using UnityEngine;
using System;

#if UNITY_EDITOR
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
            shiftDesired = false;
        }

        public InputConditions(EventType desiredEvent, int desiredMouseButton)
        {
            eventType = desiredEvent;
            mouseButton = desiredMouseButton;
            shiftDesired = false;
        }

        public InputConditions(EventType desiredEvent, bool shiftKeyDesired)
        {
            eventType = desiredEvent;
            mouseButton = -1;
            shiftDesired = shiftKeyDesired;
        }

        public InputConditions(EventType desiredEvent, int desiredMouseButton, bool shiftKeyDesired)
        {
            eventType = desiredEvent;
            mouseButton = desiredMouseButton;
            shiftDesired = shiftKeyDesired;
        }


        public readonly bool IsDesiredInput(Event evt)
        {
            if (shiftDesired)
            {
                if (evt.modifiers != EventModifiers.Shift)
                {
                    return false;
                }
            }
            else if (evt.modifiers != EventModifiers.None)
            {
                return false;
            }

            if (evt.type != eventType)
            {
                return false;
            }

            if (mouseButton != -1)
            {
                if (evt.button != mouseButton)
                {
                    return false;
                }
            }


            return true;
        }

        private EventType eventType;
        private int mouseButton;
        private bool shiftDesired;
    }

}

public class LeftClickDownListener : InputListener
{
    protected override InputConditions desiredConditions => new InputConditions(EventType.MouseDown, 0);
}

public class RightClickDownListener : InputListener
{
    protected override InputConditions desiredConditions => new InputConditions(EventType.MouseDown, 1);
}

public class LeftClickUpListener : InputListener
{
    protected override InputConditions desiredConditions => new InputConditions (EventType.MouseUp, 0);
}
#endif