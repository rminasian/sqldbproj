-- Ensure version table
IF (NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[Version]') AND OBJECTPROPERTY(id, N'IsTable') = 1))
BEGIN
    PRINT 'Creating [dbo].[Version]...'
	EXEC('CREATE TABLE [dbo].[Version] ([Current] INT)')
	EXEC('INSERT INTO [dbo].[Version] VALUES (0)')
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[NeedMigrate]') AND OBJECTPROPERTY(id, N'IsScalarFunction') = 1)
BEGIN
	EXEC('DROP FUNCTION dbo.NeedMigrate')
END
GO

CREATE FUNCTION dbo.[NeedMigrate]
(
    @ToVersion INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @Result BIT
	SELECT @Result =
		CASE WHEN EXISTS ( SELECT 1 FROM dbo.[Version] WHERE [Current] < @ToVersion ) THEN 1 ELSE 0 END
    RETURN @Result
END
GO