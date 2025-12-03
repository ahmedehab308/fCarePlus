CREATE DATABASE [fCarePlus];
GO

-- create AccountsChart table


CREATE TABLE [dbo].[JournalHeader] (
    
    [Journal_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [Journal_Date] DATE NOT NULL,
    [Journal_Description] NVARCHAR(500) NOT NULL,
    [Total_Debit] DECIMAL(18, 4) NOT NULL,
    [Total_Credit] DECIMAL(18, 4) NOT NULL,

    [Creation_Date] DATETIME DEFAULT GETDATE(), 
    [User_ID] BIGINT NULL 
    
);
GO

CREATE TABLE [dbo].[JournalDetails] (
    [Detail_ID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [Journal_ID] BIGINT NOT NULL,
    [Account_ID] UNIQUEIDENTIFIER NOT NULL,
    [Debit_Amount] DECIMAL(18, 4) DEFAULT 0,
    [Credit_Amount] DECIMAL(18, 4) DEFAULT 0,
    [Detail_Statement] NVARCHAR(500) NULL,
    
    FOREIGN KEY ([Journal_ID]) REFERENCES [dbo].[JournalHeader]([Journal_ID]),
    FOREIGN KEY ([Account_ID]) REFERENCES [dbo].[AccountsChart]([ID])
);
GO

ALTER TABLE [dbo].[JournalHeader] 
ADD [Is_Deleted] BIT NOT NULL DEFAULT 0; 
GO

ALTER TABLE [dbo].[JournalDetails] 
ADD [Is_Deleted] BIT NOT NULL DEFAULT 0; 
GO


