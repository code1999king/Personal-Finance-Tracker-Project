
USE PFT_DB;

-- Test data for users

-- Note : The raw password for the following user is maher12345
INSERT INTO Users(Username, PasswordHash, RegisteredAt, CurrentBalance)
			VALUES('maher12345', '$2a$11$gm/y58xO0vbncQr074n77eo6ITwlFa/rLgVTVqdgM4E91bSsray0K', GetDate(), 0);