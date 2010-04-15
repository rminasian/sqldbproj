IF ([dbo].NeedMigrate('<<<index>>>') = 1) 
    BEGIN
        PRINT N'Applying migration to version <<<index>>>'
