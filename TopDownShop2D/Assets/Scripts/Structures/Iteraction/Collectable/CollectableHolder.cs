namespace Project.Structures.Iteraction.Collectable
{
    using System;
    using Project.Structures.Inventory;
    using Project.Structures.Iteraction.Dialog;

    [Serializable]

    public class CollectableHolder
    {
        public Item item;
        public Dialog dialog;
        public Action onCollectFinishCallback;
    }
}
