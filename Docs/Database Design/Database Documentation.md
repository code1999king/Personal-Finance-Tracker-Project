# Database Documentation

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

> More documentation will be provided once database scripts are created.