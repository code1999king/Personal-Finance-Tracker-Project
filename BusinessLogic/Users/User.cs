using System;
using DataAccess;
using DataAccess.Users;
using BCrypt.Net;

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

        /// <summary>
        /// Authentidcates user by username and password.
        /// </summary>
        /// <param name="username">Username entered by user</param>
        /// <param name="rawPassword">Password entered by user</param>
        /// <returns><see cref="BllResult{T}"/> object contains <see cref="User"/> object</returns>
        public static BllResult<User> Login(string username, string rawPassword)
        {
            // Validate input:
            BllError? validationError = UserValidator.ValidateUsername(username)
                                    ??  UserValidator.ValidateRawPassword(rawPassword);
            if (validationError.HasValue) 
                return BllResult<User>.Failure(validationError.Value);

            // Retrieve user login information from DAL:
            DalResult<UserLoginDto> loginRes = UserDal.GetUserLoginByUsername(username);
            if(!loginRes.IsSuccess)
            {
                if (loginRes.Error == DalError.NotFound)
                    return BllResult<User>.Failure(BllError.WrongUsernameOrPassword);
                return BllResult<User>.Failure(BllError.Error);
            }

            // Verify password:
            if (!BCrypt.Net.BCrypt.Verify(rawPassword, loginRes.Value.PasswordHash))
                return BllResult<User>.Failure(BllError.WrongUsernameOrPassword);

            // Retrieve safe user iformation from DAL:
            DalResult<UserDto> userRes = UserDal.GetUserByID(loginRes.Value.UserID);
            if (!userRes.IsSuccess)
                return BllResult<User>.Failure(BllError.Error); // can't be not found

            // Create user object from DTO:
            UserDto userDto = userRes.Value;
            User user = new User(userDto.UserID, userDto.Username, userDto.RegisteredAt, userDto.CurrentBalance);
            return BllResult<User>.Success(user);

        }
    }
}
