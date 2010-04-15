USE [master]
GO

-- Variables DatabaseName, BackupName must be set
:on error exit
GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN 
    ALTER DATABASE [$(DatabaseName)] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    BACKUP DATABASE [$(DatabaseName)] TO DISK='$(BackupName)' WITH INIT
    ALTER DATABASE [$(DatabaseName)] SET MULTI_USER 
END