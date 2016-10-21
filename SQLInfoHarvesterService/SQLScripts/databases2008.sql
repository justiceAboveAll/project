SELECT [databases].[database_id]
	  ,[databases].[name]
	  ,[databases].[create_date]
	  ,[master_files].[size]
  FROM [master].[sys].[databases] AS databases
  JOIN (
		SELECT [database_id], sum([size]) AS [size] 
		FROM [master].[sys].[master_files] GROUP BY [database_id]
		) AS master_files 
  ON [databases].database_id = [master_files].database_id;