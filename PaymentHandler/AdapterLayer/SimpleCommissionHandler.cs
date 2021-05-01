namespace PaymentHandler.AdapterLayer
{
    using System;
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;

    internal class SimpleCommissionHandler : ICommissionHandler
    {
        /// <inheritdoc/>
        public async Task Send(Guid agentId, Item item, int quantity)
        {
            // Calculate and Sends the commission to the agent
            await Task.CompletedTask;
        }
    }
}