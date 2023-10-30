namespace Project.Core
{
    using System;
    using Project.Structures.Inventory;
    using Project.Enums;

    public static class DataController
    {
        public static Action<Item> onClothEquiped;
        public static Action<Item> onHatEquiped;
        public static Action<int> onPlayerMoneyChanged;
        public static Action<Inventory> onPlayerInventoryChanged;

        private static int playerMoney;
        private static Inventory playerInventory;
        private static Item equipedCloth = null;
        private static Item equipedHat = null;

        public static void Initialize()
        {
            //ADD GAME SAVE AND LOAD LOGIC

            playerMoney = 50;
            onPlayerMoneyChanged?.Invoke(playerMoney);
            equipedCloth = null;
            equipedHat = null;
            playerInventory = new Inventory();
        }

        public static int GetCurrentPlayerMoney => playerMoney;
        public static Item GetCurrentEquipedCloth() => equipedCloth;
        public static Item GetCurrentEquipedHat() => equipedHat;
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

        public static bool SellItem(InventoryItem inventoryItem)
        {
            bool result = playerInventory.RemoveItem(inventoryItem);

            if (result)
            {
                playerMoney += inventoryItem.price;
                onPlayerMoneyChanged?.Invoke(playerMoney);

                if (equipedCloth != null)
                {
                    if (inventoryItem.item.Equals(equipedCloth))
                        UnequipItem(ItemType.Cloth);
                }

                if(equipedHat != null)
                {
                    if (inventoryItem.item.Equals(equipedHat))
                        UnequipItem(ItemType.Hat);
                }

                onPlayerInventoryChanged?.Invoke(playerInventory);
                return true;
            }

            return false;
        }

        public static void EquipItem(Item item)
        {
            if (item.GetItemType() == ItemType.Cloth)
            {
                equipedCloth = item;
                onClothEquiped?.Invoke(item);
                return;
            }

            if (item.GetItemType() == ItemType.Hat)
            {
                equipedHat = item;
                onHatEquiped?.Invoke(item);
                return;
            }
        }

        public static void UnequipItem(ItemType itemType)
        {
            if (itemType == ItemType.Cloth)
            {
                equipedCloth = null;
                onClothEquiped?.Invoke(null);
                return;
            }

            if (itemType == ItemType.Hat)
            {
                equipedHat = null;
                onHatEquiped?.Invoke(null);
                return;
            }
        }
    }
}