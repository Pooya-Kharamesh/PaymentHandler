namespace PaymentHandler.Business.Interfaces
{
    using System;
    using PaymentHandler.Entities;

    /// <summary>
    /// A special rule that can make changes to an order if needed
    /// </summary>
    public interface ICartRule
    {
        /// <summary>
        /// returns a Func that assesses if this rule is applicable to the order or not 
        /// </summary>
        public Func<Order, bool> IsApplicable { get; }

        /// <summary>
        /// An action that executes the rule
        /// </summary>
        public Action<Order> Apply { get; }
    }
}
