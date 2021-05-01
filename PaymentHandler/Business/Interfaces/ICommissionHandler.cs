namespace PaymentHandler.Business.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface ICommissionHandler
    {
        /// <summary>
        /// Generates a commission payment to the agent. 
        /// </summary>
        /// <param name="agentId">Agent that receives the commission</param>
        /// <param name="item">Item which was sold</param>
        /// <param name="quantity">Quantity of item</param>
        Task Send(Guid agentId, Item item, int quantity);
    }
}