using FrappeQbwcService.FrappeModels;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using QbSync.WebConnector.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Application
{
    public class Authenticator : IAuthenticator
    {
        private readonly ApplicationDbContext dbContext;

        public Authenticator(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<IAuthenticatedTicket?> GetAuthenticationFromLoginAsync(string login, string password)
        {
            // Log in the user via the database.
            //var user = await dbContext.Users
            //    .Where(m => m.UserName == login)
            //    .Where(m => m.Password == password)
            //    .FirstOrDefaultAsync();

            var user = Manager.GetManager().GetUser(login, password);
            var guid = Guid.NewGuid().ToString();
            if (user != null)
            {
                return new QbTicket
                {
                    authenticated = true,
                    ticket = guid,

                    // We store more information about the ticket, such as the user.
                    // Check the extension to learn how to reach for this user.
                    //User = user,
                    //UserId = user.Id
                };
            }

            return new QbTicket
            {
                authenticated = false,
                ticket = guid
            };
        }

        public async Task<IAuthenticatedTicket?> GetAuthenticationFromTicketAsync(string ticket)
        {
            // Fetch the ticket based on the guid.
            var qbTicket = Manager.GetManager().GetTicket(ticket);
            //var qbTicket = await dbContext.QbTickets
            //    .FirstOrDefaultAsync(m => m.Ticket == ticket);

            return qbTicket;
        }

        public async Task SaveTicketAsync(IAuthenticatedTicket ticket)
        {
            // Save the ticket to the database.
            // It contains the information about the current step.
            //var qbTicket = await dbContext.QbTickets
            //    .FirstOrDefaultAsync(m => m.Ticket == ticket.Ticket);
            var qbTicket = Manager.GetManager().GetTicket(ticket.ticket);
            bool isCreate = false;
            if (qbTicket == null)
            {
                if (ticket is QbTicket ticketAsQbTicket)
                {
                    qbTicket = new QbTicket
                    {
                        ticket = ticket.ticket,
                        authenticated = ticket.authenticated,
                        // User = ticketAsQbTicket.User,
                        // UserId = ticketAsQbTicket.UserId
                    };
                    isCreate = true;
                    // dbContext.QbTickets.Add(qbTicket);
                }
            }

            if (qbTicket != null)
            {
                qbTicket.current_step = ticket.current_step;
            }
            Manager.GetManager().SaveTicket(qbTicket, isCreate);
        }
    }
}
