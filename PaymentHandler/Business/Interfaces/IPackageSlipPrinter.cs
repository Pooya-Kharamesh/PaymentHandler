namespace PaymentHandler.Business.Interfaces
{
    using System.Threading.Tasks;

    public interface IPackageSlipPrinter
    {
        /// <summary>
        /// Prints a package slip 
        /// </summary>
        /// <param name="to">Address of the receiver </param>
        /// <param name="content">Content of the slip</param>
        public Task Print(string to, string content);
    }
}