DECLARE @DBName SYSNAME;
DECLARE @DBId INT;

DECLARE @DBRoles TABLE(
	 database_id INT NOT NULL
	,principal_id INT NOT NULL
	,name NVARCHAR(128) NOT NULL
	,type_desc NVARCHAR(128) NOT NULL
	,member_principal_id INT NOT NULL);

DECLARE DBRolesCursor CURSOR
	FORWARD_ONLY FAST_FORWARD READ_ONLY LOCAL
	FOR
		SELECT [databases].[name]
		      ,[databases].[database_id]
		FROM [master].[sys].[databases] 
	OPEN DBRolesCursor
	FETCH NEXT FROM DBRolesCursor INTO @DBName, @DBId;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @Sql NVARCHAR(MAX);
			SET @Sql =  N'SELECT ' + STR(@DBId) + N' AS [database_id]
								  ,[db_roles].[principal_id]
								  ,[db_roles].[name]	  
								  ,[db_roles].[type_desc]
								  ,[db_role_members].[member_principal_id]
						  FROM ' + QUOTENAME(@DBName) + N'.[sys].[database_role_members] AS [db_role_members]
						  JOIN
								(SELECT [name] 
									   ,[principal_id] 
									   ,[type_desc] 
								 FROM ' + QUOTENAME(@DBName) + N'.[sys].[database_principals] 
								 WHERE [type] = ''R'') AS [db_roles]
						  ON [db_role_members].[role_principal_id] = [db_roles].[principal_id]';
			INSERT @DBRoles EXEC sp_executesql @Sql;
			FETCH NEXT FROM DBRolesCursor INTO @DBName, @DBId;
		END;
CLOSE DBRolesCursor;
DEALLOCATE DBRolesCursor;

SELECT * FROM @DBRoles;