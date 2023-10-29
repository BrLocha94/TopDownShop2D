namespace Project.UI.Windows
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Project.Utils;
    using Project.Structures.Inventory;
    using Project.Enums;
    using Project.Core;

    public class ShopWindow : WindowBase
    {
        // Grid tile prefab to instantiate
        //[SerializeField]
        //private InventoryItemHolder holderPrefab;

        // Grid pivot
        [SerializeField]
        private Transform parent;

        // Item info ballon and confirm button
        [SerializeField]
        private GameObject confimButton;

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

        //private List<InventoryItemHolder> holdersList = new List<InventoryItemHolder>();

        private Inventory inventory = null;
        //private InventoryItemHolder currentSelectedItem = null;

        Coroutine currentPopRoutine = null;

        public override void TurnOn()
        {
            if (currentState != WindowState.OFF) return;

            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            transform.localScale = offScale;

            //itemDetailsText.text = string.Empty;
            //confimButton.SetActive(false);
            gameObject.SetActive(true);

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.RescaleRoutine(transform, transform.localScale, onScale,
                                                        turnOnCurve, turnOnTime, FinishedTurnOn);
        }

        public override void TurnOff()
        {
            if (currentState != WindowState.ON) return;

            if (currentPopRoutine != null)
                StopCoroutine(currentPopRoutine);

            /*
            for (int i = holdersList.Count - 1; i >= 0; i--)
            {
                InventoryItemHolder holder = holdersList[i];
                holdersList.RemoveAt(i);
                Destroy(holder.gameObject);
            }
            */

            currentState = WindowState.ANIMATING;

            currentPopRoutine = this.RescaleRoutine(transform, transform.localScale, offScale,
                                                        turnOffCurve, turnOffTime, FinishedTurnOff);
        }

        protected override void FinishedTurnOn()
        {
            base.FinishedTurnOn();

            /*
            foreach (InventoryItem item in inventory.GetInventoryItems())
            {
                //InventoryItemHolder holder = Instantiate(holderPrefab, parent);
                //holdersList.Add(holder);
                //holder.Initialize(item);
                //holder.onItemRequested += OnItemRequested;
            }
            */
        }

        protected override void FinishedTurnOff()
        {
            inventory = null;
            //currentSelectedItem = null;

            gameObject.SetActive(false);
            //confimButton.SetActive(false);

            base.FinishedTurnOff();
        }

        public void SetGridInfo(Inventory inventory)
        {
            this.inventory = inventory;
        }
    }
}