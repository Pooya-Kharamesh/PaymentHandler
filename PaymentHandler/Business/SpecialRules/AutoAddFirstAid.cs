namespace PaymentHandler.Business.SpecialRules
{
    using System;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Entities;

    /// <summary>
    /// If the payment is for the video “Learning to Ski,” add a free “First Aid” video to the packing slip (the result of a court decision in 1997).
    /// There are more cases that are not covered in this implementation like in case of removing the “Learning to Ski,” then “First Aid” stays there for free
    /// </summary>
    public class AutoAddFirstAid : ICartRule
    {
        public Func<Order, bool> IsApplicable => (order) =>
        {
            if (order.CartItems.TryGetValue(Constants.ItemNames.LearnToSkiVideoName, out CartItem cartItem))
            {
                return cartItem.Item.Category == ItemCategory.Video && cartItem.Quantity > 0;
            }

            return false;
        };

        public Action<Order> Apply => (order) =>
             order.AddOrUpdate(
                 new Item()
                 {
                     Name = Constants.ItemNames.FirstAidVideoName,
                     Category = ItemCategory.Video
                 },
                 quantity: 1,
                 lineTotal: 0
                 );

    }
}
