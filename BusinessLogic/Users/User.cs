using System;

namespace BusinessLogic.Users
{
    /// <summary>
    /// Represents user's domain model.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's save mode.
        /// See <see cref="SaveMode"/> for more details."/>
        /// </summary>
        private SaveMode _SaveMode { get; set; }

        /// <summary>
        /// User's identifier.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// User's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's registration date.
        /// </summary>
        public DateTime RegisteredAt { get; set; }

        /// <summary>
        /// User's current balance.
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// Creates empty user object, allows you to fill it later.
        /// </summary>
        public User()
        {
            _SaveMode = SaveMode.AddNew;
            UserID = -1;
            Username = "";
            RegisteredAt = DateTime.Now;
            CurrentBalance = 0;
        }

        /// <summary>
        /// Creates user object with predefined values came from database. Private access modifier prevents creating a full user object from outside this class.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="username"></param>
        /// <param name="registeredAt"></param>
        /// <param name="currentBalance"></param>
        private User(int userID, string username, DateTime registeredAt, decimal currentBalance)
        {
            _SaveMode = SaveMode.Update;
            UserID = userID;
            Username = username;
            RegisteredAt = registeredAt;
            CurrentBalance = currentBalance;
        }
    }
}
