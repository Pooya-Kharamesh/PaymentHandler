namespace PaymentHandler.Business.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Handles the interactions with an email protocol 
    /// </summary>
    public interface IEmailClient
    {
        /// <summary>
        /// Sends an email 
        /// </summary>
        /// <param name="to">Receiver email address</param>
        /// <param name="title">Email title</param>
        /// <param name="content">Email content</param>
        /// <returns></returns>
        Task Send(string to, string title, string content);
    }
}