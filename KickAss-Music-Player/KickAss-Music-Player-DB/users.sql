CREATE TABLE [dbo].[users]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Username] VARCHAR(30) NOT NULL,
	Password varchar(255) not null,
    [Firstname] VARCHAR(50) NOT NULL, 
    [Lastname] VARCHAR(50) NOT NULL,
	[Active] BIT NOT NULL DEFAULT 0,
    [CreatedBy] INT NOT NULL, 
    [CreationDate] DATETIME2 NOT NULL, 
    [ModifiedBy] NCHAR(10) NULL, 
    [ModificationDate] DATETIME2 NULL
)

GO

CREATE NONCLUSTERED INDEX [NIX_UsersUsername] ON [dbo].[users] ([Username]) include (Id, Active, Firstname, Lastname) WITH (ALLOW_PAGE_LOCKS=ON, ALLOW_ROW_LOCKS=ON, FILLFACTOR=90);