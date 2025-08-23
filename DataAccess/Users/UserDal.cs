using System;
using System.Data.SqlClient;

namespace DataAccess.Users
{
    /// <summary>
    /// Provides methods to access user data in the database.
    /// </summary>
    public class UserDal
    {
        /// <summary>
        /// Retrieves user information by UserID.
        /// </summary>
        /// <param name="userID">ID of the requested user</param>
        /// <returns><see cref="DalResult{T}"/> object contains <see cref="UserDto"/> object</returns>
        public static DalResult<UserDto> GetUserByID(int userID)
        {
            string query = @"SELECT UserID, Username, RegisteredAt, CurrentBalance FROM Users WHERE UserID = @UserID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = (string)reader["Username"];
                                DateTime registeredAt = (DateTime)reader["RegisteredAt"];
                                decimal currentBalance = (decimal)reader["CurrentBalance"];
                                UserDto user = new UserDto(userID, username, registeredAt, currentBalance);
                                return DalResult<UserDto>.Success(user);
                            }
                            else
                            {
                                return DalResult<UserDto>.Failure(DalError.NotFound);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"UserID = {userID}");
                        return DalResult<UserDto>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">User's secure info</param>
        /// <param name="passwordHash">User's password after being hashed</param>
        /// <returns><see cref="DalResult{T}"/> object contains the new ID of the user</returns>
        public static DalResult<int> AddNewUser(UserDto user, string passwordHash)
        {
            string query = @"INSERT INTO Users(Username, PasswordHash, RegisteredAt, CurrentBalance)
                            VALUES(@Username, @PasswordHash, @RegisteredAt, @CurrentBalance);
                            SELECT SCOPE_IDENTITY();";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@RegisteredAt", user.RegisteredAt);
                    cmd.Parameters.AddWithValue("@CurrentBalance", user.CurrentBalance);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        //Result must be an integer 
                        if(result != null && int.TryParse(result.ToString(), out int newUserID))
                            return DalResult<int>.Success(newUserID);
                        else
                        {
                            DalLogger.Log(null, $"Failed to retrieve new ID of the user with Username = {user.Username}");
                            return DalResult<int>.Failure(DalError.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"Username = {user.Username}");
                        return DalResult<int>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves user's login data (UserID, PasswordHash) for login verification.
        /// </summary>
        /// <param name="username">User's name</param>
        /// <returns><see cref="DalResult{T}"/>object contains <see cref="UserLoginDto"/> object</returns>
        public static DalResult<UserLoginDto> GetUserLoginByUsername(string username)
        {
            string query = @"SELECT UserID, PasswordHash FROM Users 
                             WHERE Username = @Username";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                int userID = (int)reader["UserID"];
                                string passwordHash = (string)reader["PasswordHash"];
                                UserLoginDto userLoginDto = new UserLoginDto(userID, passwordHash);
                                return DalResult<UserLoginDto>.Success(userLoginDto);
                            }
                            else
                            {
                                return DalResult<UserLoginDto>.Failure(DalError.NotFound);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"Username = {username}");
                        return DalResult<UserLoginDto>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Checks user's existence by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns><see cref="DalResult{T}"/> object contains boolean value represents user existence</returns>
        public static DalResult<bool> ExistsByUsername(string username)
        {
            string query = @"SELECT 1 FROM Users WHERE Username = @Username;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return DalResult<bool>.Success(reader.HasRows);
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"Username = {username}");
                        return DalResult<bool>.Failure(DalError.Error);
                    }
                }
            }
        }
    }
}
