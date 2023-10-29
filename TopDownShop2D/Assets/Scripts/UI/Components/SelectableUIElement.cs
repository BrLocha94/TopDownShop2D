namespace Project.UI.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    [RequireComponent(typeof(Image))]
    public class SelectableUIElement : SelectableUIBase
    {
        [Header("External references")]
        [SerializeField]
        protected Image targetImage;

        [Header("Custom oparations to execute on cursor")]
        [SerializeField]
        private Color colorOver = Color.yellow;
        [SerializeField]
        private Color colorOut = Color.white;
        [SerializeField]
        private Color colorPressed = Color.magenta;
        [SerializeField]
        private Color colorSelected = Color.green;

        [Header("Event to trigger when selected")]
        [SerializeField]
        protected UnityEvent onSelectedEvent;

        public bool isSelected { get; private set; } = false;

        protected override void ExecuteOnPointerEnter()
        {
            if (isSelected) return;

            base.ExecuteOnPointerEnter();

            targetImage.color = colorOver;
        }

        protected override void ExecuteOnPointerExit()
        {
            if (isSelected) return;

            base.ExecuteOnPointerExit();

            targetImage.color = colorOut;
        }

        protected override void ExecuteOnPointerDown()
        {
            if (isSelected) return;

            base.ExecuteOnPointerDown();

            targetImage.color = colorPressed;
        }

        protected override void ExecuteOnPointerUp()
        {
            if (isSelected) return;

            base.ExecuteOnPointerUp();

            onSelectedEvent?.Invoke();
        }

        public virtual void SelectElement()
        {
            if (isSelected) return;

            isSelected = true;
            targetImage.color = colorSelected;
        }

        public virtual void UnselectElement()
        {
            if (!isSelected) return;

            isSelected = false;
            targetImage.color = colorOut;
        }
    }
}