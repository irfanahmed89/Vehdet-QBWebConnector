using System.Text.Json.Serialization;

namespace WebApplication.Sample.FrappeModels
{
    public class Customer
    {
        public string territory {  get; set; }
        public string custom_quickbooks_id { get; set; }
        public string custom_quickbooks_edit_sequence { get; set; }
        public bool custom_is_synced_with_quickbooks { get; set; }
        public string customer_name { get; set; }
        public string default_price_list { get; set; }
        public string customer_type { get; set; }
        public string customer_group { get; set; }
        public string customer_primary_address { get; set; }
        public bool disabled { get; set; }
    }

    public class CustomerDTO
    {
        public string name { get; set; }
        public string territory { get; set; }
        public string custom_quickbooks_id { get; set; }
        public string custom_quickbooks_edit_sequence { get; set; }
        public bool custom_is_synced_with_quickbooks { get; set; }
        public string customer_name { get; set; }
        public string default_price_list { get; set; }
        public string customer_type { get; set; }
        public string customer_group { get; set; }
        public string customer_primary_address { get; set; }
        public bool disabled { get; set; }
    }
}
