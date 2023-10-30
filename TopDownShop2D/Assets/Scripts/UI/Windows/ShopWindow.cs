namespace Project.UI.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Project.Utils;
    using Project.Structures.Inventory;
    using Project.Structures.Iteraction.Dialog;
    using Project.Enums;
    using Project.Core;
    using Project.Contracts;

    public class ShopWindow : WindowBase, IReceiver<GameState>
    {
        [Header("Dialog options")]
        [SerializeField]
        private DialogHolder dialogConfirmBuy;
        [SerializeField]
        private DialogHolder dialogCongratsBuy;
        [SerializeField]
        private DialogHolder dialogDeniedBuy;
        [SerializeField]
        private DialogHolder dialogConfirmSell;
        [SerializeField]
        private DialogHolder dialogCongratsSell;
        [SerializeField]
        private DialogHolder dialogDeniedSell;

        [Header("Grid options")]
        // Grid tile prefab to instantiate
        [SerializeField]
        private InventoryItemHolder holderPrefab;
        // Grid pivot
        [SerializeField]
        private Transform parent;

        [Header("Pop animation config")]
        [SerializeField]
        private AnimationCurve turnOnCurve;
        [SerializeField]
        private float turnOnTime;
        [SerializeField]
        private Vector3 onScale;

        [Space]

        [SerializeField]
        private AnimationCurve turnOffCurve;
        [SerializeField]
        private float turnOffTime;
        [SerializeField]
        private Vector3 offScale;

        private List<InventoryItemHolder> holdersList = new List<InventoryItemHolder>();

        private bool shopModeIsBuy = true;

        private Inventory inventory = null;
        private InventoryItemHolder currentSelectedItem = null;

        Coroutine currentPopRoutine = null;
        GameState currentGameState = GameState.NULL;

        public override void TurnOn()
        {
            if (currentState != WindowState.OFF) return;

            if (currentPopRoutine != null)
            {
                StopCoroutine(currentPopRoutine);
                currentPopRoutine = null;
            }
            
            transform.localScale = offScale;

            gameObject.SetActive(true);
            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.RescaleRoutine(transform, transform.localScale, onScale,
                                                        turnOnCurve, turnOnTime, FinishedTurnOn);
        }

        public override void TurnOff()
        {
            if (currentState != WindowState.ON) return;

            if (currentPopRoutine != null)
            {
                StopCoroutine(currentPopRoutine);
                currentPopRoutine = null;
            }

            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            currentSelectedItem = null;

            for (int i = holdersList.Count - 1; i >= 0; i--)
            {
                InventoryItemHolder holder = holdersList[i];
                holdersList.RemoveAt(i);
                Destroy(holder.gameObject);
            }

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.RescaleRoutine(transform, transform.localScale, offScale,
                                                        turnOffCurve, turnOffTime, FinishedTurnOff);
        }

        protected override void FinishedTurnOn()
        {
            base.FinishedTurnOn();

            foreach (InventoryItem item in inventory.GetInventoryItems())
            {
                InventoryItemHolder holder = Instantiate(holderPrefab, parent);
                holdersList.Add(holder);
                holder.Initialize(item);
                holder.onItemRequested += OnItemRequested;
            }
        }

        protected override void FinishedTurnOff()
        {
            inventory = null;
            currentSelectedItem = null;

            gameObject.SetActive(false);

            base.FinishedTurnOff();
        }

        public void SetGridInfo(Inventory inventory, bool shopModeIsBuy = true)
        {
            this.inventory = inventory;
            this.shopModeIsBuy = shopModeIsBuy;
        }

        private void OnItemRequested(InventoryItemHolder holder)
        {
            if (currentGameState != GameState.SHOP) return;

            if (currentSelectedItem == holder) return;

            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            currentSelectedItem = holder;
            holder.SelectInventoryItem();

            Item item = holder.inventoryItem.item;

            if (shopModeIsBuy)
            {
                string[] confirmText = new string[] { $"Buy {item.GetItemName()} : {item.GetItemDescription()}." , "Press SPACE to confirm" };
                dialogConfirmBuy.dialog.SetForceDialogTexts(confirmText);
                GameController.Instance.ExecuteShopDialog(dialogConfirmBuy);
            }
            else
            {
                string[] confirmText = new string[] { $"Sell {item.GetItemName()} : {item.GetItemDescription()}.", " Press SPACE to confirm" };
                dialogConfirmSell.dialog.SetForceDialogTexts(confirmText);
                GameController.Instance.ExecuteShopDialog(dialogConfirmSell);
            }
        }

        public void ConfirmBuy()
        {
            if (currentState != WindowState.ON) return;

            if (currentSelectedItem == null) return;

            Debug.Log($"Try to buy {currentSelectedItem.inventoryItem.item.GetItemName()}");

            bool sucess = DataController.BuyItem(currentSelectedItem.inventoryItem);

            Debug.Log($"Sucess on Buy = {sucess}");

            //CHECK SUCESS TO EXIBIT IMAGE
            if (sucess)
            {
                inventory.RemoveItem(currentSelectedItem.inventoryItem);
                GameController.Instance.ExecuteShopDialog(dialogCongratsBuy);
            }
            else
                GameController.Instance.ExecuteShopDialog(dialogDeniedBuy);
        }

        public void ConfirmSell()
        {
            if (currentState != WindowState.ON) return;

            if (currentSelectedItem == null) return;

            Debug.Log($"Try to sell {currentSelectedItem.inventoryItem.item.GetItemName()}");

            bool sucess = DataController.SellItem(currentSelectedItem.inventoryItem);

            Debug.Log($"Sucess on Sell = {sucess}");

            //CHECK SUCESS TO EXIBIT IMAGE
            if (sucess)
                GameController.Instance.ExecuteShopDialog(dialogCongratsSell);
            else
                GameController.Instance.ExecuteShopDialog(dialogDeniedSell);
        }

        public void OnBuyConfirmMessageFinish()
        {

        }

        public void OnBuyCongratsMessageFinish()
        {
            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            currentSelectedItem = null;

            for (int i = holdersList.Count - 1; i >= 0; i--)
            {
                InventoryItemHolder holder = holdersList[i];
                holdersList.RemoveAt(i);
                Destroy(holder.gameObject);
            }

            foreach (InventoryItem item in inventory.GetInventoryItems())
            {
                InventoryItemHolder holder = Instantiate(holderPrefab, parent);
                holdersList.Add(holder);
                holder.Initialize(item);
                holder.onItemRequested += OnItemRequested;
            }
        }

        public void OnBuyDeniedMessageFinish()
        {
            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            currentSelectedItem = null;
        }

        public void OnSellConfirmMessageFinish()
        {
            
        }

        public void OnSellCongratsMessageFinish()
        {
            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            currentSelectedItem = null;

            for (int i = holdersList.Count - 1; i >= 0; i--)
            {
                InventoryItemHolder holder = holdersList[i];
                holdersList.RemoveAt(i);
                Destroy(holder.gameObject);
            }

            inventory = DataController.GetPlayerInventory();

            foreach (InventoryItem item in inventory.GetInventoryItems())
            {
                InventoryItemHolder holder = Instantiate(holderPrefab, parent);
                holdersList.Add(holder);
                holder.Initialize(item);
                holder.onItemRequested += OnItemRequested;
            }
        }

        public void OnSellDeniedMessageFinish()
        {
            if (currentSelectedItem != null)
                currentSelectedItem.UnselectInventoryItem();

            currentSelectedItem = null;
        }

        private void Update()
        {
            if (currentGameState != GameState.SHOP) return;

            if (currentSelectedItem == null)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (shopModeIsBuy)
                    ConfirmBuy();
                else
                    ConfirmSell();
            }
        }

        public virtual void ReceiveUpdate(GameState updatedValue)
        {
            currentGameState = updatedValue;
        }
    }
}