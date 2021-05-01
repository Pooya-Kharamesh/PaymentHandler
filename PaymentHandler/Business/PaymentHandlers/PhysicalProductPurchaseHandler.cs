namespace PaymentHandler.Business.PaymentHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Entities;

    public class PhysicalProductPurchaseHandler : IPaymentHandler
    {
        private readonly IPackageSlipPrinter packageSlipPrinter;
        private readonly ICommissionHandler commissionHandler;

        public PhysicalProductPurchaseHandler(IPackageSlipPrinter packageSlipPrinter, ICommissionHandler commissionHandler)
        {
            this.packageSlipPrinter = packageSlipPrinter;
            this.commissionHandler = commissionHandler;
        }

        public async Task ProcessAsync(Order order)
        {
            await SendPackageSlip(order);
            await SendCommission(order);
        }

        private async Task SendCommission(Order order)
        {
            foreach (var cartItem in order.CartItems.Values)
            {
                var item = cartItem.Item as PhysicalItem;

                if (item == null)
                {
                    continue;
                }

                if (item.AgentId.HasValue)
                {
                    await commissionHandler.Send(item.AgentId.Value, item, cartItem.Quantity);
                }
            }
        }

        private async Task SendPackageSlip(Order order)
        {
            string message = string.Format(Messages.PackageSlipTemplate, order.Customer.CustomerInfo, Constants.StoreInformation.NameAndAddress, string.Join(Environment.NewLine, order.CartItems.Values));

            await packageSlipPrinter.Print(order.Customer.Address, message);

            if (IsBookOrder(order.CartItems))
            {
                await packageSlipPrinter.Print(Constants.RoyaltyDepartmentAddress, message);
            }
        }

        // TODO this could be improved by change of design
        private bool IsBookOrder(Dictionary<string, CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                if (item.Value.Item.Category == ItemCategory.Books)
                {
                    return true;
                }
            }

            return false;
        }
    }
}