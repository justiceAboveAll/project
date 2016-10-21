 SELECT [principal_id]
	   ,[name]
	   ,[type_desc]
	   ,[is_disabled]
  FROM [master].[sys].[server_principals] 
  WHERE [type] IN ('S','U');