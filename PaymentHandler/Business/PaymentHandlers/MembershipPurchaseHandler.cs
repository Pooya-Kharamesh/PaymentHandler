namespace PaymentHandler.Business.PaymentHandlers
{
    using System;
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Entities;
    public class MembershipPurchaseHandler : IPaymentHandler
    {
        private readonly IMembershipRepository membershipRepository;
        private readonly IEmailClient emailClient;

        public MembershipPurchaseHandler(IMembershipRepository membershipRepository, IEmailClient emailClient)
        {
            this.membershipRepository = membershipRepository;
            this.emailClient = emailClient;
        }

        public async Task ProcessAsync(Order order)
        {
            foreach (var cartItem in order.CartItems.Values)
            {
                var membershipItem = cartItem.Item as MembershipItem;
                if (membershipItem == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(membershipItem.MembershipEmail))
                {
                    throw new ArgumentNullException(nameof(membershipItem.MembershipEmail));
                }

                if (Equals(membershipItem.MembershipId, Guid.Empty))
                {
                    throw new ArgumentException($"{nameof(membershipItem.MembershipEmail)} is not set.");
                }

                string message = string.Empty;
                string title = string.Empty;

                // TODO : the if statement should check the item SKU and act based on that; kept is like this for simplicity
                if (cartItem.Item.Name == Constants.ItemNames.MembershipActivation)
                {


                    // TODO: ValidateMembership Id and throw if it is not valid or doesnt exist; Alternatively it could also happens in the repo

                    await membershipRepository.ActivateMembership(membershipItem.MembershipId);
                    title = Messages.MembershipActivationEmailTitle;
                    message = Messages.MembershipActivationEmailTemplate;
                }
                else if (cartItem.Item.Name == Constants.ItemNames.MembershipUpgrade)
                {
                    // TODO: ValidateMembership class to verify the new class is higher than the current one (downgrade is not supported here)

                    await membershipRepository.UpgradeMembership(membershipItem.MembershipId, membershipItem.MembershipClass);
                    title = Messages.MembershipUpgradeEmailTitle;
                    message = string.Format(Messages.MembershipActivationEmailTemplate, membershipItem.MembershipClass);
                }

                await emailClient.Send(membershipItem.MembershipEmail, title, message);
            }

            //TODO : optional sending an email to the customer with all the memberships that got activated or upgraded
            //await this.emailClient.Send(order.Customer.Address, title, message);

        }
    }
}