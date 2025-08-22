using BCrypt.Net;

namespace BusinessLogic.Users
{
    /// <summary>
    /// Responsible for hashing and verifying passwords using BCrypt algorithm.
    /// </summary>
    internal class PasswordHasher
    {
        /// <summary>
        /// Hashes the given raw password using BCrypt algorithm.
        /// </summary>
        /// <param name="rawPassword">The raw password to hash.</param>
        /// <returns>The hashed password.</returns>
        public static string HashPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }

        /// <summary>
        /// Verifies that the given raw password matches the hashed password.
        /// </summary>
        /// <param name="rawPassword">The raw password to verify.</param>
        /// <param name="passwordHash">The hashed password to compare against.</param>
        /// <returns>True if the passwords match, false otherwise.</returns>
        public static bool VerifyPassword(string rawPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, passwordHash);
        }
    }
}
