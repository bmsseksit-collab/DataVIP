using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DataVipWeb.Models
{
    [Table("bank_accounts")]
    public class BankAccount : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("details")]
        public string DetailsJson { get; set; } = "{}";

        [Column("group_name")]
        public string GroupName { get; set; } = "";

        [Column("bank_name")]
        public string BankName { get; set; } = "";

        [Column("account_type")]
        public string AccountType { get; set; } = "";

        [Column("account_name")]
        public string AccountName { get; set; } = "";

        [Column("owner_name")]
        public string OwnerName { get; set; } = "";

        [Column("account_number")]
        public string AccountNumber { get; set; } = "";

        [Column("branch")]
        public string Branch { get; set; } = "";

        [Column("note")]
        public string Note { get; set; } = "";
    }
}