USE [ADE]
GO

/****** Object:  Table [dbo].[Eventos]    Script Date: 14/02/2024 07:59:57 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Eventos](
	[id_evento] [int] IDENTITY(1,1) NOT NULL,
	[fecha_evento] [date] NOT NULL,
	[cantidad_personas_evento] [int] NOT NULL,
	[tipo_evento] [nchar](30) NOT NULL,
	[nombre_responsable_evento] [nchar](100) NOT NULL,
	[telefono_responsable_evento] [nchar](20) NOT NULL,
	[total_pagar_evento] [int] NOT NULL,
	[id_salon] [int] NOT NULL,
	[abonado_evento] [int] NOT NULL,
	[id_prereserva] [int] NULL,
 CONSTRAINT [PK_Eventos] PRIMARY KEY CLUSTERED 
(
	[id_evento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [index1] UNIQUE NONCLUSTERED 
(
	[id_salon] ASC,
	[fecha_evento] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Eventos] ADD  CONSTRAINT [DF_Eventos_abonado_evento]  DEFAULT ((0)) FOR [abonado_evento]
GO

ALTER TABLE [dbo].[Eventos]  WITH CHECK ADD  CONSTRAINT [FK_Eventos_Prereservas] FOREIGN KEY([id_prereserva])
REFERENCES [dbo].[Prereservas] ([id_prereserva])
GO

ALTER TABLE [dbo].[Eventos] CHECK CONSTRAINT [FK_Eventos_Prereservas]
GO

ALTER TABLE [dbo].[Eventos]  WITH CHECK ADD  CONSTRAINT [FK_Eventos_Salones] FOREIGN KEY([id_salon])
REFERENCES [dbo].[Salones] ([id_salon])
GO

ALTER TABLE [dbo].[Eventos] CHECK CONSTRAINT [FK_Eventos_Salones]
GO

