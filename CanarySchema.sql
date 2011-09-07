use Canary
go

drop table dbo.[EventInstance]
go
drop table dbo.[Event]
go
drop table dbo.[Application]
go

create table dbo.[Application] (
	[Id] int not null identity(1,1)
		constraint [PK_Application] primary key,
	[Name] varchar(128) not null,
	[Environment] varchar(128) not null
)
go

create table dbo.[Event] (
	[Id] int not null identity(1,1)
		constraint [PK_Event] primary key,
	[AppId] int not null
		constraint [FK_Event_App] foreign key references dbo.[Application]([Id]),
	[Level] int not null,
	[Type] varchar(256) not null,
	[Message] varchar(max) not null,
	[Source] varchar(max) not null,
	[Hash] varchar(40) not null,
	[FirstTimestamp] datetime not null,
	[LastTimestamp] datetime not null,
	[TotalCount] int not null
)
go

create table dbo.EventInstance (
	[Id] int not null identity(1,1)
		constraint [PK_EventInstance] primary key,
	[EventId] int not null
		constraint [FK_EventInstance_Event] foreign key references dbo.[Event]([Id]),
	[User] varchar(256) not null,
	[Details] varchar(max) not null,
	[Timestamp] datetime not null
)
go