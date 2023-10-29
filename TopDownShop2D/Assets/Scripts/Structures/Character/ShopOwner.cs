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

        public void OpenShop()
        {
            if (itemList.Count == 0)
            {
                Debug.Log("There are no item associated with this shop owner object");
                return;
            }

            //OpenShop????
        }
    }
}