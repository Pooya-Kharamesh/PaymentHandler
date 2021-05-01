namespace PaymentHandler.Business
{
    using System;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Business.PaymentHandlers;
    using PaymentHandler.Entities;

    internal class OrderProcessor
    {
        private readonly ICommissionHandler commissionHandler;
        private readonly IPackageSlipPrinter packageSlipPrinter;
        private readonly IEmailClient emailClient;
        private readonly IMembershipRepository membershipRepository;
        private readonly ISpecialRuleRepository specialRuleRepository;

        // TODO :this should be done by DI framework and cleaned up from this class
        public OrderProcessor(
            ICommissionHandler commissionHandler,
            IPackageSlipPrinter packageSlipPrinter,
            IEmailClient emailClient,
            IMembershipRepository membershipRepository,
            ISpecialRuleRepository specialRuleRepository)
        {
            this.commissionHandler = commissionHandler;
            this.packageSlipPrinter = packageSlipPrinter;
            this.emailClient = emailClient;
            this.membershipRepository = membershipRepository;
            this.specialRuleRepository = specialRuleRepository;
        }

        public void Process(Order order)
        {
            if (!IsValideOrder())
            {
                throw new Exception("An invalid order is encountered.");
            }

            var handler = GetHandlingStrategy(order);
            handler.ProcessAsync(order);

        }

        private bool IsValideOrder()
        {
            //TODO: check the order is consistence with the cartItems
            return true;
        }

        public IPaymentHandler GetHandlingStrategy(Order order)
        {
            switch (order.OrderType)
            {
                case OrderType.PhysicalProductPurchase:
                    return new PhysicalProductPurchaseHandler(this.packageSlipPrinter, this.commissionHandler);

                case OrderType.MembershipPurchase:
                    return new MembershipPurchaseHandler(this.membershipRepository, this.emailClient);

                case OrderType.DigitalProductPurchase:
                    return new DigitalProductPurchaseHandler(this.specialRuleRepository);
                case OrderType.Unkown:
                default:
                    throw new Exception($"Order type {order.OrderType} is not supported.");
            }
        }
    }
}
