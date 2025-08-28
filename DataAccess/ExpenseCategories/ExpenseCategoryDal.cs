using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccess.ExpenseCategories
{
    /// <summary>
    /// Provides methods to access expense category data in the database.
    /// </summary>
    public class ExpenseCategoryDal
    {
        /// <summary>
        /// Retrieves expense category
        /// </summary>
        /// <param name="expenseCategoryID"></param>
        /// <returns><see cref="DalResult{T}"/> object contains <see cref="ExpenseCategoryDto"/> object</returns>
        public static DalResult<ExpenseCategoryDto> GetCategoryByID(int expenseCategoryID)
        {
            string query = @"SELECT ExpenseCategoryID, CategoryName, UserID FROM ExpenseCategories WHERE ExpenseCategoryID = @ExpenseCategoryID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ExpenseCategoryID", expenseCategoryID);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string categoryName = (string)reader["CategoryName"];
                                int userID = (int)reader["UserID"];
                                ExpenseCategoryDto categoryDto = new ExpenseCategoryDto(expenseCategoryID, categoryName, userID);
                                return DalResult<ExpenseCategoryDto>.Success(categoryDto);
                            }
                            else
                            {
                                return DalResult<ExpenseCategoryDto>.Failure(DalError.NotFound);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"ExpenseCategoryID = {expenseCategoryID}");
                        return DalResult<ExpenseCategoryDto>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all expense categories linked to the given UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns><see cref="DalResult{T}"/> object contains a list of <see cref="ExpenseCategoryDto"/> objects</returns>
        public static DalResult<List<ExpenseCategoryDto>> GetAllCategoriesByUserID(int userID)
        {
            string query = @"SELECT ExpenseCategoryID, CategoryName, UserID FROM ExpenseCategories WHERE UserID = @UserID;";
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
                            List<ExpenseCategoryDto> categoriesList = new List<ExpenseCategoryDto>();
                            while (reader.Read())
                            {
                                int expenseCategoryID = (int)reader["ExpenseCategoryID"];
                                string categoryName = (string)reader["CategoryName"];
                                ExpenseCategoryDto categoryDto = new ExpenseCategoryDto(expenseCategoryID, categoryName, userID);
                                categoriesList.Add(categoryDto);
                            }
                            return DalResult<List<ExpenseCategoryDto>>.Success(categoriesList);
                        }
                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"While getting expense category list for UserID = {userID}");
                        return DalResult<List<ExpenseCategoryDto>>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new expense category to the database
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns><see cref="DalResult{T}"/> object contains the new ID of the expense category</returns>
        public static DalResult<int> AddNewCategory(ExpenseCategoryDto categoryDto)
        {
            string query = @"INSERT INTO ExpenseCategories(CategoryName, UserID)
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
        /// Updates expense category name
        /// </summary>
        /// <param name="expenseCategoryID"></param>
        /// <param name="newCategoryName"></param>
        /// <returns><see cref="DalResult{T}"/> object contains boolean indicates to updating success</returns>
        public static DalResult<bool> UpdateCategoryNameByID(int expenseCategoryID, string newCategoryName)
        {
            string query = @"UPDATE ExpenseCategories SET CategoryName = @CategoryName
                            WHERE CategoryID = @CategoryID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", newCategoryName);
                    cmd.Parameters.AddWithValue("@CategoryID", expenseCategoryID);
                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0) // affected rows count greater than zero
                            return DalResult<bool>.Success(true);
                        else
                        {
                            DalLogger.Log(null, $"Failed to update category name with CategoryID = {expenseCategoryID}");
                            return DalResult<bool>.Failure(DalError.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"CategoryID = {expenseCategoryID}");
                        return DalResult<bool>.Failure(DalError.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes expense category from database
        /// </summary>
        /// <param name="expenseCategoryID"></param>
        /// <returns><see cref="DalResult{T}"/> object contains boolean indicates to deletion success</returns>
        public static DalResult<bool> DeleteCategoryByID(int expenseCategoryID)
        {
            string query = @"DELETE FROM ExpenseCategories
                            WHERE CategoryID = @CategoryID;";
            using (SqlConnection conn = new SqlConnection(DalSettings.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", expenseCategoryID);
                    try
                    {
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0) // affected rows count greater than zero
                            return DalResult<bool>.Success(true);
                        else
                        {
                            DalLogger.Log(null, $"Failed to delete category with CategoryID = {expenseCategoryID}");
                            return DalResult<bool>.Failure(DalError.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DalLogger.Log(ex, $"CategoryID = {expenseCategoryID}");
                        return DalResult<bool>.Failure(DalError.Error);
                    }
                }
            }
        }
    }
}
