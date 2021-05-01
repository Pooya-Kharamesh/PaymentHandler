namespace PaymentHandler.Business.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A repository of special rules 
    /// </summary>
    public interface ISpecialRuleRepository
    {
        /// <summary>
        /// Get all the rules available
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ICartRule>> GetAll();
    }
}
