-- User table
CREATE TABLE [dbo].[Users] (
    [UserID]          INT           IDENTITY(1000,1) NOT NULL,
    [FirstName]       VARCHAR(50)   NOT NULL,
    [LastName]        VARCHAR(50)   NOT NULL,
    [UserName]        VARCHAR(100)  NOT NULL,
    [Password]        VARCHAR(MAX)  NOT NULL,
    [DateRegistered]  DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);

-- Account table
CREATE TABLE Accounts (
    AccountID INT PRIMARY KEY,
    UserID INT,
    AccountNumber VARCHAR(20) UNIQUE,
    Balance DECIMAL(18, 2),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);


-- Transaction table
CREATE TABLE Transactions (
    [TransactionID]   INT IDENTITY(1,1) NOT NULL,
    [UserID]          INT NOT NULL,
    [TransactionType] VARCHAR(50) NOT NULL,
    [Amount]          DECIMAL(18, 2) NOT NULL,
    [TransactionDate] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([TransactionID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID])
);

-- Deposit table
CREATE TABLE Deposits (
    [DepositID]   INT IDENTITY(1,1) NOT NULL,
    [UserID]      INT NOT NULL,
    [Amount]      DECIMAL(18, 2) NOT NULL,
    [DepositDate] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([DepositID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID])
);

-- Withdrawal table
CREATE TABLE Withdrawals (
    [WithdrawalID]   INT IDENTITY(1,1) NOT NULL,
    [UserID]         INT NOT NULL,
    [Amount]         DECIMAL(18, 2) NOT NULL, 
    [WithdrawalDate] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([WithdrawalID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID])
);

-- SendMoney table
CREATE TABLE SendMoney (
    [SendMoneyID]     INT IDENTITY(1,1) NOT NULL,
    [SenderUserID]    INT NOT NULL,
    [ReceiverUserID]  INT NOT NULL,
    [Amount]          DECIMAL(18, 2) NOT NULL,
    [SendDate]        DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([SendMoneyID] ASC),
    FOREIGN KEY ([SenderUserID]) REFERENCES [Users]([UserID]),
    FOREIGN KEY ([ReceiverUserID]) REFERENCES [Users]([UserID])
);
