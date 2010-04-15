USE [master]
GO

-- Variable DatabaseName must be set

:on error exit
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END
GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL) 
BEGIN
    ALTER DATABASE [$(DatabaseName)]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$(DatabaseName)];
END

GO
PRINT N'Creating $(DatabaseName)...'
GO
CREATE DATABASE [$(DatabaseName)] COLLATE SQL_Latin1_General_CP1_CI_AS
GO
EXECUTE sp_dbcmptlevel [$(DatabaseName)], 90;


IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
BEGIN
EXEC [$(DatabaseName)].[dbo].[sp_fulltext_database] @action = 'disable'
END
GO

ALTER DATABASE [$(DatabaseName)] SET
	ANSI_NULL_DEFAULT ON,
	ANSI_NULLS ON,
	ANSI_PADDING ON,
	ANSI_WARNINGS ON,
	ARITHABORT ON,
	AUTO_CREATE_STATISTICS ON,
	AUTO_UPDATE_STATISTICS ON,
	CONCAT_NULL_YIELDS_NULL ON,
	QUOTED_IDENTIFIER ON,
	AUTO_CLOSE OFF,
	AUTO_SHRINK OFF,
	CURSOR_CLOSE_ON_COMMIT OFF,
	NUMERIC_ROUNDABORT OFF,
	RECURSIVE_TRIGGERS OFF,
	AUTO_UPDATE_STATISTICS_ASYNC OFF,
	DATE_CORRELATION_OPTIMIZATION OFF,
	TRUSTWORTHY OFF,
	ALLOW_SNAPSHOT_ISOLATION OFF,
	DB_CHAINING OFF
GO