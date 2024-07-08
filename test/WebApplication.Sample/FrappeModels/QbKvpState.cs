namespace FrappeQbwcService.FrappeModels
{
    public class QbKvpState
    {
        public string name { get; set; }
        public string ticket { get; set; } = default!;
        public string key { get; set; } = default!;
        public string? value { get; set; }
        public string? current_step { get; set; }
    }
}
