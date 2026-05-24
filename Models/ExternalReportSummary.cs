using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DataVipWeb.Models
{
    [Table("external_report_summaries")]
    public class ExternalReportSummary : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("site_id")]
        public Guid SiteId { get; set; }

        [Column("report_date")]
        public DateTime ReportDate { get; set; }

        [Column("group_name")]
        public string GroupName { get; set; } = "";

        [Column("summary_count")]
        public int SummaryCount { get; set; }

        [Column("summary_total")]
        public decimal SummaryTotal { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}