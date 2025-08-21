

using System.Text;

namespace BusinessLogic.Users
{
    /// <summary>
    /// Contains static helper methods for user data validation.
    /// </summary>
    internal class UserValidator
    {
        /// <summary>
        /// Username must be between 3 and 30 characters long, cannot contain spaces, and cannot be null or whitespace.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static BllError? ValidateUsername(string username)
        {
            bool isValidUsername = !string.IsNullOrWhiteSpace(username) // Not null or whitespace
                && username.Length >= 3 && username.Length <= 30        // Length between 3 and 30 characters
                && !username.Contains(" ");                             // No spaces allowed

            if (isValidUsername) return null;
            else return BllError.InvalidUsername;
        }

        /// <summary>
        /// Password must be at least 8 characters long, cannot be null or empty, and cannot exceed 72 bytes when encoded in UTF-8.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static BllError? ValidateRawPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8) // minimum allowed length is 8 characters
            {
                return BllError.TooShortPassword;
            }

            // Passwords can be hashed if the byte count is not greater than 72 bytes:
            if (Encoding.UTF8.GetByteCount(password) > 72)
            {
                return BllError.TooLongPassword;
            }
            
            return null;
        }
    }
}
