namespace PaymentHandler.Entities
{
    /// <summary>
    /// A line in the cart 
    /// </summary>
    public class CartItem
    {
        public Item Item { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal { get; set; }

        public override string ToString() => $"{Item}  *  {Quantity}";
    }
}