namespace Project.Structures.Iteraction.Dialog
{
    using System;
    using UnityEngine.Events;

    [Serializable]
    public class DialogHolder
    {
        public Dialog dialog;
        public UnityEvent onDialogFinishCallback;
    }
}