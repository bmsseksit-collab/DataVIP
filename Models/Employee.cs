using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DataVipWeb.Models
{
    [Table("employees")]
    public class Employee : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; } = "";

        [Column("full_name")]
        public string FullName { get; set; } = "";

        [Column("nick_name")]
        public string NickName { get; set; } = "";

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("phone")]
        public string Phone { get; set; } = "";

        [Column("image_url")]
        public string? ImageUrl { get; set; }
    }
}