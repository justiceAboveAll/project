SELECT  [server_roles].[principal_id]
	   ,[server_roles].[name]
	   ,[server_roles].[type_desc]
	   ,[server_role_members].[member_principal_id]
FROM [master].[sys].[server_role_members] AS [server_role_members]
JOIN (
		SELECT [name] 
		      ,[principal_id] 
			  ,[type_desc] 
		FROM [master].[sys].[server_principals] 
		WHERE [type] = 'R'
	) AS [server_roles]
ON [server_role_members].[role_principal_id] = [server_roles].[principal_id];