:ON ERROR EXIT
USE GestionIntegralDB;
GO

-- Run in SQLCMD mode with -v SuperAdminClaveHash=<BCrypt hash generated outside SQL>.
-- This changes only the initial account while its stored value is still the plaintext seed.
DECLARE @SuperAdminClaveHash NVARCHAR(500) = N'$(SuperAdminClaveHash)';
PRINT 'SuperAdminClaveHash: ' + CAST(LEN(@SuperAdminClaveHash) AS NVARCHAR(10));	
IF LEN(@SuperAdminClaveHash) <> 60
   OR LEFT(@SuperAdminClaveHash, 4) NOT IN (N'$2a$', N'$2b$', N'$2y$')
	THROW 50001, 'SuperAdminClaveHash must be a BCrypt hash.', 1;

BEGIN TRY
	BEGIN TRANSACTION;

	UPDATE dbo.Usuarios
	SET ClaveHash = @SuperAdminClaveHash
	WHERE Usuario = N'SuperAdmin'
	  AND (LEN(ClaveHash) <> 60 OR LEFT(ClaveHash, 4) NOT IN (N'$2a$', N'$2b$', N'$2y$'));

	IF @@ROWCOUNT <> 1
		THROW 50002, 'SuperAdmin plaintext seed was not found; no password was changed.', 1;

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;
END CATCH;
