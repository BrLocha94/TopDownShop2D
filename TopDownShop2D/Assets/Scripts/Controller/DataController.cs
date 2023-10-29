namespace Project.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Utils;
    using Project.Structures.Inventory;
    using Project.Enums;

    public static class DataController
    {
        private static Action<int> onPlayerMoneyChanged;
        private static Action<Inventory> onPlayerInventoryChanged;

        private static int playerMoney;
        private static Inventory playerInventory;

        public static void Initialize()
        {
            //ADD GAME SAVE AND LOAD LOGIC

            playerMoney = 1000;
            playerInventory = new Inventory();
        }

        public static int GetCurrentPlayerMoney => playerMoney;
        public static Inventory GetPlayerInventory() => playerInventory;

        public static bool BuyItem(InventoryItem item)
        {
            if (item.price > playerMoney) return false;

            playerInventory.AddItem(item);
            onPlayerInventoryChanged?.Invoke(playerInventory);

            playerMoney -= item.price;
            onPlayerMoneyChanged?.Invoke(playerMoney);

            return true;
        }

        public static bool SellItem(InventoryItem item)
        {
            // Implement sell logic

            return false;
        }
    }
}