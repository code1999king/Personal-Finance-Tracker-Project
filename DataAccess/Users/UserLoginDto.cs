

namespace DataAccess.Users
{
    /// <summary>
    /// Contains only required data for login verification.
    /// Note that this class must be used only for user login case.
    /// </summary>
    public class UserLoginDto
    {
        public int UserID { get; }
        public string PasswordHash { get; }
        public UserLoginDto(int userID, string passwordHash)
        {
            UserID = userID;
            PasswordHash = passwordHash;
        }
    }
}
