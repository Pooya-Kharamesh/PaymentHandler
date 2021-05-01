namespace PaymentHandler.Business.PaymentHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Entities;

    public class DigitalProductPurchaseHandler : IPaymentHandler
    {
        private IEnumerable<ICartRule> specialRules;
        private readonly ISpecialRuleRepository specialRulesRepository;

        public DigitalProductPurchaseHandler(ISpecialRuleRepository specialRulesRepository)
        {
            this.specialRulesRepository = specialRulesRepository ?? throw new ArgumentNullException(nameof(specialRulesRepository));
        }

        ///<inheritdoc/>
        public async Task ProcessAsync(Order order)
        {
            if (specialRules == null)
            {
                specialRules = await specialRulesRepository.GetAll() ?? Enumerable.Empty<ICartRule>();
            }

            ApplySpecialRules(order);
        }

        private void ApplySpecialRules(Order order)
        {
            foreach (ICartRule rule in specialRules)
            {
                if (rule.IsApplicable(order))
                {
                    rule.Apply(order);
                }
            }

        }

    }
}