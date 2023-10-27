namespace Project.Utils
{
    using Project.Enums;
    using Project.Contracts;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class DebugText : OnlyEditorObject, IReceiver<GameState>
    {
        [SerializeField]
        private Text target;

        public void ReceiveUpdate(GameState updatedValue)
        {
            if (target == null)
            {
                Debug.Log("There are no text assigned to this object");
                return;
            }

            target.text = updatedValue.ToString();
        }
    }
}