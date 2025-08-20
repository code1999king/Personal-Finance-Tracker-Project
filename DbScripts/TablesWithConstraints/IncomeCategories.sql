
-- IncomeCategories table :
CREATE TABLE IncomeCategories (
	IncomeCategoryID INT IDENTITY(1,1) NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	UserID INT NOT NULL
)

-- Primary key :
ALTER TABLE IncomeCategories ADD CONSTRAINT PK_IncomeCategories PRIMARY KEY (IncomeCategoryID)

-- Foreign keys:
ALTER TABLE IncomeCategories ADD CONSTRAINT FK_IncomeCategories_Users FOREIGN KEY (UserID) REFERENCES Users(UserID)

-- Unique constraints :

-- unique category names per user 
ALTER TABLE IncomeCategories ADD CONSTRAINT UQ_IncomeCategories_CategoryName_UserID UNIQUE (CategoryName, UserID)