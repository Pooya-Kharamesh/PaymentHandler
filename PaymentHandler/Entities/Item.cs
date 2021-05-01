using PaymentHandler.Entities;

namespace PaymentHandler
{
    public class Item
    {
        public string Name { get; set; }

        public ItemCategory Category { get; set; }

        public decimal DefaultUnitPrice { get; set; }

        public override string ToString() => Name;
    }
}