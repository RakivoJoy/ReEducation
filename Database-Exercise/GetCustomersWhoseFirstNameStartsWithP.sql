CREATE OR ALTER PROCEDURE dbo.GetCustomersWhoseFirstNameStartsWithP
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  CustomerID, FirstName, LastName, Email, Phone
    FROM    dbo.Customers
    WHERE   FirstName LIKE N'P%'
    ORDER BY FirstName, LastName;
END;
GO