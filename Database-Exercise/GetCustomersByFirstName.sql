USE ShopDB;
GO

CREATE OR ALTER PROCEDURE dbo.GetCustomersByFirstName
    @FirstName NVARCHAR(50) = N'Anna'   -- default vaule
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  CustomerID, FirstName, LastName, Email, Phone
    FROM    dbo.Customers
    WHERE   FirstName = @FirstName
    ORDER BY LastName;
END;
GO