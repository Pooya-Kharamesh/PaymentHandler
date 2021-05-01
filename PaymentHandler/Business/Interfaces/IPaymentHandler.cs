namespace PaymentHandler.Business.Interfaces
{
    using System.Threading.Tasks;
    using PaymentHandler.Entities;

    /// <summary>
    /// Handler for a payment
    /// </summary>
    internal interface IPaymentHandler
    {
        /// <summary>
        /// Processes a payment
        /// </summary>
        /// <param name="order">A given order</param>
        public Task ProcessAsync(Order order);
    }
}