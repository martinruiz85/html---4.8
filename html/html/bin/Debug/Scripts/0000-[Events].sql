---- Script de columnas (Events) ----
-- PRIMARY KEY: PK__Events__3214EC07C770AD16
-- ======= COLUMNAS =======
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Id' AND Object_ID = Object_ID(N'[dbo].[Events]'))
BEGIN
    ALTER TABLE [dbo].[Events] ADD [Id] bigint NOT NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RegistrationDate' AND Object_ID = Object_ID(N'[dbo].[Events]'))
BEGIN
    ALTER TABLE [dbo].[Events] ADD [RegistrationDate] datetime NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'SubsidiaryId' AND Object_ID = Object_ID(N'[dbo].[Events]'))
BEGIN
    ALTER TABLE [dbo].[Events] ADD [SubsidiaryId] int NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'PartnerId' AND Object_ID = Object_ID(N'[dbo].[Events]'))
BEGIN
    ALTER TABLE [dbo].[Events] ADD [PartnerId] int NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Status' AND Object_ID = Object_ID(N'[dbo].[Events]'))
BEGIN
    ALTER TABLE [dbo].[Events] ADD [Status] int NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'DownloadDate' AND Object_ID = Object_ID(N'[dbo].[Events]'))
BEGIN
    ALTER TABLE [dbo].[Events] ADD [DownloadDate] datetime NULL;
END
-- ======= DEFAULT CONSTRAINTS =======
-- ======= CHECK CONSTRAINTS =======
-- ======= FOREIGN KEYS =======
