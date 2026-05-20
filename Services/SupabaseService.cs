using Supabase;
using Microsoft.Extensions.Configuration;

namespace DataVipWeb.Services
{
    public class SupabaseService
    {
        public Client Client { get; }

        public SupabaseService(IConfiguration configuration)
        {
            var supabaseUrl = configuration["SupabaseUrl"];
            var supabaseKey = configuration["SupabaseKey"];

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            Client = new Client(supabaseUrl, supabaseKey, options);
        }
    }
}