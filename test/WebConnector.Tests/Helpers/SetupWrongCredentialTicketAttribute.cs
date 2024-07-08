using QbSync.WebConnector.Tests.Models;
using System;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupWrongCredentialTicketAttribute : AuthenticatorAttribute
    {
        public SetupWrongCredentialTicketAttribute()
        {
            AuthenticatedTicket = new AuthenticatedTicket
            {
                authenticated = false,
                ticket = Guid.NewGuid().ToString()
            };
        }
    }
}
