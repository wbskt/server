﻿CREATE TABLE [dbo].[Users] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [UserName]     VARCHAR (100) NOT NULL,
    [Email]        VARCHAR (100) NOT NULL,
    [PasswordHash] VARCHAR (512) NOT NULL,
    [PasswordSalt] VARCHAR(50) NOT NULL, 
    CONSTRAINT [pk_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

