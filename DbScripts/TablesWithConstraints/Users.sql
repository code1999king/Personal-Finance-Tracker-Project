
USE PFT_DB;

-- Users table :
CREATE TABLE Users (
	UserID INT IDENTITY(1,1) NOT NULL,
	Username NVARCHAr(30) NOT NULL,
	PasswordHash NCHAR(60) NOT NULL,
	RegisteredAt DATETIME NOT NULL,
	CurrentBalance DECIMAL(18, 2) NOT NULL,
)
-- Primary key :
ALTER TABLE Users ADD CONSTRAINT PK_Users PRIMARY KEY (UserID);

-- Foreign keys :
ALTER TABLE Users ADD CONSTRAINT UQ_Users_Username UNIQUE (Username);
