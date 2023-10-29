namespace Project.Structures.Inventory
{
    using UnityEngine.Events;

    public class InventoryHolder
    {
        public Inventory inventory;
        public UnityEvent onInventoryClosedCallback;
    }
}