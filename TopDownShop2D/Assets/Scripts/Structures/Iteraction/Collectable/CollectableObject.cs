namespace Project.Structures.Iteraction.Collectable
{
    using UnityEngine;
    using Project.Structures.Iteraction.Dialog;
    using Project.Enums;

    public class CollectableObject : IteractionObjectBase
    {
        [Header("Collectable Object")]
        [SerializeField]
        private CollectableHolder itemToCollect = null;

        private void Awake()
        {
            if(itemToCollect != null)
            {
                itemToCollect.onCollectFinishCallback += AdvanceIteraction;
            }
        }

        public override void ExecuteIteraction(Direction direction)
        {
            if (itemToCollect == null)
            {
                Debug.Log("There are no item associated with this object");
                return;
            }

            base.ExecuteIteraction(direction);

            Debug.Log("Executed collectable");
        }

        protected override void AdvanceIteraction()
        {
            Debug.Log("finished collectable");
        }
    }
}