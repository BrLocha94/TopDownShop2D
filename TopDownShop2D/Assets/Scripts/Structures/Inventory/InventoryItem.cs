namespace Project.Structures.Inventory
{
    public class InventoryItem
    {
        public Item item { get; private set; }
        public int price { get; private set; }

        public InventoryItem(Item item, int price)
        {
            this.item = item;
            this.price = price;
        }

        public void UpdatePrice(int price)
        {
            if (price < 0) return;

            this.price = price;
        }
    }
}