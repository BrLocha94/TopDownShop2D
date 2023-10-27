namespace Project.Structures.Inventory
{
    using UnityEngine;
    using Project.Enums;

    [CreateAssetMenu(fileName = "Item", menuName = "GameAssets/Item")]
    public class Item : ScriptableObject
    {
        [Header("Iten basic information")]
        [SerializeField]
        protected ItemType itemType = ItemType.Null;
        [SerializeField]
        protected Sprite itemSprite = null;
        [SerializeField]
        protected Sprite itemIcon = null;
        [SerializeField]
        protected Color itemColor = Color.white;
        [SerializeField]
        protected string itemName = string.Empty;
        [SerializeField]
        protected int itemPrice = 1;
        [SerializeField]
        [TextArea(1, 5)]
        protected string itemDescription = string.Empty;

        public ItemType GetItemType() => itemType;
        public Sprite GetItemSprite() => itemSprite;
        public Sprite GetItemIcon() => itemIcon;
        public Color GetItemColor() => itemColor;
        public string GetItemName() => itemName;
        public int GetItemPrice() => itemPrice;
        public string GetItemDescription() => itemDescription;
    }
}