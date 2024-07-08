using FrappeQbwcService.FrappeModels;
using Microsoft.EntityFrameworkCore;
using QbSync.QbXml.Objects;
using QbSync.WebConnector.Core;
using QbSync.WebConnector.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Sample.Db;
using WebApplication.Sample.Extensions;
using WebApplication.Sample.FrappeModels;

namespace WebApplication.Sample.Application.Steps
{
    public class CustomerQuery
    {
        public const string NAME = "CustomerQuery";
        private const string LAST_MODIFIED_CUSTOMER = "LAST_MODIFIED_CUSTOMER";

        public class Request : StepQueryRequestWithIterator<CustomerQueryRqType>
        {
            public override string Name => NAME;
            private int _maxReturned { get; set; } = 100;
            private readonly ApplicationDbContext dbContext;

            public Request(
                ApplicationDbContext dbContext
            )
            {
                this.dbContext = dbContext;
            }

            protected override async Task<bool> ExecuteRequestAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRqType request)
            {
                // Let's see if we had a previous saved time so we don't re-query the entire QuickBooks every time.
                //var previouslySavedFromModified = (await dbContext.QbSettings
                //    .FirstOrDefaultAsync(m => m.setting_name == LAST_MODIFIED_CUSTOMER))?.value;
                var previouslySavedFromModified = Manager.GetManager().GetSetting(LAST_MODIFIED_CUSTOMER)?.value;
                request.MaxReturned = _maxReturned.ToString();
                request.ActiveStatus = ActiveStatus.All;
                request.FromModifiedDate = DATETIMETYPE.ParseOrDefault(previouslySavedFromModified, DATETIMETYPE.MinValue).GetQueryFromModifiedDate();               
                return await base.ExecuteRequestAsync(authenticatedTicket, request);
            }

            protected override async Task<string?> RetrieveMessageAsync(IAuthenticatedTicket ticket, string key)
            {
                var state = Manager.GetManager().GetKvpState(ticket.ticket, ticket.current_step, key);
                return state?.value;
            }


        }

        public class Response : StepQueryResponseWithIterator<CustomerQueryRsType>
        {
            public override string Name => NAME;

            private readonly ApplicationDbContext dbContext;

            public Response(
                ApplicationDbContext dbContext
            )
            {
                this.dbContext = dbContext;
            }

            protected override async Task ExecuteResponseAsync(IAuthenticatedTicket authenticatedTicket, CustomerQueryRsType response)
            {
                try
                {
                    if (response.CustomerRet != null)
                    {
                        foreach (var customer in response.CustomerRet)
                        {
                            //string? name = Manager.GetManager().GetCustomerByListID(customer.ListID);
                            //if (!string.IsNullOrEmpty(name))
                            //{
                            //   // Manager.GetManager().CreateAndUpdateCustomer(customer, false, name);
                            //}
                            //else
                            //{
                            //   // Manager.GetManager().CreateAndUpdateCustomer(customer, true);
                            //}
                        }

                        var lastFromModifiedDate = response.CustomerRet.OrderBy(m => m.TimeModified).Select(m => m.TimeModified).LastOrDefault();
                        await dbContext.SaveIfNewerAsync(LAST_MODIFIED_CUSTOMER, lastFromModifiedDate);
                    }
                    else
                    {
                        Common.WriteToFile("CustomerQuery -> Info: There is no customer to create in Frappe");
                    }
                }
                catch (Exception ex)
                {
                    Common.WriteToFile("CustomerQuery -> ExecuteRequestAsync Error: " + ex.Message + ", Stack Trace: " + ex.StackTrace);
                }
                await base.ExecuteResponseAsync(authenticatedTicket, response);
            }

            protected override async Task SaveMessageAsync(IAuthenticatedTicket ticket, string key, string value)
            {
                var state = Manager.GetManager().GetKvpState(ticket.ticket, ticket.current_step, key);
                bool isCreate = false;
                if (state == null)
                {
                    state = new QbKvpState
                    {
                        current_step = ticket.current_step,
                        ticket = ticket.ticket,
                        key = key
                    };
                    isCreate = true;
                }

                state.value = value;

                Manager.GetManager().SaveKvpState(state, isCreate);
            }
        }
    }
}