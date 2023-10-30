namespace Project.Structures.Inventory
{
    using System;
    using UnityEngine.Events;

    [Serializable]
    public class InventoryHolder
    {
        public Inventory inventory;
        public UnityEvent onInventoryClosedCallback;

        public void ForceInventory(Inventory newInventory)
        {
            inventory = newInventory;
        }
    }
}