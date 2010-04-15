DECLARE @Name       sysname
DECLARE ProcCursor CURSOR FOR 
    SELECT name FROM sysobjects WHERE xtype = 'P' AND [Name] LIKE 'Pr%'
    FOR READ ONLY
OPEN ProcCursor

    FETCH NEXT FROM ProcCursor INTO @Name

    WHILE @@FETCH_STATUS = 0 
    BEGIN
        EXEC('DROP PROCEDURE dbo.' + @Name)
        FETCH NEXT FROM ProcCursor INTO @Name
    END

CLOSE ProcCursor
DEALLOCATE ProcCursor

DECLARE FunctionCursor CURSOR FOR 
    SELECT 
        name 
    FROM 
        sysobjects 
    WHERE 
        xtype IN ( 'TF', 'IF', 'FN' ) 
        AND (
            [Name] LIKE 'Fn%'
        )
    FOR READ ONLY
OPEN FunctionCursor

    FETCH NEXT FROM FunctionCursor INTO @Name

    WHILE @@FETCH_STATUS = 0 
    BEGIN
        EXEC('DROP FUNCTION dbo.' + @Name)
        FETCH NEXT FROM FunctionCursor INTO @Name
    END

CLOSE FunctionCursor
DEALLOCATE FunctionCursor
GO
