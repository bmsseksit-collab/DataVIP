using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DataVipWeb.Models
{
    [Table("external_sites")]
    public class ExternalSite : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("site_code")]
        public string SiteCode { get; set; } = "";

        [Column("site_name")]
        public string SiteName { get; set; } = "";

        [Column("api_url")]
        public string ApiUrl { get; set; } = "";

        [Column("bearer_token")]
        public string BearerToken { get; set; } = "";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}