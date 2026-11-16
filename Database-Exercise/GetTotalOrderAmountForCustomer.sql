-- Creates the procedure if it does not exist.
-- Alters it if it already exists.
CREATE OR ALTER PROCEDURE dbo.GetTotalOrderAmountForCustomer
    @CustomerID INT = 1 -- Uses customer 1 when no parameter is supplied
AS
BEGIN
    -- Suppresses "(N rows affected)" messages.
    SET NOCOUNT ON;

    SELECT
        c.CustomerID,

        -- Builds the customer's full name.
        c.FirstName + N' ' + c.LastName AS CustomerName,

        -- Calculates Quantity * UnitPrice for every item,
        -- adds the values, and returns 0 when no items exist.
        ISNULL(SUM(oi.Quantity * oi.UnitPrice), 0) AS TotalOrderAmount

    -- The customer table is the starting point.
    FROM dbo.Customers AS c

    -- Finds orders belonging to the customer.
    -- LEFT JOIN keeps customers with no orders.
    LEFT JOIN dbo.Orders AS o
        ON o.CustomerID = c.CustomerID

    -- Finds items belonging to each order.
    -- LEFT JOIN keeps orders or customers with no items.
    LEFT JOIN dbo.OrderItems AS oi
        ON oi.OrderID = o.OrderID

    -- Selects only the requested customer.
    WHERE c.CustomerID = @CustomerID

    -- Required because SUM is used and the customer columns
    -- are also included in the SELECT list.
    GROUP BY
        c.CustomerID,
        c.FirstName,
        c.LastName;
END;
GO