namespace PaymentHandler.AdapterLayer
{
    using System.Threading.Tasks;
    using PaymentHandler.Business.Interfaces;

    internal class SMTPClient : IEmailClient
    {
        public async Task Send(string to, string title, string content)
        {
            // send email
            await Task.CompletedTask;
        }
    }
}