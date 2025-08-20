
USE PFT_DB;

-- ExpenseCategories table :
CREATE TABLE ExpenseCategories (
	ExpenseCategoryID INT IDENTITY(1,1) NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	UserID INT NOT NULL
)

-- Primary key :
ALTER TABLE ExpenseCategories ADD CONSTRAINT PK_ExpenseCategories PRIMARY KEY (ExpenseCategoryID)

-- Foreign keys:
ALTER TABLE ExpenseCategories ADD CONSTRAINT FK_ExpenseCategories_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)

-- Unique category names per user :
ALTER TABLE expenseCategories ADD CONSTRAINT UQ_ExpenseCategories_CategoryName_UserID UNIQUE (CategoryName, UserID)