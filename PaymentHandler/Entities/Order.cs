namespace PaymentHandler.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An order made by a customer
    /// </summary>
    public class Order
    {
        public Order()
        {
            CartItems = new Dictionary<string, CartItem>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets or sets the customer who put the order 
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// List of items in the cart
        /// </summary>
        public Dictionary<string, CartItem> CartItems { get; }

        /// <summary>
        /// Specifies type of the order 
        /// for simplicity of this project it is set by user; In a real life scenario - most likely - it would be calculated from the types of the items in the cart
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// Add a given number of items to the cart
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <param name="lineTotal">optional parameter that override the line total</param>
        public void Add(Item item, int quantity, decimal? lineTotal = null)
        {
            if (CartItems.TryGetValue(item.Name, out CartItem cartItem))
            {
                cartItem.Quantity += quantity;
                if (lineTotal.HasValue)
                {
                    cartItem.LineTotal = lineTotal.Value;
                }
            }
            else
            {
                CartItems.Add(
                    item.Name,
                    new CartItem()
                    {
                        Item = item,
                        Quantity = quantity,
                        LineTotal = lineTotal.HasValue ? lineTotal.Value : quantity * item.DefaultUnitPrice
                    });
            }
        }

        /// <summary>
        /// Updates the quantity of an item to the given number; If item does not exist in the cart it will be added
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <param name="lineTotal">optional parameter that override the line total</param>
        public void AddOrUpdate(Item item, int quantity, decimal? lineTotal = null)
        {
            if (CartItems.TryGetValue(item.Name, out CartItem cartItem))
            {
                cartItem.Quantity = quantity;
                if (lineTotal.HasValue)
                {
                    cartItem.LineTotal = lineTotal.Value;
                }
            }
            else
            {
                CartItems.Add(
                    item.Name,
                    new CartItem()
                    {
                        Item = item,
                        Quantity = quantity,
                        LineTotal = lineTotal.HasValue ? lineTotal.Value : quantity * item.DefaultUnitPrice
                    });
            }
        }

    }
}