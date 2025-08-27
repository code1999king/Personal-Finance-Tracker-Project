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
        /// See <see cref="BllSaveMode"/> for more details."/>
        /// </summary>
        private BllSaveMode _SaveMode { get; set; }

        /// <summary>
        /// User's identifier.
        /// </summary>
        public int UserID { get; private set; }

        /// <summary>
        /// User's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's registration date.
        /// </summary>
        public DateTime RegisteredAt { get; private set; }

        /// <summary>
        /// User's current balance.
        /// </summary>
        public decimal CurrentBalance { get; private set; }

        /// <summary>
        /// Creates empty user object, allows you to fill it later.
        /// </summary>
        public User()
        {
            _SaveMode = BllSaveMode.AddNew;
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
            _SaveMode = BllSaveMode.Update;
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
            string additionalLogInfo = $"Username = {username}, RawPassword = {rawPassword}";
            // Retrieve user login information from DAL:
            DalResult<UserLoginDto> loginRes = UserDal.GetUserLoginByUsername(username);
            if (!loginRes.IsSuccess)
            {
                if (loginRes.Error == DalError.NotFound)
                    return BllResult<int>.Failure(BllError.WrongUsernameOrPassword).WithLogging(additionalLogInfo);
                return BllResult<int>.Failure(BllError.Error).WithLogging(additionalLogInfo);
            }

            // Verify user password:
            if (!PasswordHasher.VerifyPassword(rawPassword, loginRes.Value.PasswordHash))
                return BllResult<int>.Failure(BllError.WrongUsernameOrPassword).WithLogging(additionalLogInfo);

            // Authintaction successfull:
            return BllResult<int>.Success(loginRes.Value.UserID).WithLogging(additionalLogInfo);
        }

        /// <summary>
        /// Creates User domain object, where it takes user ID from userID parameter and other information from UserDto object parameter.
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
        /// Creates User domain object
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        private static User _MapToUser(UserDto userDto)
        {
            return new User(
                userID: userDto.UserID,
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
            string additionalLogInfo = $"UserID = {userID}";
            // Validate user identifier:
            if (userID < 1)
                return BllResult<User>.Failure(BllError.InvalidUserID).WithLogging(additionalLogInfo);

            // Retrieve user information:
            DalResult<UserDto> getRes = UserDal.GetUserByID(userID);
            if(!getRes.IsSuccess)
            {
                if (getRes.Error == DalError.NotFound)
                    return BllResult<User>.Failure(BllError.UserNotFound).WithLogging(additionalLogInfo);
                return BllResult<User>.Failure(BllError.Error).WithLogging(additionalLogInfo);
            }

            // Create user object from Dto and return it:
            return BllResult<User>.Success(_MapToUser(getRes.Value)).WithLogging(additionalLogInfo);
        }

        /// <summary>
        /// Authentidcates user by username and password.
        /// </summary>
        /// <param name="username">Username entered by user</param>
        /// <param name="rawPassword">Password entered by user</param>
        /// <returns><see cref="BllResult{T}"/> object contains <see cref="User"/> object</returns>
        public static BllResult<User> Login(string username, string rawPassword)
        {
            string additionalLogInfo = $"Username = {username}, RawPassword = {rawPassword}";
            // Validate login credentials:
            BllError? validationError = UserRules.ValidateLoginCredentials(username, rawPassword);
            if (validationError.HasValue) 
                return BllResult<User>.Failure(validationError.Value).WithLogging(additionalLogInfo);

            // Authenticate user:
            BllResult<int> authRes = _Authenticate(username, rawPassword);
            if (!authRes.IsSuccess)
                return BllResult<User>.Failure(authRes.Error).WithLogging(additionalLogInfo);

            // Retrieve safe user iformation from DAL:
            int userID = authRes.Value; // for clarity
            BllResult<User> getRes = Find(userID);
            if (!getRes.IsSuccess) 
                return BllResult<User>.Failure(BllError.Error).WithLogging(additionalLogInfo); // can't be not found!!

            return BllResult<User>.Success(getRes.Value).WithLogging(additionalLogInfo);

        }

        /// <summary>
        /// Registers a new user in the system with username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="rawPassword"></param>
        /// <returns><see cref="BllResult{T}"/> object contains <see cref="User"/> object</returns>
        public static BllResult<User> Register(string username, string rawPassword)
        {
            string additionalLogInfo = $"Username = {username}, RawPassword = {rawPassword}";
            // Ensure valid login credentials:
            BllError? validationError = UserRules.ValidateRegisterRules(username, rawPassword);
            if (validationError.HasValue)
                return BllResult<User>.Failure(validationError.Value).WithLogging(additionalLogInfo);

            // Prepare new user info:
            UserDto userDto = _CreateUserDtoForRegistration(username);
            string passwordHash = PasswordHasher.HashPassword(rawPassword);

            // Add New user to database:
            DalResult<int> addRes = UserDal.AddNewUser(userDto, passwordHash);
            if (!addRes.IsSuccess)
                return BllResult<User>.Failure(BllError.Error).WithLogging(additionalLogInfo);

            // return User object with safe info:
            return BllResult<User>.Success(_MapToUser(addRes.Value, userDto)).WithLogging(additionalLogInfo);
        }
    }
}
