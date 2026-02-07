using BCrypt.Net;

namespace CodeDungeon.Helpers 
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public static bool VerifyPassword(string password, string hash) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}