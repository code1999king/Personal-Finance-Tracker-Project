using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccess.IncomeCategories
{
    /// <summary>
    /// Provides methods to access income category data in the database.
    /// </summary>
    public class IncomeCategoryDal
    {
        /// <summary>
        /// Retrieves income category
        /// </summary>
        /// <param name="expenseCategoryID"></param>
        /// <returns><see cref="DalResult{T}"/> object contains <see cref="ExpenseCategoryDto"/> object</returns>
        public static DalResult<IncomeCategoryDto> GetCategoryByID(int IncomeCategoryID)
        {
            string query = @"SELECT IncomeCategoryID, CategoryName, UserID FROM IncomeCategories WHERE IncomeCategoryID = @IncomeCategoryID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IncomeCategoryID", IncomeCategoryID);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string categoryName = (string)reader["CategoryName"];
                                int userID = (int)reader["UserID"];
                                IncomeCategoryDto categoryDto = new IncomeCategoryDto(IncomeCategoryID, categoryName, userID);
                                return DalResult<IncomeCategoryDto>.Success(categoryDto);
                            }
                            else
                            {
                                return DalResult<IncomeCategoryDto>.Failure(DalError.NotFound);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"IncomeCategoryID = {IncomeCategoryID}");
                        return DalResult<IncomeCategoryDto>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all income categories linked to the given UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns><see cref="DalResult{T}"/> object contains a list of <see cref="IncomeCategoryDto"/> objects</returns>
        public static DalResult<List<IncomeCategoryDto>> GetAllCategoriesByUserID(int userID)
        {
            string query = @"SELECT IncomeCategoryID, CategoryName, UserID FROM IncomeCategories WHERE UserID = @UserID;";
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
                            List<IncomeCategoryDto> categoriesList = new List<IncomeCategoryDto>();
                            while (reader.Read())
                            {
                                int IncomeCategoryID = (int)reader["IncomeCategoryID"];
                                string categoryName = (string)reader["CategoryName"];
                                IncomeCategoryDto categoryDto = new IncomeCategoryDto(IncomeCategoryID, categoryName, userID);
                                categoriesList.Add(categoryDto);
                            }
                            return DalResult<List<IncomeCategoryDto>>.Success(categoriesList);
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"While getting Income category list for UserID = {userID}");
                        return DalResult<List<IncomeCategoryDto>>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new income category to the database
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns><see cref="DalResult{T}"/> object contains the new ID of the Income category</returns>
        public static DalResult<int> AddNewCategory(IncomeCategoryDto categoryDto)
        {
            string query = @"INSERT INTO IncomeCategories(CategoryName, UserID)
                            VALUES(@CategoryName, @UserID);
                            SELECT SCOPE_IDENTITY();";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", categoryDto.CategoryName);
                    cmd.Parameters.AddWithValue("@UserID", categoryDto.UserID);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        //Result must be an integer 
                        if (result != null && int.TryParse(result.ToString(), out int newCategoryID))
                            return DalResult<int>.Success(newCategoryID);
                        else
                        {
                            DalLogger.Log(null, $"Failed to retrieve new ID of the category with CategoryName = {categoryDto.CategoryName}");
                            return DalResult<int>.Failure(DalError.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"CategoryName = {categoryDto.CategoryName}");
                        return DalResult<int>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Updates income category name
        /// </summary>
        /// <param name="IncomeCategoryID"></param>
        /// <param name="newCategoryName"></param>
        /// <returns><see cref="DalResult{T}"/> object contains boolean indicates to updating success</returns>
        public static DalResult<bool> UpdateCategoryNameByID(int IncomeCategoryID, string newCategoryName)
        {
            string query = @"UPDATE IncomeCategories SET CategoryName = @CategoryName
                            WHERE CategoryID = @CategoryID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", newCategoryName);
                    cmd.Parameters.AddWithValue("@CategoryID", IncomeCategoryID);
                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0) // affected rows count greater than zero
                            return DalResult<bool>.Success(true);
                        else
                        {
                            DalLogger.Log(null, $"Failed to update category name with CategoryID = {IncomeCategoryID}");
                            return DalResult<bool>.Failure(DalError.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"CategoryID = {IncomeCategoryID}");
                        return DalResult<bool>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes income category from database
        /// </summary>
        /// <param name="IncomeCategoryID"></param>
        /// <returns><see cref="DalResult{T}"/> object contains boolean indicates to deletion success</returns>
        public static DalResult<bool> DeleteCategoryByID(int IncomeCategoryID)
        {
            string query = @"DELETE FROM IncomeCategories
                            WHERE CategoryID = @CategoryID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", IncomeCategoryID);
                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0) // affected rows count greater than zero
                            return DalResult<bool>.Success(true);
                        else
                        {
                            DalLogger.Log(null, $"Failed to delete category with CategoryID = {IncomeCategoryID}");
                            return DalResult<bool>.Failure(DalError.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"CategoryID = {IncomeCategoryID}");
                        return DalResult<bool>.Failure(DalError.Error);
                    }
                }
            }
        }
    }
}
