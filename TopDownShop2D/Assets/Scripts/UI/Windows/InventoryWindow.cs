namespace Project.UI.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Project.Enums;
    using Project.Structures.Inventory;
    using Project.Utils;
    using Project.Core;
    using UnityEngine.UI;
    using TMPro;

    public class InventoryWindow : WindowBase
    {
        // Grid tile prefab to instantiate
        [SerializeField]
        private InventoryItemHolder holderPrefab;

        // Grid pivot
        [SerializeField]
        private Transform parent;

        [SerializeField]
        private InventoryItemHolder equipedCloth;
        [SerializeField]
        private InventoryItemHolder equipedHat;

        // Item info ballon and confirm button
        [SerializeField]
        private TextMeshProUGUI itemDetailsText;
        [SerializeField]
        private GameObject equipButton;
        [SerializeField]
        private GameObject unequipButton;

        [Header("Pop animation config")]
        [SerializeField]
        private AnimationCurve turnOnCurve;
        [SerializeField]
        private float turnOnTime;
        [SerializeField]
        private Vector3 onPosition;

        [Space]

        [SerializeField]
        private AnimationCurve turnOffCurve;
        [SerializeField]
        private float turnOffTime;
        [SerializeField]
        private Vector3 offPosition;

        Coroutine currentPopRoutine = null;

        private List<InventoryItemHolder> holdersList = new List<InventoryItemHolder>();

        private Inventory inventory = null;
        private InventoryItemHolder currentSelectedItem = null;
        private InventoryItemHolder currentEquipSlotSelected = null;

        private void OnClothEquiped(Item item)
        {
            equipedCloth.Initialize(new InventoryItem(item, 0));

            if (item == null)
            {
                currentEquipSlotSelected = null;
                unequipButton.SetActive(false);
                equipedCloth.UnselectInventoryItem();
                itemDetailsText.text = string.Empty;
                return;
            }

            OnEquipSlotSelected(equipedCloth);
        }

        private void OnHatEquiped(Item item)
        {
            equipedHat.Initialize(new InventoryItem(item, 0));

            if (item == null)
            {
                currentEquipSlotSelected = null;
                unequipButton.SetActive(false);
                equipedHat.UnselectInventoryItem();
                itemDetailsText.text = string.Empty;
                return;
            }

            OnEquipSlotSelected(equipedHat);
        }

        public override void TurnOn()
        {
            if (currentState != WindowState.OFF) return;

            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            transform.localPosition = offPosition;
            gameObject.SetActive(true);

            itemDetailsText.text = string.Empty;
            equipButton.SetActive(false);
            unequipButton.SetActive(false);

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.MoveRoutine(transform, transform.localPosition, onPosition,
                                                        turnOnCurve, turnOnTime, FinishedTurnOn);
        }

        public override void TurnOff()
        {
            if (currentState != WindowState.ON) return;

            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            for (int i = holdersList.Count - 1; i >= 0; i--)
            {
                InventoryItemHolder holder = holdersList[i];
                holdersList.RemoveAt(i);
                Destroy(holder.gameObject);
            }

            equipedCloth.onItemRequested -= OnEquipSlotSelected;
            equipedHat.onItemRequested -= OnEquipSlotSelected;

            inventory = null;

            if (currentEquipSlotSelected != null)
            {
                currentEquipSlotSelected.UnselectInventoryItem();
                currentEquipSlotSelected = null;
            }

            currentSelectedItem = null;

            itemDetailsText.text = string.Empty;
            equipButton.SetActive(false);
            unequipButton.SetActive(false);

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.MoveRoutine(transform, transform.localPosition, offPosition,
                                                        turnOffCurve, turnOffTime, FinishedTurnOff);
        }

        protected override void FinishedTurnOn()
        {
            base.FinishedTurnOn();

            inventory = DataController.GetPlayerInventory();

            foreach (InventoryItem item in inventory.GetInventoryItems())
            {
                InventoryItemHolder holder = Instantiate(holderPrefab, parent);
                holdersList.Add(holder);
                holder.Initialize(item);
                holder.onItemRequested += OnItemRequested;
            }

            equipedCloth.onItemRequested += OnEquipSlotSelected;
            equipedHat.onItemRequested += OnEquipSlotSelected;
        }

        protected override void FinishedTurnOff()
        {
            gameObject.SetActive(false);

            base.FinishedTurnOff();
        }

        public void SetGridInfo(Inventory inventory)
        {
            this.inventory = inventory;
        }

        private void OnItemRequested(InventoryItemHolder holder)
        {
            if (currentSelectedItem == holder) return;

            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            if (currentEquipSlotSelected != null)
                currentEquipSlotSelected.UnselectInventoryItem();

            if (holder.inventoryItem == null)
            {
                itemDetailsText.text = string.Empty;
                equipButton.SetActive(false);
                unequipButton.SetActive(false);
                return;
            }

            currentSelectedItem = holder;
            holder.SelectInventoryItem();

            Item item = holder.inventoryItem.item;
            itemDetailsText.text = $"{item.GetItemName()} : {item.GetItemDescription()}";

            equipButton.SetActive(true);
            unequipButton.SetActive(false);
        }

        private void OnEquipSlotSelected(InventoryItemHolder holder)
        {
            if (currentEquipSlotSelected == holder) return;

            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            if (currentEquipSlotSelected != null)
                currentEquipSlotSelected.UnselectInventoryItem();

            if (holder.inventoryItem == null)
            {
                itemDetailsText.text = string.Empty;
                equipButton.SetActive(false);
                unequipButton.SetActive(false);
                return;
            }

            currentEquipSlotSelected = holder;
            holder.SelectInventoryItem();

            Item item = holder.inventoryItem.item;
            itemDetailsText.text = $"{item.GetItemName()} : {item.GetItemDescription()}";

            equipButton.SetActive(false);
            unequipButton.SetActive(true);
        }

        public void OnEquipButtonPressed()
        {
            //DataController.EquipItem(currentSelectedItem.inventoryItem.item);
        }

        public void OnUnequipButtonPressed()
        {
            //DataController.UnequipItem(currentEquipSlotSelected.inventoryItem.item.GetItemType());
        }
    }
}