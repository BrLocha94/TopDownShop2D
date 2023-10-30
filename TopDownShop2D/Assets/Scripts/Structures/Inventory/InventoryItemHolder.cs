namespace Project.Structures.Inventory
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using TMPro;

    public class InventoryItemHolder : MonoBehaviour
    {
        public Action<InventoryItemHolder> onItemRequested;

        [SerializeField]
        private bool showValue = true;

        [SerializeField]
        private Image targetImage;
        [SerializeField]
        private TextMeshProUGUI targetLabel;

        [SerializeField]
        private UnityEvent onSelectedEvent;
        [SerializeField]
        private UnityEvent onUnselectedEvent;

        public InventoryItem inventoryItem { get; private set; } = null;

        public void Initialize(InventoryItem inventoryItem)
        {
            this.inventoryItem = inventoryItem;

            if (inventoryItem == null || inventoryItem.item == null)
            {
                targetImage.sprite = null;
                return;
            }

            targetImage.sprite = inventoryItem.item.GetItemIcon();

            if (targetLabel != null)
                targetLabel.text = showValue ? "$" + inventoryItem.item.GetItemPrice().ToString() : inventoryItem.item.GetItemName();
        }

        public void RequestInventoryItem()
        {
            onItemRequested?.Invoke(this);
        }

        public void SelectInventoryItem()
        {
            onSelectedEvent?.Invoke();
        }

        public void UnselectInventoryItem()
        {
            onUnselectedEvent?.Invoke();
        }
    }
}
