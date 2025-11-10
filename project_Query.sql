-- Create Database safely (MSSQL syntax)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Learnable_DB')
BEGIN
    CREATE DATABASE Learnable_DB;
END
GO

USE Learnable_DB;
GO

-- ======================
-- USER TABLE
-- ======================
CREATE TABLE [User] (
    UserId UNIQUEIDENTIFIER PRIMARY KEY,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    DisplayName NVARCHAR(100),
    PasswordHash NVARCHAR(255) NOT NULL,
    Salt NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    Name NVARCHAR(100),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- ======================
-- TEACHER TABLE
-- ======================
CREATE TABLE Teacher (
    ProfileId UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER UNIQUE,
    DateOfBirth DATE NULL,
    ContactPhone NVARCHAR(20),
    Bio NVARCHAR(MAX),
    AvatarUrl NVARCHAR(255),
    LastUpdatedAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
GO

-- ======================
-- STUDENT TABLE
-- ======================
CREATE TABLE Student (
    StudentId UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER UNIQUE,
    EnrollmentDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
GO

-- ======================
-- CLASS TABLE
-- ======================
CREATE TABLE Class (
    ClassId UNIQUEIDENTIFIER PRIMARY KEY,
    TeacherId UNIQUEIDENTIFIER,
    ClassName NVARCHAR(100) NOT NULL,
    ClassJoinName NVARCHAR(100) UNIQUE NOT NULL,
    Description NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Active',
    FOREIGN KEY (TeacherId) REFERENCES Teacher(ProfileId)
);
GO

-- ======================
-- CLASS_STUDENT TABLE
-- ======================
CREATE TABLE ClassStudent (
    ClassId UNIQUEIDENTIFIER,
    StudentId UNIQUEIDENTIFIER,
    JoinDate DATETIME DEFAULT GETDATE(),
    StudentStatus NVARCHAR(50) DEFAULT 'Active',
    PRIMARY KEY (ClassId, StudentId),
    FOREIGN KEY (ClassId) REFERENCES Class(ClassId),
    FOREIGN KEY (StudentId) REFERENCES Student(StudentId)
);
GO

-- ======================
-- REPOSITORY TABLE
-- ======================
CREATE TABLE Repository (
    RepoId UNIQUEIDENTIFIER PRIMARY KEY,
    ClassId UNIQUEIDENTIFIER,
    RepoName NVARCHAR(100) NOT NULL,
    RepoDescription NVARCHAR(MAX),
    RepoCertification NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) DEFAULT 'Active',
    FOREIGN KEY (ClassId) REFERENCES Class(ClassId)
);
GO

-- ======================
-- ASSETS TABLE
-- ======================
CREATE TABLE Assets (
    AssetsProfileId UNIQUEIDENTIFIER PRIMARY KEY,
    RepoId UNIQUEIDENTIFIER,
    Type NVARCHAR(50) NOT NULL,
    Title NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    LastUpdatedAt DATETIME,
    Url NVARCHAR(255) NOT NULL,
    FOREIGN KEY (RepoId) REFERENCES Repository(RepoId)
);
GO

-- ======================
-- EXAM TABLE
-- ======================
CREATE TABLE Exam (
    ExamId UNIQUEIDENTIFIER PRIMARY KEY,
    RepoId UNIQUEIDENTIFIER,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    StartDatetime DATETIME,
    EndDatetime DATETIME,
    Duration INT,
    FOREIGN KEY (RepoId) REFERENCES Repository(RepoId)
);
GO

-- ======================
-- MARKS TABLE
-- ======================
CREATE TABLE Marks (
    ExamId UNIQUEIDENTIFIER,
    StudentId UNIQUEIDENTIFIER,
    Marks INT,
    ExamStatus NVARCHAR(50),
    PRIMARY KEY (ExamId, StudentId),
    FOREIGN KEY (ExamId) REFERENCES Exam(ExamId),
    FOREIGN KEY (StudentId) REFERENCES Student(StudentId)
);
GO

-- ======================
-- AUDIT_LOG TABLE
-- ======================
CREATE TABLE AuditLog (
    LogId UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER,
    ClassId UNIQUEIDENTIFIER,
    Action NVARCHAR(50),
    Timestamp DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50),
    FOREIGN KEY (UserId) REFERENCES [User](UserId),
    FOREIGN KEY (ClassId) REFERENCES Class(ClassId)
);
GO

-- ======================
-- REQUEST_NOTIFICATION TABLE
-- ======================
CREATE TABLE RequestNotification (
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY,
    SenderId UNIQUEIDENTIFIER,
    ReceiverId UNIQUEIDENTIFIER,
    NotificationStatus NVARCHAR(50) DEFAULT 'Sent',
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SenderId) REFERENCES [User](UserId),
    FOREIGN KEY (ReceiverId) REFERENCES [User](UserId)
);
GO

-- ======================
-- PROMPT TABLE
-- ======================
CREATE TABLE Prompt (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    PromptCode NVARCHAR(100),
    PromptText NVARCHAR(MAX)
);
GO
