namespace Project.Structures.Character
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Structures.Inventory;
    using Project.Core;

    public class ShopOwner : NonPlayableCharacter
    {
        [Header("Shop owner configs")]
        [SerializeField]
        private List<Item> itemList = new List<Item>();
        [SerializeField]
        private InventoryHolder inventoryHolder;
        [SerializeField]
        private InventoryHolder playerInventoryHolder;

        private void Awake()
        {
            if (inventoryHolder.inventory == null)
            {
                List<InventoryItem> list = new List<InventoryItem>();

                foreach (Item item in itemList)
                {
                    list.Add(new InventoryItem(item, item.GetItemPrice()));
                }

                Inventory inventory = new Inventory(list);
                inventoryHolder.inventory = inventory;
            }
        }

        public void OpenShop()
        {
            if (itemList.Count == 0)
            {
                Debug.Log("There are no item associated with this shop owner object");
                return;
            }

            GameController.Instance.OpenShop(inventoryHolder);
        }

        public void OpenShopToSell()
        {
            var playerInventory = DataController.GetPlayerInventory();
            playerInventoryHolder.ForceInventory(playerInventory);

            GameController.Instance.OpenShopToSell(playerInventoryHolder);
        }
    }
}