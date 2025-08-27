# Business Logic Layer (BLL) Documentation

**Namespace:** `BusinessLogic`.

---

## Overview
The **Business Logic Layer (BLL)** is responsible for data format validation, business rules validation, handling errors, logging, and returning standardized result objects. 

It contains two main parts:
* **Core BLL Components** reside in the root `BusinessLogic` namespace, and provide services needed by the overall BLL process.
* **Entity Specific Components** reside in sub-namespaces, each one of them contains classes related to a specific entity.

___

## Core BLL Components
These classes reside in the root `BusinessLogic` namespace.  
They are not tied to a specific entity, but provide essential services such as logging, standardized results, and error handling, and other needed components for the entire BLL. 
* **BllResult:** Defines a standard result object returned from BLL operations. It carries execution details such as success status flag, returned data, and error code. [See class implementation](../../BusinessLogic/BllResult.cs).
* **BllLogger:** Responsible for recording logs related to BLL execution. [See class implementation](../../BusinessLogic/BllLogger.cs).
* **BllResultExtensions:** Contains extension methods related to BLlResult class. [See class implementation](../../BusinessLogic/BllResultExtenstions.cs).
* **BllError (Enum):** Defines standardized error codes that can be returned in BllResult when an operation fails. [See enum implementation](../../BusinessLogic/BllError.cs).
* **SaveMode (Enum):** Defines save modes (AddNew, Update) to control saving process for entities. [See enum implementation](../../BusinessLogic/SaveMode.cs).

## Entity Specific Conponents
**Entity Specific Components** are organized into sub-namespaces under the root namespace `BusinessLogic` to keep the codebase clean, structured, and maintainable.  
Each sub-namespace corresponds to an entity and contains all related classes for that entity.  
Example:
* `BusinessLogic.Users` -> Contains all user related classes (`User`, `UserRules`, ..).
And all other entities in the system are organized using the same pattern.

---

## Naming Conventions
* **Core classes:** Named as `Bll<Purpose>`.
	* `BllLogger` -> Logs BLL operations.
	* `BllResult` -> Represents BLL result definition.
* **Entity classes:** Named as `<EntityName>`. For specialized cases, a suffix is added to `<EntityName>` to indicate purpose.
	* `User` -> Defines user's object in BLL with all its properties and methods.
	* `UserRules` -> Contains all validation and business rules to be applied in `User` methods.

	---

## Usage Examples
Checking the result of a BLL operation:
```C#
// Suppose the result is stored in a variable named bllResult:
if(bllResult.IsSuccess) // Always check "IsSuccess" before accessing any other property
	Console.WriteLine("Process succeeded. Data: " + bllResult.Value.ToString());
else
	Console.WriteLine("Process failed. Error: " + bllResult.Error.ToString());
```

Login a user with a given username and password:
```C#
BllResult<User> loginRes = User.Login(username, password);
if(loginRes.IsSuccess)
	Console.WriteLine("Login Succeeded"");
	// You can access user info by loginRes.Value
else
	Console.WriteLine("Login Failed");
	// You can access error code by loginRes.Error
```
