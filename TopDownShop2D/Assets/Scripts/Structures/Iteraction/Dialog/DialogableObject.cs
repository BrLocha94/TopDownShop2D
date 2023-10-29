namespace Project.Structures.Iteraction.Dialog
{
    using Project.Enums;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Core;

    public sealed class DialogableObject : IteractionObjectBase
    {
        [Header("Dialogable Object")]
        [SerializeField]
        private List<DialogHolder> dialogList = new List<DialogHolder>();

        private int dialogIndex = 0;

        public override void ExecuteIteraction(Direction direction)
        {
            if(dialogList.Count == 0)
            {
                Debug.Log("There are no dialog associated with this object");
                return;
            }

            base.ExecuteIteraction(direction);

            dialogIndex = 0;

            GameController.Instance.ExecuteSimpleDialog(dialogList[dialogIndex]);
        }

        public override void AdvanceIteraction()
        {
            dialogIndex++;
            if (dialogIndex >= dialogList.Count)
            {
                dialogIndex = 0;
                GameController.Instance.CloseDialog();
                return;
            }

            GameController.Instance.ExecuteSimpleDialog(dialogList[dialogIndex]);
        }
    }
}