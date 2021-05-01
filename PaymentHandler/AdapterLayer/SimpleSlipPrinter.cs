namespace PaymentHandler.AdapterLayer
{
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;

    internal class SimpleSlipPrinter : IPackageSlipPrinter
    {
        ///<inheritdoc />
        public async Task Print(string to, string content)
        {
            // create a message 
            // Print the slip or just send an email 
        }
    }
}