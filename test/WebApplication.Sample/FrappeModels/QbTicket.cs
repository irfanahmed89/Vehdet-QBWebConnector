using QbSync.WebConnector.Core;

namespace FrappeQbwcService.FrappeModels
{
    public class QbTicket : IAuthenticatedTicket
    {
        public string name { get; set; }
        public string ticket { get; set; } = default!;

        public string? current_step { get; set; }

        public bool authenticated { get; set; }
    }
}
