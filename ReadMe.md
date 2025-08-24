![Readme Cover Photo](Docs/General%20Doc%20Images/ReadMeCover.jpg)
# Personal Finance Tracker (PFT)

---

## Overview
**Personal Finance Tracker** is a simple desktop application, 
helps individuals manage and track their balance and financial transactions.

---

## Key Features
- [x] Security : User password hashing using `BCrypt`.
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
* **Strong database constraints** (ensure data integrity at the database level and reduce reliance on BLL validation).
* **Support for uncategorized transactions** (by keeping both `ExpenseCategoryID` and `IncomeCategoryID` foreign keys nullable).
* **Cached current balance** (improves performance, and avoids unnecessary repeated calculations).

**`Transactions` table design clarifications:**
* Negative amount -> Transaction is considered an **expense** -> can not link to an income category (enforced by constraint).
* Positive amount -> Transaction is considered an **income** -> can not link to an expense category (enforced by constraint).

![Database ERD Diagram](Docs/Database%20Design/ERD.drawio.svg)
For more details about database design, see [Database Documentation](Docs/Database%20Design/Database%20Documentation.md).

---

## Data Access Layer Design
**Data Access Layer (DAL)** represents the bridge between the application and database, which contains the required CRUD methods.

**Key features:**
* **Elegant output contract** (Standarized operation results via DalResult class).
* **Comprehensive error handling and logging**.
* **Secure resource management** (automatic disposal of database connections and unmanaged resources).

> *More documentation will be provided during project developement*;

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

## Get Started
Follow these steps to set up and run **Personal Finance Tracker** locally:

1. **Clone the repository**
```bash
git clone https://github.com/code1999king/Personal-Finance-Tracker-Project.git
cd Personal-Finance-Tracker-Project
```
2. **Open the solution**
	* Launch **Visual Studio 2022**.
	* Open `PersonalFinanceTracker.sln`.

3. **Set up the database**
	* Launch **SQL Server Management Studio (SSMS)**.
	* Open a **New Query** tab and run the SQL scripts from the `DbScripts` project to create the database.
	* :warning: **Important**: Follow the correct execution order as detailed in [Database Documentation](Docs/Database%20Design/Database%20Documentation.md).
4. **Configure the database connection string**
	* By default, the connection string in `App.config` expects a local SQL Server instance (`.`) and a database named `PFT_DB`.
	* If your setup differs (e.g., different instance name, database name, or authentication mode), update the connectionStrings section in `App.config` accordingly.
5. **Restore dependencies**
	* Visual Studio will automatically restore NuGet packages (e.g., `BCrypt.Net-Next`) on build.
6. **Build and run the application**
	* Set the **Presentation** project as the startup project.
	* Press **F5** (or click ? Run) to launch the application.

---

## License
Public project created for educational purposes.
