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
        /// Authenticates user with username and raw password. Please ensure valid username and password inputs before calling this method.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="rawPassword"></param>
        /// <returns><see cref="BllResult{T}"/> object contains authenticated user ID</returns>
        private static BllResult<int> _Authenticate(string username, string rawPassword)
        {
            // Retrieve user login information from DAL:
            DalResult<UserLoginDto> loginRes = UserDal.GetUserLoginByUsername(username);
            if (!loginRes.IsSuccess)
            {
                if (loginRes.Error == DalError.NotFound)
                    return BllResult<int>.Failure(BllError.WrongUsernameOrPassword);
                return BllResult<int>.Failure(BllError.Error);
            }

            // Verify user password:
            if (!BCrypt.Net.BCrypt.Verify(rawPassword, loginRes.Value.PasswordHash))
                return BllResult<int>.Failure(BllError.WrongUsernameOrPassword);

            // Authintaction successfull:
            return BllResult<int>.Success(loginRes.Value.UserID);
        }
        
        /// <summary>
        /// Creates User domain object
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        private static User _MapToUser(int userID, UserDto userDto)
        {
            return new User(
                userID: userID,
                username: userDto.Username,
                registeredAt: userDto.RegisteredAt,
                currentBalance: userDto.CurrentBalance
                );
        }

        /// <summary>
        /// Creates UserDto object as a new user with the given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private static UserDto _CreateUserDtoForRegistration(string username)
        {
            return new UserDto(
                userId: -1, 
                username: username, 
                registeredAt: DateTime.Now, 
                currentBalance: 0);
        }
        

        /// <summary>
        /// Retrieves user's safe information
        /// </summary>
        /// <param name="userID">Identifier of retrieved user</param>
        /// <returns><see cref="BllResult{T}"/> object contains <see cref="User"/> object</returns>
        public static BllResult<User> Find(int userID)
        {
            // Validate user identifier:
            if (userID < 1)
                return BllResult<User>.Failure(BllError.InvalidUserID);

            // Retrieve user information:
            DalResult<UserDto> userRes = UserDal.GetUserByID(userID);
            if(!userRes.IsSuccess)
            {
                if (userRes.Error == DalError.NotFound)
                    return BllResult<User>.Failure(BllError.UserNotFound);
                return BllResult<User>.Failure(BllError.Error);
            }

            // Create user object from Dto:
            UserDto userDto = userRes.Value;
            User user = new User(userDto.UserID, userDto.Username, userDto.RegisteredAt, userDto.CurrentBalance);
            return BllResult<User>.Success(user);
        }

        /// <summary>
        /// Authentidcates user by username and password.
        /// </summary>
        /// <param name="username">Username entered by user</param>
        /// <param name="rawPassword">Password entered by user</param>
        /// <returns><see cref="BllResult{T}"/> object contains <see cref="User"/> object</returns>
        public static BllResult<User> Login(string username, string rawPassword)
        {
            // Validate login credentials:
            BllError? validationError = UserValidator.ValidateLoginCredentials(username, rawPassword);
            if (validationError.HasValue) 
                return BllResult<User>.Failure(validationError.Value);

            // Authenticate user:
            BllResult<int> authRes = _Authenticate(username, rawPassword);
            if (!authRes.IsSuccess)
                return BllResult<User>.Failure(authRes.Error);

            // Retrieve safe user iformation from DAL:
            int userID = authRes.Value; // for clarity
            BllResult<User> userRes = Find(userID);
            if (!userRes.IsSuccess) 
                return BllResult<User>.Failure(BllError.Error); // can't be not found!!

            User user = userRes.Value; // for clarity
            return BllResult<User>.Success(user);

        }

        /// <summary>
        /// Registers a new user in the system with username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="rawPassword"></param>
        /// <returns><see cref="BllResult{T}"/> object contains <see cref="User"/> object</returns>
        public static BllResult<User> Register(string username, string rawPassword)
        {
            // Ensure valid login credentials:
            BllError? validationError = UserValidator.ValidateRegisterRules(username, rawPassword);
            if (validationError.HasValue)
                return BllResult<User>.Failure(validationError.Value);

            // Prepare new user info:
            UserDto userDto = _CreateUserDtoForRegistration(username);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);

            // Add New user to database:
            DalResult<int> addRes = UserDal.AddNewUser(userDto, passwordHash);
            if (!addRes.IsSuccess)
                return BllResult<User>.Failure(BllError.Error);

            // return User object with safe info:
            return BllResult<User>.Success(_MapToUser(addRes.Value, userDto));
        }
    }
}
