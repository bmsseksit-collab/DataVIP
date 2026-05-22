namespace DataVipWeb
{
    public static class AuthState
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static string Username { get; set; } = "";
        public static string Role { get; set; } = "";
        public static DateTime? LoginAt { get; set; }
    }
}