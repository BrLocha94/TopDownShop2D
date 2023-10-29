namespace Project.UI.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Project.Enums;

    public abstract class WindowBase : MonoBehaviour
    {
        public Action onTurnOnFinishEvent;
        public Action onTurnOffFinishEvent;

        protected WindowState currentState = WindowState.OFF;

        public abstract void TurnOn();
        public abstract void TurnOff();

        protected virtual void FinishedTurnOn()
        {
            currentState = WindowState.ON;

            onTurnOnFinishEvent?.Invoke();
        }

        protected virtual void FinishedTurnOff()
        {
            currentState = WindowState.OFF;

            onTurnOffFinishEvent?.Invoke();
        }

        protected bool CanPopOn() => currentState == WindowState.OFF;
        protected bool CanPopOff() => currentState == WindowState.ON;
    }
}