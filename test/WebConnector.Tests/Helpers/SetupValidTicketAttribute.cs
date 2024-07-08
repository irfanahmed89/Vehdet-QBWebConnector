using QbSync.WebConnector.Tests.Models;
using System;

namespace QbSync.WebConnector.Tests.Helpers
{
    class SetupValidTicketAttribute : AuthenticatorAttribute
    {
        public SetupValidTicketAttribute()
        {
            AuthenticatedTicket = new AuthenticatedTicket
            {
                authenticated = true,
                ticket = Guid.NewGuid().ToString()
            };
        }
    }
}