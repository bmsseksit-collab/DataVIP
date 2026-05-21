using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DataVipWeb.Models
{
    [Table("bank_groups")]
    public class BankGroup : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("group_name")]
        public string GroupName { get; set; } = "";
    }
}