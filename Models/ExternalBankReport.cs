using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DataVipWeb.Models
{
    [Table("external_bank_reports")]
    public class ExternalBankReport : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("site_id")]
        public Guid SiteId { get; set; }

        [Column("report_date")]
        public DateTime ReportDate { get; set; }

        [Column("group_name")]
        public string GroupName { get; set; } = "";

        [Column("bank_name")]
        public string BankName { get; set; } = "";

        [Column("account_name")]
        public string AccountName { get; set; } = "";

        [Column("account_no")]
        public string AccountNo { get; set; } = "";

        [Column("limit_amount")]
        public decimal LimitAmount { get; set; }

        [Column("transaction_count")]
        public int TransactionCount { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}