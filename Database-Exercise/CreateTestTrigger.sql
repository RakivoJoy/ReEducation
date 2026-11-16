USE ShopDB;
GO

IF OBJECT_ID(N'dbo.testTrigger', N'U') IS NOT NULL
    DROP TABLE dbo.testTrigger;
GO

CREATE TABLE dbo.testTrigger (
    FirstName VARCHAR(50),
    LastName  VARCHAR(50)
);
GO