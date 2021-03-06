USE [Assignment]
GO
/****** Object:  Table [dbo].[TBL_Transaction]    Script Date: 05/26/2022 00:17:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [nvarchar](50) NULL,
	[TransactionDate] [datetime] NULL,
	[Amount] [decimal](18, 2) NULL,
	[CurrencyCode] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_TBL_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TBL_Transaction] ON
INSERT [dbo].[TBL_Transaction] ([TransactionId], [Id], [TransactionDate], [Amount], [CurrencyCode], [Status]) VALUES (1, N'Invoice0000001', CAST(0x0000A9FA00092310 AS DateTime), CAST(1000.00 AS Decimal(18, 2)), N'USD', N'Approved')
INSERT [dbo].[TBL_Transaction] ([TransactionId], [Id], [TransactionDate], [Amount], [CurrencyCode], [Status]) VALUES (2, N'Invoice0000002', CAST(0x0000A9FB002253E4 AS DateTime), CAST(300.00 AS Decimal(18, 2)), N'USD', N'Failed')
INSERT [dbo].[TBL_Transaction] ([TransactionId], [Id], [TransactionDate], [Amount], [CurrencyCode], [Status]) VALUES (3, N'Inv00001', CAST(0x0000A9DE00E2A388 AS DateTime), CAST(200.00 AS Decimal(18, 2)), N'USD', N'Done')
INSERT [dbo].[TBL_Transaction] ([TransactionId], [Id], [TransactionDate], [Amount], [CurrencyCode], [Status]) VALUES (4, N'Inv00002', CAST(0x0000A9DF010A3664 AS DateTime), CAST(10000.00 AS Decimal(18, 2)), N'EUR', N'Rejected')
SET IDENTITY_INSERT [dbo].[TBL_Transaction] OFF
