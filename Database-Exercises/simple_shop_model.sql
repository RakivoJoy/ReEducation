/* ============================================================
   Simple ER model: Online Shop
   Target: Microsoft SQL Server

   Entities and relationships:
     Customers  1 --- N  Orders
     Orders     1 --- N  OrderItems
     Products   1 --- N  OrderItems
     Categories 1 --- N  Products
   (Orders <-> Products is many-to-many via OrderItems)
   ============================================================ */

-- Create and use the database (optional - remove if you already have one)
IF DB_ID(N'ShopDB') IS NULL
    CREATE DATABASE ShopDB;
GO

USE ShopDB;
GO

/* ---------- Drop existing tables (child tables first) ---------- */
IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NOT NULL DROP TABLE dbo.OrderItems;
IF OBJECT_ID(N'dbo.Orders',     N'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID(N'dbo.Products',   N'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID(N'dbo.Categories', N'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID(N'dbo.Customers',  N'U') IS NOT NULL DROP TABLE dbo.Customers;
GO

/* ---------- Tables ---------- */
CREATE TABLE dbo.Customers (
    CustomerID  INT IDENTITY(1,1) NOT NULL,
    FirstName   NVARCHAR(50)  NOT NULL,
    LastName    NVARCHAR(50)  NOT NULL,
    Email       NVARCHAR(100) NOT NULL,
    Phone       NVARCHAR(20)  NULL,
    CreatedAt   DATETIME2(0)  NOT NULL CONSTRAINT DF_Customers_CreatedAt DEFAULT SYSDATETIME(),
    CONSTRAINT PK_Customers PRIMARY KEY (CustomerID),
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
GO

CREATE TABLE dbo.Categories (
    CategoryID   INT IDENTITY(1,1) NOT NULL,
    CategoryName NVARCHAR(60)  NOT NULL,
    Description  NVARCHAR(255) NULL,
    CONSTRAINT PK_Categories PRIMARY KEY (CategoryID),
    CONSTRAINT UQ_Categories_Name UNIQUE (CategoryName)
);
GO

CREATE TABLE dbo.Products (
    ProductID    INT IDENTITY(1,1) NOT NULL,
    CategoryID   INT           NOT NULL,
    ProductName  NVARCHAR(100) NOT NULL,
    UnitPrice    DECIMAL(10,2) NOT NULL,
    StockQty     INT           NOT NULL CONSTRAINT DF_Products_Stock DEFAULT 0,
    CONSTRAINT PK_Products PRIMARY KEY (ProductID),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID)
        REFERENCES dbo.Categories (CategoryID),
    CONSTRAINT CK_Products_Price CHECK (UnitPrice >= 0),
    CONSTRAINT CK_Products_Stock CHECK (StockQty >= 0)
);
GO

CREATE TABLE dbo.Orders (
    OrderID     INT IDENTITY(1,1) NOT NULL,
    CustomerID  INT           NOT NULL,
    OrderDate   DATETIME2(0)  NOT NULL CONSTRAINT DF_Orders_Date DEFAULT SYSDATETIME(),
    Status      NVARCHAR(20)  NOT NULL CONSTRAINT DF_Orders_Status DEFAULT N'New',
    CONSTRAINT PK_Orders PRIMARY KEY (OrderID),
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID)
        REFERENCES dbo.Customers (CustomerID),
    CONSTRAINT CK_Orders_Status CHECK (Status IN (N'New', N'Paid', N'Shipped', N'Cancelled'))
);
GO

CREATE TABLE dbo.OrderItems (
    OrderID     INT           NOT NULL,
    ProductID   INT           NOT NULL,
    Quantity    INT           NOT NULL,
    UnitPrice   DECIMAL(10,2) NOT NULL,   -- price at time of purchase
    CONSTRAINT PK_OrderItems PRIMARY KEY (OrderID, ProductID),
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderID)
        REFERENCES dbo.Orders (OrderID) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductID)
        REFERENCES dbo.Products (ProductID),
    CONSTRAINT CK_OrderItems_Qty CHECK (Quantity > 0)
);
GO

/* ---------- Indexes on foreign keys ---------- */
CREATE INDEX IX_Products_CategoryID  ON dbo.Products (CategoryID);
CREATE INDEX IX_Orders_CustomerID    ON dbo.Orders (CustomerID);
CREATE INDEX IX_OrderItems_ProductID ON dbo.OrderItems (ProductID);
GO

/* ---------- Sample data ---------- */
INSERT INTO dbo.Customers (FirstName, LastName, Email, Phone) VALUES
    (N'Anna',  N'Lindberg', N'anna.lindberg@example.com', N'+46701234567'),
    (N'Erik',  N'Svensson', N'erik.svensson@example.com', NULL),
    (N'Maria', N'Nilsson',  N'maria.nilsson@example.com',  N'+46709876543'),
    (N'Mark', N'Pack',  N'mark@example.com',  N'+445646543'),
    (N'Astrid', N'Oblex',  N'astra@example.com',  N'+4670925243');

INSERT INTO dbo.Categories (CategoryName, Description) VALUES
    (N'Books',       N'Printed and digital books'),
    (N'Electronics', N'Gadgets and accessories');

INSERT INTO dbo.Products (CategoryID, ProductName, UnitPrice, StockQty) VALUES
    (1, N'SQL for Beginners',   29.90, 100),
    (1, N'Database Design',     39.50,  50),
    (2, N'USB-C Cable',          9.99, 300),
    (2, N'Wireless Mouse',      19.99, 120);

INSERT INTO dbo.Orders (CustomerID, Status) VALUES
    (1, N'Paid'),
    (2, N'New');

INSERT INTO dbo.OrderItems (OrderID, ProductID, Quantity, UnitPrice) VALUES
    (1, 1, 1, 29.90),
    (1, 3, 2,  9.99),
    (2, 4, 1, 19.99);
GO

/* ---------- Example query: order totals per customer ---------- */
SELECT  c.CustomerID,
        c.FirstName + N' ' + c.LastName AS CustomerName,
        o.OrderID,
        o.OrderDate,
        o.Status,
        SUM(oi.Quantity * oi.UnitPrice) AS OrderTotal
FROM    dbo.Customers  c
JOIN    dbo.Orders     o  ON o.CustomerID = c.CustomerID
JOIN    dbo.OrderItems oi ON oi.OrderID   = o.OrderID
GROUP BY c.CustomerID, c.FirstName, c.LastName, o.OrderID, o.OrderDate, o.Status
ORDER BY o.OrderID;
GO
