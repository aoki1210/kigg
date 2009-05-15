/****** Object:  FullTextCatalog [StoryComment]    Script Date: 05/15/2009 03:48:01 ******/
IF NOT EXISTS (SELECT * FROM sysfulltextcatalogs ftc WHERE ftc.name = N'StoryComment')
CREATE FULLTEXT CATALOG [StoryComment]
WITH ACCENT_SENSITIVITY = ON
AS DEFAULT
AUTHORIZATION [dbo]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tag](
	[Id] [uniqueidentifier] NOT NULL,
	[UniqueName] [nvarchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Tag]') AND name = N'IX_Tag_Name')
CREATE NONCLUSTERED INDEX [IX_Tag_Name] ON [dbo].[Tag] 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Tag]') AND name = N'IX_Tag_UniqueName_CreatedAat')
CREATE NONCLUSTERED INDEX [IX_Tag_UniqueName_CreatedAat] ON [dbo].[Tag] 
(
	[UniqueName] ASC,
	[CreatedAt] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'26e317c1-1cc8-49ae-8832-00d63c4c2fbd', N'Enterprise-Library', N'Enterprise Library', CAST(0x00009BE7015FEB04 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'e73ff50f-e1a1-4809-90fd-0531c473f638', N'IoC-DI', N'IoC/DI', CAST(0x00009BE700C9C1E6 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'1a7bc711-bb9d-481f-9aef-0d3d0a05a03c', N'Tips', N'Tips', CAST(0x00009BEB016C9E4A AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'30094d14-0861-4faf-83ab-2336dfe2187f', N'ASPNET-MVC', N'ASP.NET MVC', CAST(0x00009BEB0169B68F AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'057bc6b1-a04c-4470-8384-271df8f4b509', N'Linq', N'Linq', CAST(0x00009BE700C9BC0C AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'6bf0f9a4-3290-41fd-a11d-30adfa870c95', N'CLR', N'CLR', CAST(0x00009BE700C9BE64 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'd2e7b22e-1d0b-476e-8ef1-4683728ba414', N'VB.NET', N'VB.NET', CAST(0x00009BE700C9B75E AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'a56a57ed-cfd1-4231-9389-46ff02bc5afb', N'WF', N'WF', CAST(0x00009BE700C9BAE0 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'ffafcda0-f25a-42ce-8eb6-5f7a8ee31bb6', N'Kigg', N'Kigg', CAST(0x00009BEB016EB3CC AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'a2f52aef-cccb-4603-aa36-8f28dc18eb8c', N'DotNetShoutOut', N'DotNetShoutOut', CAST(0x00009BEB0169B67E AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'04cc0c68-ecfa-4d8d-9013-9ae2e9f6279a', N'FSharp', N'F#', CAST(0x00009BE700C9B633 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'10f2736a-bdf2-49d9-9e41-a864ef4f3690', N'XLinq', N'XLinq', CAST(0x00009BE700C9BD38 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'e60999cd-63f9-406d-9cc0-bc38ef028e75', N'NUnit', N'NUnit', CAST(0x00009BE700C9C311 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'564583c7-5d03-4c4e-bdec-d1345f813898', N'MBUnit', N'MBUnit', CAST(0x00009BE700C9C43D AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'c786baea-858f-4fc9-be62-d6abe29b214c', N'WPF', N'WPF', CAST(0x00009BE700C9B889 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'aac37fcc-9857-4593-b752-d86df4add63a', N'Iron-Ruby', N'Iron Ruby', CAST(0x00009BE700C9BF90 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'fbbab560-6785-420a-bb6e-da3a07c4ab43', N'xunit', N'xunit', CAST(0x00009BE700C9C568 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'7a025242-bbdf-4899-a310-e44585dcdba8', N'WCF', N'WCF', CAST(0x00009BE700C9B9B5 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'4b3309ee-601c-457a-a1ed-fc029e03d737', N'CSharp', N'C#', CAST(0x00009BE700C9B508 AS DateTime))
INSERT [dbo].[Tag] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'b262462c-8f35-4148-aa63-fc2c4de13745', N'DLR', N'DLR', CAST(0x00009BE700C9C0BA AS DateTime))
/****** Object:  Table [dbo].[User]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Password] [nvarchar](64) COLLATE Latin1_General_CI_AS NULL,
	[Email] [nvarchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[Role] [int] NOT NULL,
	[LastActivityAt] [datetime] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND name = N'IX_User_Email')
CREATE NONCLUSTERED INDEX [IX_User_Email] ON [dbo].[User] 
(
	[Email] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND name = N'IX_User_LastActivityAt')
CREATE NONCLUSTERED INDEX [IX_User_LastActivityAt] ON [dbo].[User] 
(
	[LastActivityAt] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND name = N'IX_User_Role')
CREATE NONCLUSTERED INDEX [IX_User_Role] ON [dbo].[User] 
(
	[Role] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND name = N'IX_User_UserName')
CREATE NONCLUSTERED INDEX [IX_User_UserName] ON [dbo].[User] 
(
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
INSERT [dbo].[User] ([Id], [UserName], [Password], [Email], [IsActive], [IsLockedOut], [Role], [LastActivityAt], [CreatedAt]) VALUES (N'be1c680a-459c-455d-801e-46eea999903a', N'bee', N'8TBPXN6R8DvcdsASQ2WPGg==', N'muhammad.mosa@yahoo.co.uk', 1, 0, 1, CAST(0x00009BF0016E2DD2 AS DateTime), CAST(0x00009BE7013AAF47 AS DateTime))
INSERT [dbo].[User] ([Id], [UserName], [Password], [Email], [IsActive], [IsLockedOut], [Role], [LastActivityAt], [CreatedAt]) VALUES (N'd58058db-47a2-44ba-a4c0-d1ac20d4381a', N'support', N'HUIyXE3SqvQGIwT2ZKTgBw==', N'support@your-domain.com', 1, 0, 2, CAST(0x00009BE7013AAF44 AS DateTime), CAST(0x00009BE7013AAF44 AS DateTime))
INSERT [dbo].[User] ([Id], [UserName], [Password], [Email], [IsActive], [IsLockedOut], [Role], [LastActivityAt], [CreatedAt]) VALUES (N'332ea6ae-ddd7-425e-9186-d4d9d894388a', N'admin', N'GaKFQUS2Oo92F6byJQGbEg==', N'admin@your-domain.com', 1, 0, 4, CAST(0x00009BEC00029EC0 AS DateTime), CAST(0x00009BE7013AAF39 AS DateTime))
INSERT [dbo].[User] ([Id], [UserName], [Password], [Email], [IsActive], [IsLockedOut], [Role], [LastActivityAt], [CreatedAt]) VALUES (N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'kigguser', N'03XbZD8o+d3YlAFLAUn1hg==', N'kigguser@kigg.com', 1, 0, 0, CAST(0x00009BF0016E6D03 AS DateTime), CAST(0x00009BEB016B3B75 AS DateTime))
/****** Object:  Table [dbo].[KnownSource]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KnownSource]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[KnownSource](
	[Url] [nvarchar](450) COLLATE Latin1_General_CI_AS NOT NULL,
	[Grade] [int] NOT NULL,
 CONSTRAINT [PK_KnownSource] PRIMARY KEY CLUSTERED 
(
	[Url] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[KnownSource] ([Url], [Grade]) VALUES (N'http://weblogs.asp.net', 0)
/****** Object:  Table [dbo].[Category]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[Id] [uniqueidentifier] NOT NULL,
	[UniqueName] [nvarchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[Name] [nvarchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND name = N'IX_Category_UniqueName_CreatedAt')
CREATE NONCLUSTERED INDEX [IX_Category_UniqueName_CreatedAt] ON [dbo].[Category] 
(
	[UniqueName] ASC,
	[CreatedAt] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'7c113ed9-47a4-44f1-bdee-1cc86c02ce07', N'Architecture', N'Architecture', CAST(0x00009BE700C9CFEF AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'541be442-e92b-4df3-b8f4-1f90ab868af1', N'Agile', N'Agile', CAST(0x00009BE700C9D11A AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'51bf8672-60c1-41be-8ed5-2709cd37e898', N'Screencast', N'Screencast', CAST(0x00009BE700C9D371 AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'bc56f1bf-f72f-41e2-b47c-344bc90f2f85', N'Podcast', N'Podcast', CAST(0x00009BE700C9D246 AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'348cc0ce-f578-4fc9-a617-465cfc545baf', N'ASP.NET', N'ASP.NET', CAST(0x00009BE700C9C695 AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'75e18e82-43ec-44a8-951e-59f5959dc521', N'SQL', N'SQL', CAST(0x00009BE700C9CD99 AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'605724ce-3038-43d8-a39d-90aecfb855e8', N'Foundation', N'Foundation', CAST(0x00009BE700C9CEC3 AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'454f46ce-f259-44f3-985d-94c018956390', N'Silverlight', N'Silverlight', CAST(0x00009BE700C9C8EB AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'128ee016-0614-414b-b737-a1dc7fc32b6c', N'Ajax', N'Ajax', CAST(0x00009BE700C9C7BF AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'b4d2489e-9a04-4b13-a951-c2a2a9174a58', N'Web-Service', N'Web Service', CAST(0x00009BE700C9CC6E AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'c2aefcdc-a07b-49e9-8c04-d68fd9f7e9f0', N'UX', N'UX', CAST(0x00009BE700C9CB43 AS DateTime))
INSERT [dbo].[Category] ([Id], [UniqueName], [Name], [CreatedAt]) VALUES (N'd352139d-6cd1-4f57-a2af-d82964ad731c', N'Smart-Client', N'Smart Client', CAST(0x00009BE700C9CA16 AS DateTime))
/****** Object:  Table [dbo].[UserTag]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserTag](
	[UserId] [uniqueidentifier] NOT NULL,
	[TagId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserTag] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[UserTag] ([UserId], [TagId]) VALUES (N'be1c680a-459c-455d-801e-46eea999903a', N'30094d14-0861-4faf-83ab-2336dfe2187f')
INSERT [dbo].[UserTag] ([UserId], [TagId]) VALUES (N'be1c680a-459c-455d-801e-46eea999903a', N'a2f52aef-cccb-4603-aa36-8f28dc18eb8c')
INSERT [dbo].[UserTag] ([UserId], [TagId]) VALUES (N'332ea6ae-ddd7-425e-9186-d4d9d894388a', N'26e317c1-1cc8-49ae-8832-00d63c4c2fbd')
INSERT [dbo].[UserTag] ([UserId], [TagId]) VALUES (N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'1a7bc711-bb9d-481f-9aef-0d3d0a05a03c')
INSERT [dbo].[UserTag] ([UserId], [TagId]) VALUES (N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'30094d14-0861-4faf-83ab-2336dfe2187f')
/****** Object:  Table [dbo].[Story]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Story](
	[Id] [uniqueidentifier] NOT NULL,
	[UniqueName] [nvarchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
	[Title] [nvarchar](256) COLLATE Latin1_General_CI_AS NOT NULL,
	[HtmlDescription] [nvarchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[TextDescription] [nvarchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[Url] [nvarchar](2048) COLLATE Latin1_General_CI_AS NOT NULL,
	[UrlHash] [nchar](24) COLLATE Latin1_General_CI_AS NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[IPAddress] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastActivityAt] [datetime] NOT NULL,
	[ApprovedAt] [datetime] NULL,
	[PublishedAt] [datetime] NULL,
	[Rank] [int] NULL,
	[LastProcessedAt] [datetime] NULL,
 CONSTRAINT [PK_Story] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND name = N'IX_Story_ApprovedAt_PublishedAt_Rank_CreatedAt_LastActivityAt')
CREATE NONCLUSTERED INDEX [IX_Story_ApprovedAt_PublishedAt_Rank_CreatedAt_LastActivityAt] ON [dbo].[Story] 
(
	[ApprovedAt] DESC,
	[PublishedAt] DESC,
	[Rank] ASC,
	[CreatedAt] DESC,
	[LastActivityAt] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND name = N'IX_Story_CategoryId')
CREATE NONCLUSTERED INDEX [IX_Story_CategoryId] ON [dbo].[Story] 
(
	[CategoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND name = N'IX_Story_LastProcessedAt')
CREATE NONCLUSTERED INDEX [IX_Story_LastProcessedAt] ON [dbo].[Story] 
(
	[LastProcessedAt] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND name = N'IX_Story_UniqueName')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Story_UniqueName] ON [dbo].[Story] 
(
	[UniqueName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND name = N'IX_Story_UrlHash')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Story_UrlHash] ON [dbo].[Story] 
(
	[UrlHash] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Story]') AND name = N'IX_Story_UserId')
CREATE NONCLUSTERED INDEX [IX_Story_UserId] ON [dbo].[Story] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Story]'))
CREATE FULLTEXT INDEX ON [dbo].[Story](
[TextDescription] LANGUAGE [English], 
[Title] LANGUAGE [English])
KEY INDEX [PK_Story] ON [StoryComment]
WITH CHANGE_TRACKING AUTO
GO
INSERT [dbo].[Story] ([Id], [UniqueName], [Title], [HtmlDescription], [TextDescription], [Url], [UrlHash], [CategoryId], [UserId], [IPAddress], [CreatedAt], [LastActivityAt], [ApprovedAt], [PublishedAt], [Rank], [LastProcessedAt]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'KiGG-is-now-upgraded-to-ASPNET-MVC-RTM-10-Kazi-Manzur-Rashids-Blog', N'KiGG is now upgraded to ASP.NET MVC RTM 1.0 - Kazi Manzur Rashid''s Blog', N'<p>Tuesday, April 07, 2009 7:02 PM
            kazimanzurrashid
                KiGG is now upgraded to ASP.NET MVC RTM 1.0</p>

<p>Just to let you know that I have uploaded the latest source of KiGG in Codeplex. Other than upgrading to ASP.NET MVC RTM, there are few enhancements:Implementing EventAggregator. Background Services like:      Broadcast in Twitter. Ping different Feed Servers automatically. Restrict Story submit from specific domains. SQL Server Full Text search. Othe...</p>', N'Tuesday, April 07, 2009 7:02 PM
            kazimanzurrashid
                KiGG is now upgraded to ASP.NET MVC RTM 1.0

Just to let you know that I have uploaded the latest source of KiGG in Codeplex. Other than upgrading to ASP.NET MVC RTM, there are few enhancements:Implementing EventAggregator. Background Services like:      Broadcast in Twitter. Ping different Feed Servers automatically. Restrict Story submit from specific domains. SQL Server Full Text search. Othe...', N'http://weblogs.asp.net/rashid/archive/2009/04/07/kigg-is-now-upgraded-to-asp-net-mvc-rtm-1-0.aspx', N'6n6aEvGj0cddRr62cI9WuQ==', N'7c113ed9-47a4-44f1-bdee-1cc86c02ce07', N'be1c680a-459c-455d-801e-46eea999903a', N'::1', CAST(0x00009BEB0169B60C AS DateTime), CAST(0x00009BEF002A766A AS DateTime), NULL, NULL, NULL, NULL)
INSERT [dbo].[Story] ([Id], [UniqueName], [Title], [HtmlDescription], [TextDescription], [Url], [UrlHash], [CategoryId], [UserId], [IPAddress], [CreatedAt], [LastActivityAt], [ApprovedAt], [PublishedAt], [Rank], [LastProcessedAt]) VALUES (N'f7f2d355-f5e7-4aa3-a7e8-1282d6c0ccd0', N'ASPNET-MVC-Best-Practices-Part-2-Kazi-Manzur-Rashids-Blog', N'ASP.NET MVC Best Practices (Part 2) - Kazi Manzur Rashid''s Blog', N'<p>Friday, April 03, 2009 1:39 PM
            kazimanzurrashid
                ASP.NET MVC Best Practices (Part 2)</p>

<p>This is the second part of the series and may be the last, till I find some thing new. My plan was to start with routing, controller, controller to model, controller to view and last of all the view, but some how I missed one important thing in routing, so I will begin with that in this post.15. Routing consideration
If you are developing a pure ASP.NET MVC a...</p>', N'Friday, April 03, 2009 1:39 PM
            kazimanzurrashid
                ASP.NET MVC Best Practices (Part 2)

This is the second part of the series and may be the last, till I find some thing new. My plan was to start with routing, controller, controller to model, controller to view and last of all the view, but some how I missed one important thing in routing, so I will begin with that in this post.15. Routing consideration
If you are developing a pure ASP.NET MVC a...', N'http://weblogs.asp.net/rashid/archive/2009/04/03/asp-net-mvc-best-practices-part-2.aspx', N'Y6su7LFAVnKcebHAyYeV/g==', N'348cc0ce-f578-4fc9-a617-465cfc545baf', N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'::1', CAST(0x00009BEB016C9D04 AS DateTime), CAST(0x00009BEB016E9BD0 AS DateTime), NULL, NULL, NULL, NULL)
INSERT [dbo].[Story] ([Id], [UniqueName], [Title], [HtmlDescription], [TextDescription], [Url], [UrlHash], [CategoryId], [UserId], [IPAddress], [CreatedAt], [LastActivityAt], [ApprovedAt], [PublishedAt], [Rank], [LastProcessedAt]) VALUES (N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'Enterprise-Library-41-Hands-on-Labs', N'Enterprise Library 4.1 Hands-on-Labs', N'<p>Last month (March 2009)  HOLs for Enterprise Library 4.1 released. Earlier this year there were HOLs released for Validation Application Block 4.1, now a full HOLs is released for the entire Enterprise Library.
Hands-on Labs walk you through the key usage scenarios of the application blocks in in various application contexts. You can practice the labs from start to finish or you can use the starter solutions provided to complete only the labs you want to, in the order you prefer.Enterprise Library for ....</p>', N'Last month (March 2009)  HOLs for Enterprise Library 4.1 released. Earlier this year there were HOLs released for Validation Application Block 4.1, now a full HOLs is released for the entire Enterprise Library.
Hands-on Labs walk you through the key usage scenarios of the application blocks in in various application contexts. You can practice the labs from start to finish or you can use the starter solutions provided to complete only the labs you want to, in the order you prefer.Enterprise Library for ....', N'http://mosesofegypt.net/post/EntLib41HOLsReleased.aspx', N'H5vLoBPSWGdfHMz15HHljA==', N'7c113ed9-47a4-44f1-bdee-1cc86c02ce07', N'332ea6ae-ddd7-425e-9186-d4d9d894388a', N'127.0.0.1', CAST(0x00009BE7015FEA45 AS DateTime), CAST(0x00009BF0016E524F AS DateTime), NULL, NULL, 1, NULL)
/****** Object:  Table [dbo].[StoryTag]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoryTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoryTag](
	[StoryId] [uniqueidentifier] NOT NULL,
	[TagId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StoryTag] PRIMARY KEY CLUSTERED 
(
	[StoryId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'30094d14-0861-4faf-83ab-2336dfe2187f')
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'ffafcda0-f25a-42ce-8eb6-5f7a8ee31bb6')
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'a2f52aef-cccb-4603-aa36-8f28dc18eb8c')
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'f7f2d355-f5e7-4aa3-a7e8-1282d6c0ccd0', N'1a7bc711-bb9d-481f-9aef-0d3d0a05a03c')
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'f7f2d355-f5e7-4aa3-a7e8-1282d6c0ccd0', N'30094d14-0861-4faf-83ab-2336dfe2187f')
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'f7f2d355-f5e7-4aa3-a7e8-1282d6c0ccd0', N'ffafcda0-f25a-42ce-8eb6-5f7a8ee31bb6')
INSERT [dbo].[StoryTag] ([StoryId], [TagId]) VALUES (N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'26e317c1-1cc8-49ae-8832-00d63c4c2fbd')
/****** Object:  Table [dbo].[StoryView]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoryView]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoryView](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[StoryId] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[IPAddress] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_View] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoryView]') AND name = N'IX_StoryView_StoryId_Timestamp_IpAddress')
CREATE CLUSTERED INDEX [IX_StoryView_StoryId_Timestamp_IpAddress] ON [dbo].[StoryView] 
(
	[StoryId] ASC,
	[Timestamp] DESC,
	[IPAddress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET IDENTITY_INSERT [dbo].[StoryView] ON
INSERT [dbo].[StoryView] ([Id], [StoryId], [Timestamp], [IPAddress]) VALUES (2, N'85ec5544-af6e-4177-8046-5d7e7a0c7205', CAST(0x00009BEB001A8EFD AS DateTime), N'127.0.0.1')
INSERT [dbo].[StoryView] ([Id], [StoryId], [Timestamp], [IPAddress]) VALUES (3, N'85ec5544-af6e-4177-8046-5d7e7a0c7205', CAST(0x00009BEB001A9D41 AS DateTime), N'127.0.0.1')
INSERT [dbo].[StoryView] ([Id], [StoryId], [Timestamp], [IPAddress]) VALUES (4, N'85ec5544-af6e-4177-8046-5d7e7a0c7205', CAST(0x00009BEC00029ACA AS DateTime), N'::1')
INSERT [dbo].[StoryView] ([Id], [StoryId], [Timestamp], [IPAddress]) VALUES (5, N'85ec5544-af6e-4177-8046-5d7e7a0c7205', CAST(0x00009BEF0029BCB2 AS DateTime), N'::1')
SET IDENTITY_INSERT [dbo].[StoryView] OFF
/****** Object:  Table [dbo].[StoryMarkAsSpam]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoryMarkAsSpam]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoryMarkAsSpam](
	[StoryId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[IPAddress] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_StoryMarkAsSpam] PRIMARY KEY CLUSTERED 
(
	[StoryId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[CommentSubscribtion]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommentSubscribtion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommentSubscribtion](
	[StoryId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CommentSubscribtion] PRIMARY KEY CLUSTERED 
(
	[StoryId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[StoryVote]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoryVote]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoryVote](
	[StoryId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[IPAddress] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_StoryVote] PRIMARY KEY CLUSTERED 
(
	[StoryId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'be1c680a-459c-455d-801e-46eea999903a', N'127.0.0.1', CAST(0x00009BEB0169B632 AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'332ea6ae-ddd7-425e-9186-d4d9d894388a', N'127.0.0.1', CAST(0x00009BEB016E9D2C AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'127.0.0.1', CAST(0x00009BEB016C3C5B AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'f7f2d355-f5e7-4aa3-a7e8-1282d6c0ccd0', N'332ea6ae-ddd7-425e-9186-d4d9d894388a', N'127.0.0.1', CAST(0x00009BEB016E9BD0 AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'f7f2d355-f5e7-4aa3-a7e8-1282d6c0ccd0', N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'127.0.0.1', CAST(0x00009BEB016C9E2C AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'be1c680a-459c-455d-801e-46eea999903a', N'127.0.0.1', CAST(0x00009BEC017AE57E AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'332ea6ae-ddd7-425e-9186-d4d9d894388a', N'127.0.0.1', CAST(0x00009BE7015FEA45 AS DateTime))
INSERT [dbo].[StoryVote] ([StoryId], [UserId], [IPAddress], [Timestamp]) VALUES (N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'66b3ab1c-e00a-44c6-af9a-e13dada0f565', N'127.0.0.1', CAST(0x00009BF0016E524F AS DateTime))
/****** Object:  Table [dbo].[StoryComment]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoryComment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StoryComment](
	[Id] [uniqueidentifier] NOT NULL,
	[HtmlBody] [nvarchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[TextBody] [nvarchar](max) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[StoryId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[IPAddress] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsOffended] [bit] NOT NULL,
 CONSTRAINT [PK_StoryComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoryComment]') AND name = N'IX_StoryComment_StoryId_CreatedAt')
CREATE NONCLUSTERED INDEX [IX_StoryComment_StoryId_CreatedAt] ON [dbo].[StoryComment] 
(
	[StoryId] ASC,
	[CreatedAt] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StoryComment]') AND name = N'IX_StoryComment_UserId')
CREATE NONCLUSTERED INDEX [IX_StoryComment_UserId] ON [dbo].[StoryComment] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[StoryComment]'))
CREATE FULLTEXT INDEX ON [dbo].[StoryComment](
[TextBody] LANGUAGE [English])
KEY INDEX [PK_StoryComment] ON [StoryComment]
WITH CHANGE_TRACKING AUTO
GO
INSERT [dbo].[StoryComment] ([Id], [HtmlBody], [TextBody], [CreatedAt], [StoryId], [UserId], [IPAddress], [IsOffended]) VALUES (N'6805bed2-49ac-4fbf-9b58-61e7d36c63e1', N'<p>This is my personal comment</p>', N'This is my personal comment', CAST(0x00009BEF002A766A AS DateTime), N'9bc8c5a1-b1f4-4d68-b433-05abd5295153', N'be1c680a-459c-455d-801e-46eea999903a', N'::1', 0)
INSERT [dbo].[StoryComment] ([Id], [HtmlBody], [TextBody], [CreatedAt], [StoryId], [UserId], [IPAddress], [IsOffended]) VALUES (N'aa5f1fc2-8684-4258-8552-acdc3d139246', N'<p>another hello comment</p>', N'another hello comment', CAST(0x00009BED0012EE7B AS DateTime), N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'be1c680a-459c-455d-801e-46eea999903a', N'::1', 0)
INSERT [dbo].[StoryComment] ([Id], [HtmlBody], [TextBody], [CreatedAt], [StoryId], [UserId], [IPAddress], [IsOffended]) VALUES (N'62f08cc5-1c43-4b6b-a54a-c4c53e60159f', N'<p>hello how are you?</p>', N'hello how are you?', CAST(0x00009BEF0028CA93 AS DateTime), N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'be1c680a-459c-455d-801e-46eea999903a', N'::1', 0)
INSERT [dbo].[StoryComment] ([Id], [HtmlBody], [TextBody], [CreatedAt], [StoryId], [UserId], [IPAddress], [IsOffended]) VALUES (N'd921847b-b927-4803-8e54-fd348587a2af', N'<p>hello comment</p>', N'hello comment', CAST(0x00009BED0011F234 AS DateTime), N'85ec5544-af6e-4177-8046-5d7e7a0c7205', N'be1c680a-459c-455d-801e-46eea999903a', N'::1', 0)
/****** Object:  Table [dbo].[UserScore]    Script Date: 05/15/2009 03:48:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserScore]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserScore](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[ActionType] [int] NOT NULL,
	[Score] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_UserScore] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UserScore]') AND name = N'IX_UserScore_UserId_Timestamp_Score')
CREATE CLUSTERED INDEX [IX_UserScore_UserId_Timestamp_Score] ON [dbo].[UserScore] 
(
	[UserId] ASC,
	[Timestamp] DESC,
	[Score] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
/****** Object:  UserDefinedFunction [dbo].[StorySearch]    Script Date: 05/15/2009 03:48:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StorySearch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[StorySearch]
      (@query nvarchar(4000))
returns table
as
  return (
			select distinct [Id]
			from containstable(Story,(Title, TextDescription), @query) as ft
			join [Story]
			on [Story].[ID] = ft.[KEY]
		 )
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[EFStorySearch]    Script Date: 05/15/2009 03:48:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EFStorySearch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[EFStorySearch]
      (
		@storyId uniqueidentifier,
		@query nvarchar(4000)
      )
returns bit
as
BEGIN
			if (EXISTS(select [Id]
						from containstable(Story,(Title, TextDescription), @query) as ft
						join [Story] on [Story].[ID] = ft.[KEY]
						where [Story].[ID] = @storyId))
			BEGIN
				return 1
			END			
				return 0
			
		 
END' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[EFCommentSearch]    Script Date: 05/15/2009 03:48:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EFCommentSearch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[EFCommentSearch]
      (
		@storyId uniqueidentifier,
		@query nvarchar(4000)
      )
returns bit
as
BEGIN
			if (EXISTS(select [StoryId]
					   from containstable(StoryComment,(TextBody), @query) as ft
					   join [StoryComment] on [StoryComment].[ID] = ft.[KEY]
					   where [StoryComment].[StoryId] = @storyId))
			BEGIN
				return 1
			END			
				
				return 0
			
		 
END' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[CommentSearch]    Script Date: 05/15/2009 03:48:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommentSearch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'create function [dbo].[CommentSearch]
      (@query nvarchar(4000))
returns table
as
  return (
			select distinct [StoryId]
			from containstable(StoryComment,(TextBody), @query) as ft
			join [StoryComment]
			on [StoryComment].[ID] = ft.[KEY]
		 )
' 
END
GO
/****** Object:  ForeignKey [FK_CommentSubscribtion_Story]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CommentSubscribtion_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommentSubscribtion]'))
ALTER TABLE [dbo].[CommentSubscribtion]  WITH CHECK ADD  CONSTRAINT [FK_CommentSubscribtion_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CommentSubscribtion_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommentSubscribtion]'))
ALTER TABLE [dbo].[CommentSubscribtion] CHECK CONSTRAINT [FK_CommentSubscribtion_Story]
GO
/****** Object:  ForeignKey [FK_CommentSubscribtion_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CommentSubscribtion_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommentSubscribtion]'))
ALTER TABLE [dbo].[CommentSubscribtion]  WITH CHECK ADD  CONSTRAINT [FK_CommentSubscribtion_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CommentSubscribtion_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[CommentSubscribtion]'))
ALTER TABLE [dbo].[CommentSubscribtion] CHECK CONSTRAINT [FK_CommentSubscribtion_User]
GO
/****** Object:  ForeignKey [FK_Story_Category]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story]  WITH CHECK ADD  CONSTRAINT [FK_Story_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story] CHECK CONSTRAINT [FK_Story_Category]
GO
/****** Object:  ForeignKey [FK_Story_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story]  WITH CHECK ADD  CONSTRAINT [FK_Story_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Story_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Story]'))
ALTER TABLE [dbo].[Story] CHECK CONSTRAINT [FK_Story_User]
GO
/****** Object:  ForeignKey [FK_StoryComment_Story]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryComment_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryComment]'))
ALTER TABLE [dbo].[StoryComment]  WITH CHECK ADD  CONSTRAINT [FK_StoryComment_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryComment_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryComment]'))
ALTER TABLE [dbo].[StoryComment] CHECK CONSTRAINT [FK_StoryComment_Story]
GO
/****** Object:  ForeignKey [FK_StoryComment_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryComment_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryComment]'))
ALTER TABLE [dbo].[StoryComment]  WITH CHECK ADD  CONSTRAINT [FK_StoryComment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryComment_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryComment]'))
ALTER TABLE [dbo].[StoryComment] CHECK CONSTRAINT [FK_StoryComment_User]
GO
/****** Object:  ForeignKey [FK_StoryMarkAsSpam_Story]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryMarkAsSpam_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryMarkAsSpam]'))
ALTER TABLE [dbo].[StoryMarkAsSpam]  WITH CHECK ADD  CONSTRAINT [FK_StoryMarkAsSpam_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryMarkAsSpam_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryMarkAsSpam]'))
ALTER TABLE [dbo].[StoryMarkAsSpam] CHECK CONSTRAINT [FK_StoryMarkAsSpam_Story]
GO
/****** Object:  ForeignKey [FK_StoryMarkAsSpam_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryMarkAsSpam_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryMarkAsSpam]'))
ALTER TABLE [dbo].[StoryMarkAsSpam]  WITH CHECK ADD  CONSTRAINT [FK_StoryMarkAsSpam_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryMarkAsSpam_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryMarkAsSpam]'))
ALTER TABLE [dbo].[StoryMarkAsSpam] CHECK CONSTRAINT [FK_StoryMarkAsSpam_User]
GO
/****** Object:  ForeignKey [FK_StoryTag_Story]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryTag_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryTag]'))
ALTER TABLE [dbo].[StoryTag]  WITH CHECK ADD  CONSTRAINT [FK_StoryTag_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryTag_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryTag]'))
ALTER TABLE [dbo].[StoryTag] CHECK CONSTRAINT [FK_StoryTag_Story]
GO
/****** Object:  ForeignKey [FK_StoryTag_Tag]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryTag_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryTag]'))
ALTER TABLE [dbo].[StoryTag]  WITH CHECK ADD  CONSTRAINT [FK_StoryTag_Tag] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryTag_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryTag]'))
ALTER TABLE [dbo].[StoryTag] CHECK CONSTRAINT [FK_StoryTag_Tag]
GO
/****** Object:  ForeignKey [FK_StoryView_Story]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryView_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryView]'))
ALTER TABLE [dbo].[StoryView]  WITH CHECK ADD  CONSTRAINT [FK_StoryView_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryView_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryView]'))
ALTER TABLE [dbo].[StoryView] CHECK CONSTRAINT [FK_StoryView_Story]
GO
/****** Object:  ForeignKey [FK_StoryVote_Story]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryVote_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryVote]'))
ALTER TABLE [dbo].[StoryVote]  WITH CHECK ADD  CONSTRAINT [FK_StoryVote_Story] FOREIGN KEY([StoryId])
REFERENCES [dbo].[Story] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryVote_Story]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryVote]'))
ALTER TABLE [dbo].[StoryVote] CHECK CONSTRAINT [FK_StoryVote_Story]
GO
/****** Object:  ForeignKey [FK_StoryVote_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryVote_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryVote]'))
ALTER TABLE [dbo].[StoryVote]  WITH CHECK ADD  CONSTRAINT [FK_StoryVote_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoryVote_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoryVote]'))
ALTER TABLE [dbo].[StoryVote] CHECK CONSTRAINT [FK_StoryVote_User]
GO
/****** Object:  ForeignKey [FK_UserScore_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserScore_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserScore]'))
ALTER TABLE [dbo].[UserScore]  WITH CHECK ADD  CONSTRAINT [FK_UserScore_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserScore_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserScore]'))
ALTER TABLE [dbo].[UserScore] CHECK CONSTRAINT [FK_UserScore_User]
GO
/****** Object:  ForeignKey [FK_UserTag_Tag]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserTag_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserTag]'))
ALTER TABLE [dbo].[UserTag]  WITH CHECK ADD  CONSTRAINT [FK_UserTag_Tag] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserTag_Tag]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserTag]'))
ALTER TABLE [dbo].[UserTag] CHECK CONSTRAINT [FK_UserTag_Tag]
GO
/****** Object:  ForeignKey [FK_UserTag_User]    Script Date: 05/15/2009 03:48:00 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserTag_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserTag]'))
ALTER TABLE [dbo].[UserTag]  WITH CHECK ADD  CONSTRAINT [FK_UserTag_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserTag_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserTag]'))
ALTER TABLE [dbo].[UserTag] CHECK CONSTRAINT [FK_UserTag_User]
GO
