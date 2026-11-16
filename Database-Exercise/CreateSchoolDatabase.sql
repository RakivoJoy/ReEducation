-- Create database
CREATE DATABASE School;
GO

USE School;
GO


-- Create table Students
CREATE TABLE Students
(
    S_ID INT NOT NULL,
    FName NVARCHAR(20),
    LName NVARCHAR(20),
    Comment Text,
    CONSTRAINT PK_S_ID PRIMARY KEY (S_ID)
);
GO


-- Add students
INSERT INTO Students (S_ID, FName, LName, Comment)
VALUES (1, N'Stina', N'Larsson', 'Good student');

INSERT INTO Students (S_ID, FName, LName)
VALUES (2, N'Bina', N'Bens');

INSERT INTO Students (S_ID, FName, LName, Comment)
VALUES (3, N'Gina', N'öäå', 'OK');

GO


-- Test
SELECT *
FROM Students;

GO