namespace Project.Structures.Iteraction.Dialog
{
    using System;
    using Project.Structures.Inventory;

    [Serializable]

    public class CollectableHolder
    {
        public Item item;
        public Dialog dialog;
        public Action onCollectFinishCallback;
    }
}
