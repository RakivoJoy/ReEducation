INSERT INTO dbo.Customers (FirstName, LastName, Email) VALUES
    (N'Peter', N'Karlsson', N'peter.karlsson@example.com'),
    (N'Petra', N'Holm',     N'petra.holm@example.com');
GO

EXEC dbo.GetCustomersWhoseFirstNameStartsWithP;

EXEC dbo.GetTotalOrderAmountForCustomer;
EXEC dbo.GetTotalOrderAmountForCustomer @CustomerID = 2;

