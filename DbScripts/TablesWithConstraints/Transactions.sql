

USE PFT_DB;

-- Transactions table :
CREATE TABLE Transactions (
	TransactionID INT IDENTITY(1,1) NOT NULL,
	Amount DECIMAL(18, 2) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	Notes NVARCHAR(200) NULL,
	ExpenseCategoryID INT NULL,
	IncomeCategoryID INT NULL,
	UserID INT NOT NULL
)

-- Primary key :
ALTER TABLE Transactions ADD CONSTRAINT PK_Transactions PRIMARY KEY (TransactionID);

-- Foreign keys :
ALTER TABLE Transactions ADD CONSTRAINT FK_Transactions_ExpenseCategories FOREIGN KEY (ExpenseCategoryID) REFERENCES ExpenseCategories(ExpenseCategoryID);
ALTER TABLE Transactions ADD CONSTRAINT FK_Transactions_IncomeCategories FOREIGN KEY (IncomeCategoryID) REFERENCES IncomseCategories(IncomeCategoryID);
ALTER TABLE Transactions ADD CONSTRAINT FK_Transactions_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);


-- Check: transaction amount can't be zero
ALTER TABLE Transactions ADD CONSTRAINT CK_Transactions_AmountNotZero CHECK (Amount <> 0);

-- Transaction must be linked to a correct category type :
	-- if Amount > 0 it is income transaction, then ExpenseCategoryID must be null
	-- if Amount < 0 it is expense transaction, then IncomeCategoryID must be null
ALTER TABLE Transactions ADD CONSTRAINT CK_Transactions_CorrectCategoryType CHECK (
	(Amount > 0 AND ExpenseCategoryID IS NULL)
	OR
	(Amount < 0 AND IncomeCategoryID IS NULL)
)