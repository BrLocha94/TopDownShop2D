namespace Project.Structures.Inventory
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Inventory
    {
        private List<InventoryItem> items;

        public List<InventoryItem> GetInventoryItems() => items;

        public Inventory(List<InventoryItem> inventoryItems)
        {
            this.items = inventoryItems;
        }

        public Inventory(params InventoryItem[] inventoryItems)
        {
            this.items = new List<InventoryItem>();

            foreach (InventoryItem item in inventoryItems)
            {
                this.items.Add(item);
            }
        }

        public Inventory(List<Item> itemsList)
        {
            this.items = new List<InventoryItem>();

            foreach (Item item in itemsList)
            {
                this.items.Add(new InventoryItem(item, item.GetItemPrice()));
            }
        }

        public bool AddItem(InventoryItem inventoryItem)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item == inventoryItem.item)
                {
                    // List already have this item
                    // ADD AMOUNT LOGIC
                    return false;
                }
            }

            items.Add(inventoryItem);
            return true;
        }

        public bool RemoveItem(InventoryItem inventoryItem)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item == inventoryItem.item)
                {
                    // ADD AMOUNT LOGIC
                    items.RemoveAt(i);
                    return true;
                }
            }

            Debug.Log("Inventory does not have this item");
            return false;
        }

        public void RemoveItemAt(int index)
        {
            if (index < 0 || index >= items.Count) return;

            //ADD AMOUNT LOGIC

            items.RemoveAt(index);
        }
    }
}