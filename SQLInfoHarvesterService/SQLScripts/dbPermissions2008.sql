DECLARE @DBName SYSNAME;
DECLARE @DBId INT;

DECLARE @DBPermissions TABLE(
	 database_id INT not null
	,permission_name NVARCHAR(128) not null
	,state_desc NVARCHAR(128) not null
	,grantee_principal_id INT not null);

DECLARE DBPermissionsCursor CURSOR
	FORWARD_ONLY FAST_FORWARD READ_ONLY LOCAL
	FOR
		SELECT [databases].[name]
		      ,[databases].[database_id]
		FROM [master].[sys].[databases] 
	OPEN DBPermissionsCursor
	FETCH NEXT FROM DBPermissionsCursor INTO @DBName, @DBId;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @Sql NVARCHAR(MAX);
			SET @Sql = N'SELECT DISTINCT ' 
							+ STR(@DBId) + N' AS [database_id] 
							,[permission_name]
							,[state_desc]
							,[grantee_principal_id] 							
					     FROM ' + QUOTENAME(@DBName) + N'.[sys].[database_permissions] 
						 WHERE [grantee_principal_id] <> 0';
			INSERT @DBPermissions EXEC sp_executesql @Sql;
			FETCH NEXT FROM DBPermissionsCursor INTO @DBName, @DBId;
		END;
CLOSE DBPermissionsCursor;
DEALLOCATE DBPermissionsCursor;

SELECT * FROM @DBPermissions;