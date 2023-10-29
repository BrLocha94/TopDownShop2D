namespace Project.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using Project.Enums;

    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(EventTrigger))]
    public abstract class SelectableUIBase : MonoBehaviour
    {
        protected CursorState currentState = CursorState.NULL;

        public void OnPointerEnter()
        {
            currentState = CursorState.OVER;
            ExecuteOnPointerEnter();
        }

        public void OnPointerExit()
        {
            currentState = CursorState.OUT;
            ExecuteOnPointerExit();
        }

        public void OnPointerDown()
        {
            if (currentState != CursorState.OVER) return;

            currentState = CursorState.PRESSED;
            ExecuteOnPointerDown();
        }

        public void OnPointerUp()
        {
            if (currentState == CursorState.PRESSED)
            {
                currentState = CursorState.OVER;
                ExecuteOnPointerUp();
            }
        }

        protected virtual void ExecuteOnPointerEnter() { }
        protected virtual void ExecuteOnPointerExit() { }
        protected virtual void ExecuteOnPointerDown() { }
        protected virtual void ExecuteOnPointerUp() { }
    }
}
