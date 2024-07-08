using QbSync.WebConnector.Core;

namespace QbSync.WebConnector.Tests.Models
{
    public class AuthenticatedTicket : IAuthenticatedTicket
    {
        public string ticket { get; set; }
        public string current_step { get; set; }
        public bool authenticated { get; set; }
    }
}
