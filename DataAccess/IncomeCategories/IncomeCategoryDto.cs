

namespace DataAccess.IncomeCategories
{
    public class IncomeCategoryDto
    {
        public int IncomeCategoryID { get; }
        public string CategoryName { get; }
        public int UserID { get; }
        public IncomeCategoryDto(int incomeCategoryID, string categoryName, int userID)
        {
            IncomeCategoryID = incomeCategoryID;
            CategoryName = categoryName;
            UserID = userID;
        }
    }
}
