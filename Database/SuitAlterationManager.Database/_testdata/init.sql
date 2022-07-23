DECLARE @UserAdminID UNIQUEIDENTIFIER = '33468EEC-17A5-4EAC-98BF-32A8C9E79C51';

MERGE INTO [System].[User] AS Target
USING (VALUES
   (@UserAdminID,'admin@mail.it', '$2a$11$XyIze7noyK/VN/MsXv9MMuMyH.vNoVFfMB3/20idC4LGtx86zp62S')
) AS Source (
       [Id]
      ,[Email]
      ,[Password]
  )
ON (Target.[Email] = Source.[Email])
WHEN NOT MATCHED BY TARGET THEN
    INSERT([Id], [Email], [Password])
    VALUES (Source.[Id], Source.[Email], Source.[Password]);
