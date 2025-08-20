# Database Documentation

**Database Name:** `PFT_DB` stands for (Personal Finance Tracker Database)

## Overview
The application uses a relational database with four tables, focusing on simplicity and data integrity, 
to securely store user's data, including their profile info, financial transactions and income/expense categories info.  
![Database ERD Diagram](ERD.drawio.svg)

## Entity Relationships
* Each user has zero or many transactions, income categories, and expense categories.
* Each transaction must be linked to one user and only one.
* Each income/expense category must be linked to one user and only one.
* Each transaction can't belong to multiple categories at the same time.
* Each transaction can only belong to :
	* One income category.
	* One expense category.
	* No category at all (un-categorized).

---

## Important Notes
Please read the following notes before dealing with the database:
* `Users.CurrentBalance` must be **recalculated** after any change in `Transactions` table.
* When **retrieving user's current balance** you should use `Users.CurrentBalance` to reduce calculations and optimize performance.
* The following columns **shouldn't be modified** after creation:
	* `Users.RegisteredAt`
	* `Transactions.CreatedAt`
	* `Transactions.UserID`
	* `ExpenseCategories.UserID`
	* `IncomeCategories.UserID`

---

## Users Table
This table is used to store Users in the system.
[See Users Table Script](../../DbScripts/TablesWithConstraints/Users.sql)

|Column Name              |   Type & Nullability         |   Notes   |
|:-----------:            | :-----------:                | :---------: |
|UserID                   |`INT IDENTITY(1,1)` `NOT NULL`|  User's ID    |
|Username                 |`NVARCHAR(30)` `NOT NULL`     |Accepts strings up to 30 characters long for username|
|PasswordHash             |`NCHAR(60)` `NOT NULL`        |The output length of the chosen hashing algorithm `BCrypt` is 60 characters|
|RegisteredAt             |`DATETIME` `NOT NULL`         |Represents registration timestamp and this value shouldn't be changed|
|CurrentBalance           |`DECIMAL(18,2)` `NOT NULL`    |Cached value, should be used to reduce calculations and optimize performance|

**Constraints:**
* PK_Users: `PRIMARY KEY`.
* UQ_Users_Username: `UNIQUE` - Ensures username uniqueness across users.

---

## ExpenseCategories Table
This table is used to store expense categories in the system.
[See ExpenseCategories Table Script](../../DbScripts/TablesWithConstraints/ExpenseCategories.sql)

|Column Name              |   Type & Nullability         |   Notes   |
|:-----------:            | :-----------:                | :---------: |
|ExpenseCategoryID        |`INT IDENTITY(1,1)` `NOT NULL`| Category's ID|
|CategoryName             |`NVARCHAR(50)` `NOT NULL`     | Accepts strings up to 50 characters long|
|UserID                   |`INT`  `NOT NULL`             | Identifier of the owner user |

**Constraints:**
* PK_ExpenseCategories: `PRIMARY KEY`.
* FK_ExpenseCategories_Users: `FOREIGN KEY` - Ensures the category is linked to a valid user.
* UQ_ExpenseCategories_CategoryName_UserID: `UNIQUE` - Ensures that category names are unique per user.

---

## IncomeCategories Table
This table is used to store Income categories in the system.
[See IncomeCategories Table Script](../../DbScripts/TablesWithConstraints/IncomeCategories.sql)

|Column Name              |   Type & Nullability         |   Notes   |
|:-----------:            | :-----------:                | :---------: |
|IncomeCategoryID         |`INT IDENTITY(1,1)` `NOT NULL`| Category's ID|
|CategoryName             |`NVARCHAR(50)` `NOT NULL`     | Accepts strings up to 50 characters long|
|UserID                   |`INT`  `NOT NULL`             | Identifier of the owner user |

**Constraints:**
* PK_IncomeCategories: `PRIMARY KEY`.
* FK_IncomeCategories_Users: `FOREIGN KEY` - Ensures the category belongs to a valid user.
* UQ_IncomeCategories_CategoryName_UserID: `UNIQUE` - Ensures that category names are unique per user.

---

## Transactions Table:
This table is used to store financial transactions in the system.
[See Transactions Table Script](../../DbScripts/TablesWithConstraints/Transactions.sql)  

|Column Name              |   Type & Nullability         |   Notes   |
|:-----------:            | :-----------:                | :---------: |
|TransactionID            |`INT IDENTITY(1,1)` `NOT NULL`| Transaction's ID|
|Amount                   |`DECIMAL(18,2)` `NOT NULL`    | When Negative -> Expense</br>When Positive -> Income</br>Can't be zero |
|CreatedAt                |`DATETIME` `NOT NULL`         | Represents creation timestamp and this value shouldn't be changed|
|Notes                    |`NVARCHAR(200)` `NULL`        | This field is optional and accepts strings up to 200 characters long|

**Constraints:**
* PK_Transactions: `PRIMARY KEY`.
* FK_Transactions_ExpenseCategories: `FOREIGN KEY` - Ensures the transaction belongs to a valid expense category.
* FK_Transactions_IncomeCategories: `FOREIGN KEY` - Ensures the transaction belongs to a valid income category.
* FK_Transactions_Users: `FOREIGN KEY` - Ensures the transaction belongs to a valid user.
* CK_Transactions_AmountNotZero: `CHECK` - Ensures the `Amount` can never be zero.
* CK_Transactions_CorrectCategoryType: `CHECK` - Ensures the transaction belongs to the correct category and it works as follows :
	* If `Amount < 0` -> Transaction is **Expense** -> Ensures transaction can belong only to expense category.
	* If `Amount > 0` -> Transaction is **Income** -> Ensures transaction can belong only to income category.
	* It does not prevent keeping both `ExpenseCategoryID` and `IncomeCategoryID` Null valued, which results in an uncategorized transaction.

---

## Scripts Execution Order
To run database scripts in correct order, run them as follows :
1. Database.sql
2. TablesWithConstraints/Users.sql
3. TablesWithConstraints/ExpenseCategories.sql
4. TablesWithConstraints/IncomeCategories.sql
5. TablesWithConstraints/Transactions.sql

---

## Usage Examples
```sql
-- Retrieve user info whose ID = 10
SELECT * FROM Users WHERE UserID = 10;
```

```sql
-- Retrieve un-categorized transactions for the user whose ID = 123
SELECT * FROM Transactions 
WHERE UserID = 123 
AND ExpenseCategoryID IS NULL
AND IncomeCategoryID IS NULL;
```

```sql
-- Retrieve expenses that belong to 'Food' category for the user whose ID = 45
SELECT t.Amount, t.CreatedAt, t.Notes
FROM Transactions as t
INNER JOIN ExpenseCategories as x
	ON t.ExpenseCategoryID = x.ExpenseCategoryID
WHERE x.CategoryName = 'Food' AND t.UserID = 45;
```

```sql
-- Add new income category with name 'Freelancing' for the user whose ID = 12
INSERT INTO IncomeCategories(CategoryName, UserID)
VALUES('Freelancing', 12);
```

```sql
-- Update the income with TransactionID = 211 to be under the category with ID = 56
UPDATE Transactions SET IncomeCategoryID = 56
WHERE TransactionID = 211;
```