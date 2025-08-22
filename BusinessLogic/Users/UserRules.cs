

using System.Text;
using DataAccess;
using DataAccess.Users;

namespace BusinessLogic.Users
{
    /// <summary>
    /// Contains static helper methods for user validation/business rules.
    /// </summary>
    internal class UserRules
    {
        /// <summary>
        /// Username must be between 3 and 30 characters long, cannot contain spaces, and cannot be null or whitespace.
        /// </summary>
        /// <param name="username"></param>
        /// <returns><see cref="BllError"/> object contains error code, or null if validation was successfull</returns>
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
        /// <returns><see cref="BllError"/> object contains error code, or null if validation was successfull</returns>
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

        /// <summary>
        /// Username and password must be in correct format.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="rawPassword"></param>
        /// <returns><see cref="BllError"/> object contains error code, or null if validation was successfull</returns>
        public static BllError? ValidateLoginCredentials(string username, string rawPassword)
        {
            return ValidateUsername(username)
                ?? ValidateRawPassword(rawPassword);
        }

        /// <summary>
        /// Username musn't be reserved by other user in the system.
        /// </summary>
        /// <param name="username"></param>
        /// <returns><see cref="BllError"/> object contains error code, or null if validation was successfull</returns>
        public static BllError? ValidateUsernameNotReserved(string username)
        {
            DalResult<bool> existsRes = UserDal.ExistsByUsername(username);
            if (!existsRes.IsSuccess)
                return BllError.Error;
            if (existsRes.Value) // username exists
                return BllError.UsernameReserved;
            return null;
        }

        /// <summary>
        /// Username and password must be in correct format, and username mustn't be reserved by other user in the system.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="rawPassword"></param>
        /// <returns></returns>
        public static BllError? ValidateRegisterRules(string username, string rawPassword)
        {
            return ValidateUsername(username)
                ?? ValidateRawPassword(rawPassword)
                ?? ValidateUsernameNotReserved(username);
        }
    }
}
