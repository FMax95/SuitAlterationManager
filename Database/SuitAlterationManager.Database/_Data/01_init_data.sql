DECLARE @VISITOR_GroupID UNIQUEIDENTIFIER = '593EFDF6-1EE7-4756-96B0-0F983B1B8D8A'; 
DECLARE @ADMIN_GroupID UNIQUEIDENTIFIER = 'B8C72CAE-DCBB-4C15-9FAD-3EEEECEF81EF';

DECLARE @UserAdminID UNIQUEIDENTIFIER = '33468EEC-17A5-4EAC-98BF-32A8C9E79C51';

/* Permissions variables */
DECLARE @CONTEXT_User_ID UNIQUEIDENTIFIER = 'FC7DA321-D100-446B-B535-32DF1682C41B'; 
			
DECLARE @ACTION_User_Read_ID UNIQUEIDENTIFIER = 'AD817C5A-61A7-492E-8230-B89BC35BE018'; 
DECLARE @ACTION_User_Create_ID UNIQUEIDENTIFIER = '903ECB6C-BBA3-47CB-BBA8-A0A7B02C4C62'; 
DECLARE @ACTION_User_Update_ID UNIQUEIDENTIFIER = 'C39339FB-19F6-49D6-AE10-6698F852110B'; 
DECLARE @ACTION_User_Delete_ID UNIQUEIDENTIFIER = 'D1951F66-019E-4142-AAB9-0538ADE2A22B'; 
DECLARE @ACTION_Read VARCHAR(100) = 'Read'; 
DECLARE @ACTION_Create VARCHAR(100) = 'Create'; 
DECLARE @ACTION_Update VARCHAR(100) = 'Update'; 
DECLARE @ACTION_Delete VARCHAR(100) = 'Delete'; 

CREATE TABLE #ContextList ([Id] UNIQUEIDENTIFIER, [Name] VARCHAR(100));
INSERT INTO #ContextList VALUES
(@CONTEXT_User_ID, 'User')
;

CREATE TABLE #ActionList ([Id] UNIQUEIDENTIFIER, [IdContext] UNIQUEIDENTIFIER, [Name] varchar(100));
INSERT INTO #ActionList VALUES
(@ACTION_User_Read_ID, @CONTEXT_User_ID, @ACTION_Read),
(@ACTION_User_Create_ID, @CONTEXT_User_ID, @ACTION_Create),
(@ACTION_User_Update_ID, @CONTEXT_User_ID, @ACTION_Update),
(@ACTION_User_Delete_ID, @CONTEXT_User_ID, @ACTION_Delete)
;

CREATE TABLE #GroupPermissionList ([IdGroup] UNIQUEIDENTIFIER, [IdAction] UNIQUEIDENTIFIER);
INSERT INTO #GroupPermissionList VALUES
(@ADMIN_GroupID, @ACTION_User_Read_ID),
(@ADMIN_GroupID, @ACTION_User_Create_ID),
(@ADMIN_GroupID, @ACTION_User_Update_ID),
(@ADMIN_GroupID, @ACTION_User_Delete_ID)
;
/* END Permissions variables */



/* Table [System].[Group]  */ 
MERGE INTO [System].[Group] AS Target
USING (VALUES
   (@VISITOR_GroupID, 'Visitor', 'Visitors', 1)
   , (@ADMIN_GroupID, 'Admin', 'Administrators', 1)
) AS Source (
      [Id]
      ,[Name]
      ,[Description]
      ,[IsEnabled]
  )
ON (Target.[Id] = Source.[Id])
WHEN MATCHED THEN 
     UPDATE SET
      [Name] = Source.[Name]
      ,[Description] = Source.[Description]
      ,[IsEnabled] = Source.[IsEnabled]
      ,[UpdateDate] =  SYSDATETIMEOFFSET()
WHEN NOT MATCHED BY TARGET THEN
    INSERT([Id], [Name], [Description], [IsEnabled], [UpdateDate])
    VALUES (Source.[Id], Source.[Name], Source.[Description], Source.[IsEnabled],  SYSDATETIMEOFFSET())
