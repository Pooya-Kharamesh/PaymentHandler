namespace PaymentHandler.Entities
{
    using System;

    public class MembershipItem : Item
    {
        public Guid MembershipId { get; set; }

        public string MembershipEmail { get; set; }

        public MembershipClass MembershipClass { get; set; }
    }
}