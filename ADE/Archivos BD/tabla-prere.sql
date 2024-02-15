USE [ADE]
GO

/****** Object:  Table [dbo].[Prereservas]    Script Date: 14/02/2024 08:00:06 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Prereservas](
	[id_prereserva] [int] IDENTITY(1,1) NOT NULL,
	[id_salon] [int] NOT NULL,
	[id_usuario] [int] NOT NULL,
	[fecha_evento_prereserva] [date] NOT NULL,
	[estado_prereserva] [nchar](30) NOT NULL,
	[cantidad_personas_prereserva] [int] NOT NULL,
	[tipo_evento_prereserva] [nchar](30) NOT NULL,
	[fecha_registro_prereserva] [date] NOT NULL,
	[total_prereserva] [int] NULL,
 CONSTRAINT [PK_Prereservas] PRIMARY KEY CLUSTERED 
(
	[id_prereserva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Prereservas] ADD  CONSTRAINT [DF_Prereservas_total_prereserva]  DEFAULT ((0)) FOR [total_prereserva]
GO

ALTER TABLE [dbo].[Prereservas]  WITH CHECK ADD  CONSTRAINT [FK_Prereservas_Salones] FOREIGN KEY([id_salon])
REFERENCES [dbo].[Salones] ([id_salon])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Prereservas] CHECK CONSTRAINT [FK_Prereservas_Salones]
GO

ALTER TABLE [dbo].[Prereservas]  WITH CHECK ADD  CONSTRAINT [FK_Prereservas_Usuarios] FOREIGN KEY([id_usuario])
REFERENCES [dbo].[Usuarios] ([id_usuario])
GO

ALTER TABLE [dbo].[Prereservas] CHECK CONSTRAINT [FK_Prereservas_Usuarios]
GO

