--Drop table [OTCUsers]
CREATE TABLE [dbo].[OTCUsers]
(
	[UserID] [int] IDENTITY(101,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Mobile] [nvarchar](200) NOT NULL,
	[State] [bit] NOT NULL DEFAULT (1),
	[mtType] [tinyint] NOT NULL DEFAULT (1),
	[IsLockedOut] [bit] NOT NULL DEFAULT (0),
	[CreatedDate] Datetime DEFAULT(getdate()),
	[CreatedBy] int NOT NULL
)

select * from [OTCUsers]

select * from users

UserName,Password,Name,Mobile,State,mtType,IsLockedOut,CreatedDate,CreatedBy

INSERT INTO [OTCUsers] (UserName,Password,Name,Mobile,CreatedBy)
values ('tryanytest@gmail.com','admin','admin','1234567890',0000)

update [OTCUsers] set mtType=2 where UserID=101

Select UserID, UserName, Name, Mobile, State, IsLockedOut, CreatedDate from OTCUsers

select * from [OTCUsers]
delete from [OTCUsers] where username ='nagajothi.manivasagam@gmail.com'