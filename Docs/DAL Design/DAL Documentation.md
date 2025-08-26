# Data Access Layer (DAL) Documentation

**Namespace:** `DataAccess`.

---

## Overview
**Data Access Layer (DAL)** contains the code that is responsible for establishing database connection and performing all required CRUD operations on database tables, 
with exception handling, error logging, consistent result objects.  

**Data Access Layer (DAL)** contains two main parts:
* **Core Components** reside directly under `DataAccess` namespace, and provide services needed by the overall DAL process.
* **Table Specific Components** reside in sub-namespaces, each one of them contains classes related to a specific table. 

---

## Core DAL Components
These classes reside directly under the namespace `DataAccess`.  
These classes are not tied to a specific database table but are essential to the overall DAL operation, by providing services like configuration, logging, standerdized results, and error handling.  
* **DalSettings:** Holds configuration and settings for DAL. Settings are exposed as properties for easy access. Note that this class is for internal DAL usage only. [See class implementation](../../DataAccess/DalSettings.cs).
* **DalResult:** Defines a standard result object returned from DAL operations. It carries execution details such as success status, returned data, and error code. [See class implementation](../../DataAccess/DalResult.cs).
* **DalLogger:** Responsible for recording logs related to DAL execution. Note that this class is for internal DAL usage only. [See class implementation](../../DataAccess/DalLogger.cs).
* **DalError (Enum):** Defines standardized error codes that can be returned in DalResult when an operation fails. [See enum implementation](../../DataAccess/DalError.cs).

---

## Table Specific Components
**Table Specific Components** are organized into sub-namespaces under the root namespace `DataAccess` to keep the codebase clean, structured, and maintainable.  
Each sub-namespace corresponds to a database table and contains all related classes for that table.  
Examples:
* `DataAccess.Users` -> Contains CRUD operations (`UserDal`) and DTOs (`UserDto`, `UserLoginDto`) related to the `Users` table.
* `DataAccess.Transactions` -> Contains CRUD operations (`TransactionDal`) and DTOs (`TransactionDto`) related to the `Transactions` table.
* You can follow the same convention for the other tables.

---

# Naming Conventions
* **Core classes:** Named as `Dal<Purpose>`.
	* `DalSettings` Contains DAL settings.
	* `DalResult` Represents DAL result definition.

* **DAL classes:** Named as `<EntityName>Dal`. Contains CRUD operations for the corresponding table.
	* `UserDal` -> CRUD operations for the `Users` table.
	* `TransactionDal` -> CRUD operations for the `Transactions` table.

* **DTO classes:** Named as `<EntityName>Dto`. For specialized cases, a suffix is added to `<EntityName>` to indicate purpose.  
	* `UserDto` -> Carries general user data.
	* `UserLoginDto` -> Carries login-specific user data.
	* `TransactionDto` -> Carries transaction data.

