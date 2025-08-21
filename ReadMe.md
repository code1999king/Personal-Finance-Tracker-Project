![Readme Cover Photo](Docs/General%20Doc%20Images/ReadMeCover.jpg)
# Personal Finance Tracker (PFT)

---

## Overview
**Personal Finance Tracker** is a simple desktop application, 
helps individuals manage and track their balance and financial transactions.

---

## Key Features
- [ ] Security : User password hashing using `BCrypt`.
- [ ] User Authentication : Register and login functionality.
- [ ] Organization : Users can organize their expenses and incomes using categories.
- [ ] Full Management : Users can add, edit, delete, and view expenses and incomes and their categories.
- [ ] Report Generation : Providing statistical insights on financial transactions during a given period.

---

## Tech Stack
`C#`	`.NET Framework`	`ADO.NET`	`SQL Server`	`Windows Forms`.

---

## Project Structure
**Personal Finance Tracker** is built on three tier architecture approach, which is divided into three layers:  

| Layer                     |   Namespace      | Technology Used |
| :---------                |  :-------------  | :------------- |
| Presentation Layer        | Presentation     | Windows Forms   |
| Business Logic Layer      | BusinessLogic    | C# .NET Framework |
| Data Access Layer         | DataAccess       | ADO.NET, SQL Server |

In addition to the three layers:
* **DbScripts folder:** It is a `Class Library Project` contains SQL scripts for database creation and initialization.
* **Docs folder:** It is the place where detailed project documentation exists.


---

## Database Design
The application uses a relational database with four tables, focusing on simplicity and data integrity.  
**Key design highlights:**
* User specific data is supported, by having `UserID` foreign key in tables.
* **Uncategorized transactions** are supported, by keeping both `ExpenseCategoryID` and `IncomeCategoryID` foreign keys nullable.
* The current balance is cached in the `Users` table to optimize performance, and avoid repeated calculations.
* Negative amount -> Transaction is considered as **expense**, and can't be linked to income category.
* Positive amount -> Transaction is considered as **income**, and can't be linked to expense category.  

![Database ERD Diagram](Docs/Database%20Design/ERD.drawio.svg)
For more details about database design, see [Database Documentation](Docs/Database%20Design/Database%20Documentation.md)

---

## Prerequisites
Before running the project, make sure you have the following installed:
- **Windows 10**
- **.NET Framework (v4.7.2 or later)**
- **Visual Studio 2022 (v17.x or later)** with `.NET desktop development` workload
- **SQL Server 2022 Developer Edition (16.x or later)**
- **SQL Server Management Studio (SSMS) (20.2 or later)**
- **NuGet packages:** (restored automatically by Visual Studio)
  - `BCrypt.Net-Next` for password hashing.

---

## Installation Steps
> *Coming soon with V1.0*.

---

## License
Public project created for educational purposes.
