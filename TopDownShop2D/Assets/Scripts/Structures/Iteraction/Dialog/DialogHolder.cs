namespace Project.Structures.Iteraction.Dialog
{
    using System;

    [Serializable]
    public class DialogHolder
    {
        public Dialog dialog;
        public Action onDialogFinishCallback;
    }
}