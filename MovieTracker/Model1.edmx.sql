
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/11/2016 13:27:57
-- Generated from EDMX file: C:\Users\Bojan\Documents\SmartGit\MovieTracker\MovieTracker\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Movie];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MovieGenres_Genre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieGenres] DROP CONSTRAINT [FK_MovieGenres_Genre];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieGenres_Movie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieGenres] DROP CONSTRAINT [FK_MovieGenres_Movie];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Genres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genres];
GO
IF OBJECT_ID(N'[dbo].[Movies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Movies];
GO
IF OBJECT_ID(N'[dbo].[MovieGenres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovieGenres];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Genres'
CREATE TABLE [dbo].[Genres] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Movies'
CREATE TABLE [dbo].[Movies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImdbID] nvarchar(10)  NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [Year] datetime  NOT NULL,
    [Runtime] int  NOT NULL,
    [Director] nvarchar(50)  NOT NULL,
    [Actors] nvarchar(max)  NOT NULL,
    [Plot] nvarchar(max)  NOT NULL,
    [Awards] nvarchar(max)  NOT NULL,
    [Language] nvarchar(max)  NOT NULL,
    [Image] nvarchar(max)  NULL,
    [Rating] decimal(2,1)  NOT NULL,
    [Type] int  NULL
);
GO

-- Creating table 'MovieGenres'
CREATE TABLE [dbo].[MovieGenres] (
    [Genres_Id] int  NOT NULL,
    [Movies_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Genres'
ALTER TABLE [dbo].[Genres]
ADD CONSTRAINT [PK_Genres]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Movies'
ALTER TABLE [dbo].[Movies]
ADD CONSTRAINT [PK_Movies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Genres_Id], [Movies_Id] in table 'MovieGenres'
ALTER TABLE [dbo].[MovieGenres]
ADD CONSTRAINT [PK_MovieGenres]
    PRIMARY KEY CLUSTERED ([Genres_Id], [Movies_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Genres_Id] in table 'MovieGenres'
ALTER TABLE [dbo].[MovieGenres]
ADD CONSTRAINT [FK_MovieGenres_Genre]
    FOREIGN KEY ([Genres_Id])
    REFERENCES [dbo].[Genres]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Movies_Id] in table 'MovieGenres'
ALTER TABLE [dbo].[MovieGenres]
ADD CONSTRAINT [FK_MovieGenres_Movie]
    FOREIGN KEY ([Movies_Id])
    REFERENCES [dbo].[Movies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieGenres_Movie'
CREATE INDEX [IX_FK_MovieGenres_Movie]
ON [dbo].[MovieGenres]
    ([Movies_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------