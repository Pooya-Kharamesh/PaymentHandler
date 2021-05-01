namespace PaymentHandler.Business.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using PaymentHandler.Entities;

    /// <summary>
    /// A repository to interact and apply changes to the memberships
    /// </summary>
    public interface IMembershipRepository
    {
        /// <summary>
        /// Activates a membership if the membership is available and not activated
        /// </summary>
        /// <param name="membershipId">A given membership Id</param>
        /// <returns></returns>
        public Task ActivateMembership(Guid membershipId);

        /// <summary>
        /// Upgrade a membership or throws error if membership is not found or is getting downgraded
        /// </summary>
        /// <param name="membershipId">A given membership Id</param>
        /// <param name="membershipClass">A given membership class to upgrade to.</param>
        /// <returns></returns>
        public Task UpgradeMembership(Guid membershipId, MembershipClass membershipClass);
    }
}