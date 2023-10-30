namespace Project.Structures.Iteraction.Dialog
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Dialog", menuName = "GameAssets/Dialog")]
    public class Dialog : ScriptableObject
    {
        [SerializeField]
        protected string owner;
        [SerializeField]
        protected string[] texts;

        public string GetDialogOwner => owner;
        public string[] GetDialogTexts => texts;
        public void SetForceDialogTexts(string [] newTexts)
        {
            texts = newTexts;
        }
    }
}