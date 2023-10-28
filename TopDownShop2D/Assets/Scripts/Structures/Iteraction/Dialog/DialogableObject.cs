namespace Project.Structures.Iteraction.Dialog
{
    using Project.Enums;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class DialogableObject : IteractionObjectBase
    {
        [Header("Dialogable Object")]
        [SerializeField]
        private List<DialogHolder> dialogList = new List<DialogHolder>();

        private int dialogIndex = 0;

        private void Awake()
        {
            foreach (DialogHolder dialogHolder in dialogList)
            {
                dialogHolder.onDialogFinishCallback += AdvanceIteraction;
            }
        }

        public override void ExecuteIteraction(Direction direction)
        {
            if(dialogList.Count == 0)
            {
                Debug.Log("There are no dialog associated with this object");
                return;
            }

            base.ExecuteIteraction(direction);

            dialogIndex = 0;

            Debug.Log("Executed dialogs");
        }

        protected override void AdvanceIteraction()
        {
            dialogIndex++;
            if (dialogIndex >= dialogList.Count)
            {
                dialogIndex = 0;
                //CLOSE DIALOG
                return;
            }

            //EXECUTE CURRENT DIALOG
        }
    }
}