USE [admPri]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 26/6/2023 9:23:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_name] [nvarchar](50) NOT NULL,
	[user_pass] [nvarchar](50) NOT NULL,
	[date_of_birth] [date] NOT NULL,
	[is_admin] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [user_name], [user_pass], [date_of_birth], [is_admin]) VALUES (1, N'Admin', N'admin123', CAST(N'1987-02-01' AS Date), 1)
INSERT [dbo].[Users] ([id], [user_name], [user_pass], [date_of_birth], [is_admin]) VALUES (2, N'nik', N'nik123', CAST(N'1976-09-18' AS Date), 0)
INSERT [dbo].[Users] ([id], [user_name], [user_pass], [date_of_birth], [is_admin]) VALUES (3, N'prov3', N'prov3', CAST(N'1965-05-02' AS Date), 0)
INSERT [dbo].[Users] ([id], [user_name], [user_pass], [date_of_birth], [is_admin]) VALUES (4, N'prov4', N'prov4', CAST(N'1999-04-18' AS Date), 0)
INSERT [dbo].[Users] ([id], [user_name], [user_pass], [date_of_birth], [is_admin]) VALUES (5, N'prov5', N'prov5', CAST(N'1987-06-05' AS Date), 0)

SET IDENTITY_INSERT [dbo].[Users] OFF
