DECLARE @table TABLE(Id INT, Name NVARCHAR(128), Internal_Value INT, Char_Value NVARCHAR(128))
INSERT @table EXEC xp_msver
DECLARE @memory INT,
        @cpuCount TINYINT,
        @instVersion NVARCHAR(128),
        @osVersion NVARCHAR(128)
SELECT @memory = Internal_Value FROM @table WHERE Name = 'PhysicalMemory'
SELECT @cpuCount = Internal_Value FROM @table WHERE Name = 'ProcessorCount'
SELECT @instVersion = Char_Value FROM @table WHERE Name = 'ProductVersion'
SELECT @osVersion = Char_Value FROM @table WHERE Name = 'WindowsVersion'
SELECT @memory AS Memory, 
       @cpuCount AS CpuCount,
       @osVersion AS OsVersion,
       @instVersion AS InstVerstion