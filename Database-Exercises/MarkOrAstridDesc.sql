USE ShopDB;
GO

CREATE OR ALTER PROCEDURE dbo.GetCustomersMarkOrAstrid
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  CustomerID,
            FirstName,
            LastName,
            Email,
            Phone,
            CreatedAt
    FROM    dbo.Customers
    WHERE   FirstName IN (N'Mark', N'Astrid')
    ORDER BY FirstName DESC;
END;
GO

-- Anropa proceduren
EXEC dbo.GetCustomersMarkOrAstrid;