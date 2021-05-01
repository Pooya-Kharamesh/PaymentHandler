namespace PaymentHandler.AdapterLayer
{
    using System;
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;
    using PaymentHandler.Entities;

    public class MembershipRepository : IMembershipRepository
    {
        public async Task ActivateMembership(Guid membershipId)
        {
            //  Activate a membership
            await Task.CompletedTask;
        }

        public async Task UpgradeMembership(Guid membershipId, MembershipClass membershipClass)
        {
            //  Upgrade a membership
            await Task.CompletedTask;
        }
    }
}