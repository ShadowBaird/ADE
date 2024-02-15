USE [ADE]
GO

/****** Object:  Table [dbo].[Administradores]    Script Date: 14/02/2024 07:59:44 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Administradores](
	[id_admin] [int] IDENTITY(1,1) NOT NULL,
	[usuario_admin] [nchar](20) NOT NULL,
	[contra_admin] [nchar](100) NOT NULL,
	[id_salon] [int] NOT NULL,
 CONSTRAINT [PK_Administradores] PRIMARY KEY CLUSTERED 
(
	[id_admin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Administradores]  WITH NOCHECK ADD  CONSTRAINT [FK_Administradores_Salones] FOREIGN KEY([id_salon])
REFERENCES [dbo].[Salones] ([id_salon])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Administradores] CHECK CONSTRAINT [FK_Administradores_Salones]
GO

