
-- create the database if it doesn't exist
IF DB_ID('SSI_EdiViewerDB') IS NULL
BEGIN
    CREATE DATABASE SSI_EdiViewerDB;
END
GO

USE SSI_EdiViewerDB;
GO

-- create the imported files table
IF OBJECT_ID('dbo.ImportedFiles', 'U') IS NULL
BEGIN
    CREATE TABLE ImportedFiles (
        FileID INT IDENTITY(1,1) PRIMARY KEY,
        FileName NVARCHAR(260) NOT NULL,
        ImportedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        TotalCharges DECIMAL(18,2) NOT NULL
    );
END
GO

-- create the claims table
IF OBJECT_ID('dbo.Claims', 'U') IS NULL
BEGIN
    CREATE TABLE Claims (
        ClaimID INT IDENTITY(1,1) PRIMARY KEY,
        FileID INT NOT NULL,

        CLM01 NVARCHAR(50) NOT NULL,
        CLM02 DECIMAL(18,2) NOT NULL,
        CLM03 NVARCHAR(50) NULL,
        CLM04 NVARCHAR(50) NULL,

        CLM05Component1 NVARCHAR(10) NOT NULL,
        CLM05Component2 NVARCHAR(10) NOT NULL,
        CLM05Component3 NVARCHAR(10) NOT NULL,

        CLM06 NVARCHAR(10) NULL,
        CLM07 NVARCHAR(10) NULL,
        CLM08 NVARCHAR(10) NULL,
        CLM09 NVARCHAR(10) NULL,

        FOREIGN KEY (FileID)
            REFERENCES ImportedFiles(FileID)
    );
END
GO
