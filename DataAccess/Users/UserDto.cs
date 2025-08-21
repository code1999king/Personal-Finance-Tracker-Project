

using System;

namespace DataAccess.Users
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for user information.
    /// It does not contain User's password hash for security reasons.
    /// </summary>
    public class UserDto
    {
        public int UserID { get; }
        public string Username { get; }
        public DateTime RegisteredAt { get; }
        public decimal CurrentBalance { get; }
        public UserDto(int userId, string username, DateTime registeredAt, decimal currentBalance)
        {
            UserID = userId;
            Username = username;
            RegisteredAt = registeredAt;
            CurrentBalance = currentBalance;
        }
    }
}
