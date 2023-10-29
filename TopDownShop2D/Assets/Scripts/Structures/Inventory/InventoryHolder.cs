namespace Project.Structures.Inventory
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class InventoryHolder
    {
        public Inventory inventory;
        public Action onInventoryClosedCallback;
    }
}