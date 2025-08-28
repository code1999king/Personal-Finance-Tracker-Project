

namespace DataAccess.ExpenseCategories
{
    public class ExpenseCategoryDto
    {
        public int ExpenseCategoryID { get; }
        public string CategoryName { get; }
        public int UserID { get; }
        public ExpenseCategoryDto(int expenseCategoryID, string categoryName, int userID)
        {
            ExpenseCategoryID = expenseCategoryID;
            CategoryName = categoryName;
            UserID = userID;
        }
    }
}
