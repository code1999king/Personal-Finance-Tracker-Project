# Data Access Layer (DAL) Documentation

**Namespace:** `DataAccess`.

---

## Overview
The **Data Access Layer (DAL)** is responsible for connecting to the database, performing CRUD operations, handling errors, logging, and returning standardized result objects.  

It contains two main parts:
* **Core Components** reside in the root `DataAccess` namespace, and provide services needed by the overall DAL process.
* **Table Specific Components** reside in sub-namespaces, each one of them contains classes related to a specific table. 

---

## Core DAL Components
These classes reside in the root `DataAccess` namespace.  
They are not tied to a specific database table, but provide essential services such as configuration, logging, standardized results, and error handling for the entire DAL. 
* **DalSettings:** Holds configuration and settings for DAL. Settings are exposed as properties for easy access. [See class implementation](../../DataAccess/DalSettings.cs).
* **DalResult:** Defines a standard result object returned from DAL operations. It carries execution details such as success status flag, returned data, and error code. [See class implementation](../../DataAccess/DalResult.cs).
* **DalLogger:** Responsible for recording logs related to DAL execution. [See class implementation](../../DataAccess/DalLogger.cs).
* **DalError (Enum):** Defines standardized error codes that can be returned in DalResult when an operation fails. [See enum implementation](../../DataAccess/DalError.cs).

:warning: **Note:** classes `DalSettings`, `DalLogger` are intended for internal DAL usage and are not exposed to higher layers.

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
	* `DalSettings` -> Contains DAL settings.
	* `DalResult` -> Represents DAL result definition.

* **DAL classes:** Named as `<EntityName>Dal`. Contains CRUD operations for the corresponding table.
	* `UserDal` -> CRUD operations for the `Users` table.
	* `TransactionDal` -> CRUD operations for the `Transactions` table.

* **DTO classes:** Named as `<EntityName>Dto`. For specialized cases, a suffix is added to `<EntityName>` to indicate purpose.  
	* `UserDto` -> Carries general user data.
	* `UserLoginDto` -> Carries login-specific user data.
	* `TransactionDto` -> Carries transaction data.

---

## Usage Examples
Checking the result of a DAL operation:
```C#
// Suppose the result is stored in a variable named dalResult:
if(dalResult.IsSuccess) // Always check "IsSuccess" before accessing any other property
	Console.WriteLine("Process succeeded. Data: " + dalResult.Value.ToString());
else
	Console.WriteLine("Process failed. Error: " + dalResult.Error.ToString());
```

Retrieve the general info of user with ID = 10:
```C#
DalResult<UserDto> getRes = UserDal.GetUserByID(10);
if(getRes.IsSuccess)
	// Retrieval succeeded, Access user info by getRes.Value
else
	// Retrieval failed, Handle the error by getRes.Error
```

Add a new user to the database with the given userDto and passwordHash:
```C#
DalResult<int> addRes = UserDal.AddNewUser(userDto, passwordHash);
if(addRes.IsSuccess)
	// Addition succeeded, the new ID is available in addRes.Value
else
	// Addition failed, Handle the error by addRes.Error
```
