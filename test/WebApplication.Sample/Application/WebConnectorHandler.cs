using FrappeQbwcService.FrappeModels;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Application
{
    public class WebConnectorHandler : WebConnectorHandlerNoop
    {
        private readonly ApplicationDbContext dbContext;

        public WebConnectorHandler(
            ApplicationDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public override async Task CloseConnectionAsync(IAuthenticatedTicket? authenticatedTicket)
        {
            //Delete Ticket and KvpStates //irfan delete here..
            if (authenticatedTicket != null)
            {
                // We do some clean up.
                //var savedStates = dbContext.QbKvpStates
                //    .Where(m => m.Ticket == authenticatedTicket.Ticket)
                //    .ToList();

                //dbContext.QbKvpStates.RemoveRange(savedStates);

                //var savedTickets = dbContext.QbTickets
                //    .Where(m => m.Ticket == authenticatedTicket.Ticket)
                //    .ToList();
                try
                {
                    var savedStates = Manager.GetManager().GetKvpStates(authenticatedTicket.ticket);
                    foreach (QbKvpState state in savedStates)
                    {
                        Manager.GetManager().DeleteState(state.name);
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToFile("Error in DeleteState : " + ex.Message + Environment.NewLine + ex.StackTrace);
                }


                try
                {
                    var savedTickets = Manager.GetManager().GetTickets(authenticatedTicket.ticket);
                    foreach (QbTicket ticket in savedTickets)
                    {
                        Manager.GetManager().DeleteTicket(ticket.name);
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToFile("Error in DeleteTicket : " + ex.Message + Environment.NewLine + ex.StackTrace);
                }


                //dbContext.QbTickets.RemoveRange(savedTickets);

                //await dbContext.SaveChangesAsync();
            }
        }
    }
}