;


/* Table [System].[User] - First Administrator User */ 
MERGE INTO [System].[User] AS Target
USING (VALUES
   (@UserAdminID, 'admin@mail.it', '$2a$11$CHayYOcmhEqECxqwY2FB2.1j5MTKTo/9MwBTTsEBcl8hTI2uvbjwO', 1)
) AS Source (
       [Id]
	  ,[Email]
      ,[Password]
      ,[IsEnabled]
  )
ON (Target.[Id] = Source.[Id])
WHEN MATCHED THEN 
     UPDATE SET
       [Password] = Source.[Password]
      ,[Email] = Source.[Email]
      ,[IsEnabled] = Source.[IsEnabled]
      ,[UpdateDate] = SYSDATETIMEOFFSET() 
WHEN NOT MATCHED BY TARGET THEN
    INSERT([Id],[Password], [Email], [IsEnabled], [UpdateDate],[IsDeleted])
    VALUES (Source.[Id], Source.[Password],  Source.[Email], Source.[IsEnabled], SYSDATETIMEOFFSET(),0)
;


/* Table [System].[UserInformation] - First Administrator User */ 
MERGE INTO [System].[UserInformation] AS Target
USING (VALUES
   (@UserAdminID, 'System', 'Administrator', null)
) AS Source (
       [IdUser]
      ,[FirstName]
      ,[LastName]
      ,[BirthDate]
  )
ON (Target.[IdUser] = Source.[IdUser])
WHEN MATCHED THEN 
     UPDATE SET
       [BirthDate] = Source.[BirthDate]
      ,[FirstName] = Source.[FirstName]
      ,[LastName] = Source.[LastName]
WHEN NOT MATCHED BY TARGET THEN
    INSERT([IdUser], [BirthDate], [FirstName], [LastName])
    VALUES (Source.[IdUser], Source.[BirthDate], Source.[FirstName], Source.[LastName])
;

/* Table [System].[UserGroup]  */ 
MERGE INTO [System].[UserGroup] AS Target
USING (VALUES
   (@UserAdminID, @ADMIN_GroupID)
) AS Source (
      [IdUser]
      ,[IdGroup]
  )
ON (Target.[IdUser] = Source.[IdUser] AND Target.[IdGroup] = Source.[IdGroup])
WHEN MATCHED THEN 
     UPDATE SET
      [IdGroup] = Source.[IdGroup]
WHEN NOT MATCHED BY TARGET THEN
    INSERT([IdUser], [IdGroup])
    VALUES (Source.[IdUser], Source.[IdGroup])
;


/* Table [System].[Context]  */ 
MERGE INTO [System].[Context] AS Target
USING #ContextList AS Source
ON Target.[Id] = Source.[Id]
WHEN MATCHED THEN 
    UPDATE SET
      [Name] = Source.[Name]
      ,[UpdateDate] = SYSDATETIMEOFFSET() 
WHEN NOT MATCHED BY TARGET THEN
    INSERT([Id], [Name], [UpdateDate])
    VALUES (Source.[Id], Source.[Name],  SYSDATETIMEOFFSET())
;

/* Table [System].[Action]  */ 
MERGE INTO [System].[Action] AS Target
USING #ActionList AS Source
ON Target.[Id] = Source.[Id]
WHEN MATCHED THEN 
    UPDATE SET
      [Name] = Source.[Name]
      ,[UpdateDate] = SYSDATETIMEOFFSET() 
WHEN NOT MATCHED BY TARGET THEN
    INSERT([Id], [IdContext], [Name], [UpdateDate])
    VALUES (Source.[Id], Source.[IdContext], Source.[Name],  SYSDATETIMEOFFSET())
;

/* Table [System].[GroupPermission]  */ 
INSERT INTO [System].[GroupPermission] 
SELECT Source.[IdGroup], Source.[IdAction]
FROM #GroupPermissionList AS Source
    FULL OUTER JOIN [System].[GroupPermission] AS Target ON Target.[IdGroup] != Source.[IdGroup] AND Target.[IdAction] != Source.[IdAction]
;