USE [master]
GO

-- Variables DatabaseName, BackupName must be set
:on error exit
GO

ALTER DATABASE [$(DatabaseName)] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
RESTORE DATABASE [$(DatabaseName)] FROM DISK='$(BackupName)' WITH REPLACE
ALTER DATABASE [$(DatabaseName)] SET MULTI_USER