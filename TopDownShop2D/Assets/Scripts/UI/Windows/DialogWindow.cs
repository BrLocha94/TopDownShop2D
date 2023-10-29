namespace Project.UI.Windows
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using Project.Utils;
    using Project.Enums;
    using Project.Structures.Iteraction.Dialog;
    using TMPro;

    public class DialogWindow : WindowBase
    {
        public Action onDialogFinishEvent;

        [Header("Dialog window references")]
        [SerializeField]
        private TextMeshProUGUI ownerName;
        [SerializeField]
        private TypeEffect dialogText;

        [Header("Pop animation config")]
        [SerializeField]
        private AnimationCurve turnOnCurve;
        [SerializeField]
        private float turnOnTime;
        [SerializeField]
        private Vector3 onPosition;

        [Space]

        [SerializeField]
        private AnimationCurve turnOffCurve;
        [SerializeField]
        private float turnOffTime;
        [SerializeField]
        private Vector3 offPosition;

        Coroutine currentPopRoutine = null;
        Coroutine dialogRoutine = null;
        Dialog currentDialog = null;

        public override void TurnOn()
        {
            if (currentState != WindowState.OFF) return;

            if (dialogRoutine != null)
                StopCoroutine(dialogRoutine);

            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            transform.localPosition = offPosition;
            gameObject.SetActive(true);

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.MoveRoutine(transform, transform.localPosition, onPosition,
                                                        turnOnCurve, turnOnTime, FinishedTurnOn);
        }

        public override void TurnOff()
        {
            if (currentState != WindowState.ON) return;

            if (dialogRoutine != null)
                StopCoroutine(dialogRoutine);

            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.MoveRoutine(transform, transform.localPosition, offPosition,
                                                        turnOffCurve, turnOffTime, FinishedTurnOff);
        }

        protected override void FinishedTurnOn()
        {
            base.FinishedTurnOn();

            if (currentDialog != null)
            {
                dialogRoutine = StartCoroutine(DialogRoutine(() => onDialogFinishEvent?.Invoke()));
            }
        }

        protected override void FinishedTurnOff()
        {
            ClearDialog();

            gameObject.SetActive(false);

            base.FinishedTurnOff();
        }

        public void SetDialog(Dialog dialog, bool changeOwner = true)
        {
            ClearDialog();

            currentDialog = dialog;

            if(changeOwner)
                ownerName.text = currentDialog.GetDialogOwner;

            if (currentState == WindowState.ON)
            {
                dialogRoutine = StartCoroutine(DialogRoutine(() => onDialogFinishEvent?.Invoke()));
            }
            else
                TurnOn();
        }

        public void ClearDialog()
        {
            ownerName.text = string.Empty;
            dialogText.ClearText();

            currentDialog = null;
        }

        IEnumerator DialogRoutine(Action callback)
        {
            string[] texts = currentDialog.GetDialogTexts;

            int i = 0;

            while (i < texts.Length)
            {
                dialogText.StartTypingText(texts[i]);

                yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));

                if (dialogText.isExecuting)
                {
                    dialogText.StopCurrentTyping();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.E));
                }

                i++;

                yield return new WaitForEndOfFrame();
            }

            callback?.Invoke();
        }
    }
}