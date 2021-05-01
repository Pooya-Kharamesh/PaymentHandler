namespace PaymentHandler.Entities
{
    using System;

    public class PhysicalItem : Item
    {
        public Guid? AgentId { get; set; }
    }
}