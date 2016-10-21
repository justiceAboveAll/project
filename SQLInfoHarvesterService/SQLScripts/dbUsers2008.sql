DECLARE @DBName SYSNAME;
DECLARE @DBId INT;

DECLARE @DBUsers TABLE(
	 database_id INT not null
	,principal_id INT not null
	,name NVARCHAR(128) not null
	,type_desc NVARCHAR(128) not null
	);

DECLARE DBUsersCursor CURSOR
	FORWARD_ONLY FAST_FORWARD READ_ONLY LOCAL
	FOR
		SELECT [databases].[name]
		      ,[databases].[database_id]
		FROM [master].[sys].[databases] 
	OPEN DBUsersCursor
	FETCH NEXT FROM DBUsersCursor INTO @DBName, @DBId;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @Sql NVARCHAR(MAX);
			SET @Sql = N'SELECT ' + STR(@DBId) + N' AS [database_id]  							
							,[principal_id]
							,[name]  
							,[type_desc]
						 FROM ' + QUOTENAME(@DBName) + N'.[sys].[database_principals]
						 WHERE [type] IN (''U'', ''S'')';
			INSERT @DBUsers EXEC sp_executesql @Sql;
			FETCH NEXT FROM DBUsersCursor INTO @DBName, @DBId;
		END;
CLOSE DBUsersCursor;
DEALLOCATE DBUsersCursor;

SELECT * FROM @DBUsers;