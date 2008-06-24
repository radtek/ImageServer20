USE [ImageServer]
GO
/****** Object:  Table [dbo].[ServerTransferSyntax]    Script Date: 01/09/2008 15:04:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerTransferSyntax]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerTransferSyntax](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerTransferSyntax_GUID]  DEFAULT (newid()),
	[Uid] [varchar](64) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Lossless] [bit] NOT NULL,
 CONSTRAINT [PK_ServerTransferSyntax] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DuplicateSopPolicyEnum]    Script Date: 04/29/2008 14:56:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DuplicateSopPolicyEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DuplicateSopPolicyEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_DuplicateSopPolicyEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_DuplicateSopPolicyEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerPartition]    Script Date: 01/09/2008 15:03:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerPartition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerPartition](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerPartition_GUID]  DEFAULT (newid()),
	[Enabled] [bit] NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
	[AeTitle] [varchar](16) NOT NULL,
	[Port] [int] NOT NULL,
	[PartitionFolder] [nvarchar](16) NOT NULL,
	[AcceptAnyDevice] [bit] NOT NULL CONSTRAINT [DF_ServerPartition_AcceptAnyDevice]  DEFAULT ((1)),
	[AutoInsertDevice] [bit] NOT NULL CONSTRAINT [DF_ServerPartition_AutoInsertDevice]  DEFAULT ((1)),
	[DefaultRemotePort] [int] NOT NULL CONSTRAINT [DF_ServerPartition_DefaultRemotePort]  DEFAULT ((104)),
	[StudyCount] [int] NOT NULL CONSTRAINT [DF_ServerPartition_StudyCount]  DEFAULT ((0)),
	[DuplicateSopPolicyEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_ServerPartition] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ServerPartition]') AND name = N'IX_ServerPartition')
CREATE UNIQUE NONCLUSTERED INDEX [IX_ServerPartition] ON [dbo].[ServerPartition] 
(
	[AeTitle] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [INDEXES]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkQueueStatusEnum]    Script Date: 01/09/2008 15:04:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueueStatusEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueueStatusEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueueStatusEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_WorkQueueStatusEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[StudyStatusEnum]    Script Date: 01/09/2008 15:04:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudyStatusEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StudyStatusEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StudyStatusEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_StudyStatusEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WorkQueueTypeEnum]    Script Date: 01/09/2008 15:04:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueueTypeEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueueTypeEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueueTypeEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_WorkQueueTypeEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerSopClass]    Script Date: 01/09/2008 15:04:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerSopClass]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerSopClass](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_SopClass_GUID]  DEFAULT (newid()),
	[SopClassUid] [varchar](64) NOT NULL,
	[Description] [nvarchar](128) NOT NULL,
	[NonImage] [bit] NOT NULL,
 CONSTRAINT [PK_SopClass] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ServerSopClass]') AND name = N'IX_SopClass_SopClassUid')
CREATE UNIQUE NONCLUSTERED INDEX [IX_SopClass_SopClassUid] ON [dbo].[ServerSopClass] 
(
	[SopClassUid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
GO
/****** Object:  Table [dbo].[ServiceLockTypeEnum]    Script Date: 01/09/2008 15:04:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceLockTypeEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServiceLockTypeEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServiceLockTypeEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_ServiceLockTypeEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerRuleTypeEnum]    Script Date: 01/09/2008 15:03:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerRuleTypeEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerRuleTypeEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerRuleTypeEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_ServerRuleTypeEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServerRuleApplyTimeEnum]    Script Date: 01/09/2008 15:03:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerRuleApplyTimeEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerRuleApplyTimeEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerRuleApplyTimeEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_ServerRuleApplyTimeEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FilesystemTierEnum]    Script Date: 01/09/2008 15:03:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FilesystemTierEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FilesystemTierEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_FilesystemTierEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_FilesystemTier] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FilesystemQueueTypeEnum]    Script Date: 01/09/2008 15:03:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FilesystemQueueTypeEnum]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FilesystemQueueTypeEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_FilesystemQueueTypeEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_FilesystemQueueTypeEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[WorkQueuePriorityEnum]    Script Date: 03/12/2008 14:30:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[[WorkQueuePriorityEnum]]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueuePriorityEnum](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueuePriorityEnum_GUID]  DEFAULT (newid()),
	[Enum] [smallint] NOT NULL,
	[Lookup] [varchar](32) NOT NULL,
	[Description] [nvarchar](32) NOT NULL,
	[LongDescription] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_WorkQueuePriorityEnum] PRIMARY KEY CLUSTERED 
(
	[Enum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DevicePreferredTransferSyntax]    Script Date: 01/09/2008 15:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DevicePreferredTransferSyntax](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_DevicePreferredTransferSyntax_GUID]  DEFAULT (newid()),
	[DeviceGUID] [uniqueidentifier] NOT NULL,
	[ServerSopClassGUID] [uniqueidentifier] NOT NULL,
	[ServerTransferSyntaxGUID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DevicePreferredTransferSyntax] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]') AND name = N'IX_DevicePreferredTransferSyntax')
CREATE CLUSTERED INDEX [IX_DevicePreferredTransferSyntax] ON [dbo].[DevicePreferredTransferSyntax] 
(
	[DeviceGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerRule]    Script Date: 01/09/2008 15:03:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerRule]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerRule](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServerRule_GUID]  DEFAULT (newid()),
	[RuleName] [nvarchar](128) NOT NULL,
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[ServerRuleTypeEnum] [smallint] NOT NULL,
	[ServerRuleApplyTimeEnum] [smallint] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[DefaultRule] [bit] NOT NULL,
	[ExemptRule] [bit] NOT NULL CONSTRAINT [DF_ServerRule_NotRule]  DEFAULT ((0)),
	[RuleXml] [xml] NOT NULL,
 CONSTRAINT [PK_ServerRule] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[WorkQueue]    Script Date: 01/09/2008 15:04:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueue](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueue_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[StudyStorageGUID] [uniqueidentifier] NOT NULL,
	[DeviceGUID] [uniqueidentifier] NULL,
	[WorkQueueTypeEnum] [smallint] NOT NULL,
	[WorkQueueStatusEnum] [smallint] NOT NULL, 
	[WorkQueuePriorityEnum] [smallint] NOT NULL CONSTRAINT [DF_WorkQueue_WorkQueuePriorityEnum]  DEFAULT ((200)),
	[ProcessorID] [varchar](256) NULL,
	[ExpirationTime] [datetime] NULL,
	[ScheduledTime] [datetime] NOT NULL,
	[InsertTime] [datetime] NOT NULL CONSTRAINT [DF_WorkQueue_InsertTime]  DEFAULT (getdate()),
	[FailureCount] [int] NOT NULL CONSTRAINT [DF_WorkQueue_FailureCount]  DEFAULT ((0)),
	[FailureDescription] [nvarchar](256) NULL,
	[Data] [xml] NULL,
 CONSTRAINT [PK_WorkQueue] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [QUEUES]
) ON [QUEUES]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueue]') AND name = N'IX_WorkQueue_ScheduledTime')
CREATE NONCLUSTERED INDEX [IX_WorkQueue_ScheduledTime] ON [dbo].[WorkQueue] 
(
	[ScheduledTime] ASC
)
INCLUDE ( [WorkQueueStatusEnum]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueue]') AND name = N'IX_WorkQueue_StudyStorageGUID')
CREATE NONCLUSTERED INDEX [IX_WorkQueue_StudyStorageGUID] ON [dbo].[WorkQueue] 
(
	[StudyStorageGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueue]') AND name = N'IX_WorkQueue_WorkQueuePriorityEnum')
CREATE NONCLUSTERED INDEX [IX_WorkQueue_WorkQueuePriorityEnum] ON [dbo].[WorkQueue] 
(
	[WorkQueuePriorityEnum] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [INDEXES]
GO

/****** Object:  Table [dbo].[Series]    Script Date: 01/09/2008 15:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Series](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Series_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[StudyGUID] [uniqueidentifier] NOT NULL,
	[SeriesInstanceUid] [varchar](64) NOT NULL,
	[Modality] [varchar](16) NOT NULL,
	[SeriesNumber] [varchar](12) NULL,
	[SeriesDescription] [nvarchar](64) NULL,
	[NumberOfSeriesRelatedInstances] [int] NOT NULL,
	[PerformedProcedureStepStartDate] [varchar](8) NULL,
	[PerformedProcedureStepStartTime] [varchar](16) NULL,
	[SourceApplicationEntityTitle] [varchar](16) NULL,
	[StudyStatusEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_Series] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = N'IX_Series_StudyGUID_SeriesInstanceUid')
CREATE UNIQUE CLUSTERED INDEX [IX_Series_StudyGUID_SeriesInstanceUid] ON [dbo].[Series] 
(
	[StudyGUID] ASC,
	[SeriesInstanceUid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Series]') AND name = N'IX_Series_Modality')
CREATE NONCLUSTERED INDEX [IX_Series_Modality] ON [dbo].[Series] 
(
	[Modality] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[Study]    Script Date: 01/09/2008 15:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Study](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Study_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[PatientGUID] [uniqueidentifier] NOT NULL,
	[SpecificCharacterSet] varchar(128) NULL,
	[StudyInstanceUid] [varchar](64) NOT NULL,
	[PatientsName] [nvarchar](64) NULL,
	[PatientId] [nvarchar](64) NULL,
	[PatientsBirthDate] [varchar](8) NULL,
	[PatientsSex] [varchar](2) NULL,
	[StudyDate] [varchar](8) NULL,
	[StudyTime] [varchar](16) NULL,
	[AccessionNumber] [nvarchar](16) NULL,
	[StudyId] [nvarchar](16) NULL,
	[StudyDescription] [nvarchar](64) NULL,
	[ReferringPhysiciansName] [nvarchar](64) NULL,
	[NumberOfStudyRelatedSeries] [int] NOT NULL,
	[NumberOfStudyRelatedInstances] [int] NOT NULL,
	[StudyStatusEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_Study] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_AccessionNumber')
CREATE NONCLUSTERED INDEX [IX_Study_AccessionNumber] ON [dbo].[Study] 
(
	[AccessionNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_PatientGUID')
CREATE NONCLUSTERED INDEX [IX_Study_PatientGUID] ON [dbo].[Study] 
(
	[PatientGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_PatientId')
CREATE NONCLUSTERED INDEX [IX_Study_PatientId] ON [dbo].[Study] 
(
	[PatientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_PatientsName')
CREATE NONCLUSTERED INDEX [IX_Study_PatientsName] ON [dbo].[Study] 
(
	[PatientsName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_StudyDate')
CREATE NONCLUSTERED INDEX [IX_Study_StudyDate] ON [dbo].[Study] 
(
	[StudyDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_StudyDescription')
CREATE NONCLUSTERED INDEX [IX_Study_StudyDescription] ON [dbo].[Study] 
(
	[StudyDescription] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Study]') AND name = N'IX_Study_StudyInstanceUid')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Study_StudyInstanceUid] ON [dbo].[Study] 
(
	[StudyInstanceUid] ASC,
	[ServerPartitionGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[PartitionSopClass]    Script Date: 01/09/2008 15:03:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PartitionSopClass]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PartitionSopClass](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_PartitionSopClass_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[ServerSopClassGUID] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_PartitionSopClass] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[StudyStorage]    Script Date: 01/09/2008 15:04:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StudyStorage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StudyStorage](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StudyStorage_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[StudyInstanceUid] [varchar](64) NOT NULL,
	[InsertTime] [datetime] NOT NULL CONSTRAINT [DF_StudyStorage_InsertTime]  DEFAULT (getdate()),
	[LastAccessedTime] [datetime] NOT NULL CONSTRAINT [DF_StudyStorage_LastAccessedTime]  DEFAULT (getdate()),
	[Lock] [bit] NOT NULL CONSTRAINT [DF_StudyStorage_Lock]  DEFAULT ((0)),
	[StudyStatusEnum] [smallint] NOT NULL,
 CONSTRAINT [PK_StudyStorage] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StudyStorage]') AND name = N'IX_StudyStorage_PartitionGUID_StudyInstanceUid')
CREATE UNIQUE NONCLUSTERED INDEX [IX_StudyStorage_PartitionGUID_StudyInstanceUid] ON [dbo].[StudyStorage] 
(
	[ServerPartitionGUID] ASC,
	[StudyInstanceUid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 01/09/2008 15:03:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patient]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Patient](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Patient_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[PatientsName] [nvarchar](64) NULL,
	[PatientId] [nvarchar](64) NULL,
	[IssuerOfPatientId] [nvarchar](64) NULL,
	[NumberOfPatientRelatedStudies] [int] NOT NULL,
	[NumberOfPatientRelatedSeries] [int] NOT NULL,
	[NumberOfPatientRelatedInstances] [int] NOT NULL,
	[SpecificCharacterSet] varchar(128) NULL
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Device]    Script Date: 04/23/2008 23:48:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Device](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Device_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[AeTitle] [varchar](16) NOT NULL,
	[IpAddress] [varchar](16) NULL,
	[Port] [int] NOT NULL,
	[Description] [nvarchar](256) NULL,
	[Dhcp] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[AllowStorage] [bit] NOT NULL CONSTRAINT [DF_Device_StorageFlag]  DEFAULT ((0)),
	[AllowRetrieve] [bit] NOT NULL CONSTRAINT [DF_Device_AllowRetrieve]  DEFAULT ((0)),
	[AllowQuery] [bit] NOT NULL CONSTRAINT [DF_Device_AllowQuery]  DEFAULT ((0)),
	[AllowAutoRoute] [bit] NOT NULL CONSTRAINT [DF_Device_AllowAutoRoute]  DEFAULT ((1)),
 CONSTRAINT [PK_Device] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FilesystemQueue]    Script Date: 01/09/2008 15:03:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FilesystemQueue]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FilesystemQueue](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_FilesystemQueue_GUID]  DEFAULT (newid()),
	[FilesystemQueueTypeEnum] [smallint] NOT NULL,
	[StudyStorageGUID] [uniqueidentifier] NOT NULL,
	[FilesystemGUID] [uniqueidentifier] NOT NULL,
	[ScheduledTime] [datetime] NOT NULL,
	[SeriesInstanceUid] [varchar](64) NULL,
	[QueueXml] [xml] NULL,
 CONSTRAINT [PK_FilesystemQueue] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [QUEUES]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FilesystemQueue]') AND name = N'IXC_FilesystemQueue')
CREATE CLUSTERED INDEX [IXC_FilesystemQueue] ON [dbo].[FilesystemQueue] 
(
	[FilesystemGUID] ASC,
	[ScheduledTime] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [QUEUES]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[FilesystemQueue]') AND name = N'IX_FilesystemQueue_StudyStorageGUID')
CREATE UNIQUE NONCLUSTERED INDEX [IX_FilesystemQueue_StudyStorageGUID] ON [dbo].[FilesystemQueue] 
(
	[StudyStorageGUID] ASC,
	[FilesystemQueueTypeEnum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[ServiceLock]    Script Date: 01/09/2008 15:04:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceLock]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServiceLock](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_ServiceLock_GUID]  DEFAULT (newid()),
	[ServiceLockTypeEnum] [smallint] NOT NULL,
	[ProcessorId] [varchar](256) NULL,
	[Lock] [bit] NOT NULL,
	[ScheduledTime] [datetime] NOT NULL,
	[FilesystemGUID] [uniqueidentifier] NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_ServiceLock_Enabled]  DEFAULT ((1)),
 CONSTRAINT [PK_ServiceLock] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StorageFilesystem]    Script Date: 01/09/2008 15:04:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StorageFilesystem](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StorageFilesystem_GUID]  DEFAULT (newid()),
	[StudyStorageGUID] [uniqueidentifier] NOT NULL,
	[FilesystemGUID] [uniqueidentifier] NOT NULL,
	[StudyFolder] [varchar](8) NOT NULL,
 CONSTRAINT [PK_StorageFilesystem] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]') AND name = N'IX_StorageFilesystem_StudyStorageGUID')
CREATE CLUSTERED INDEX [IX_StorageFilesystem_StudyStorageGUID] ON [dbo].[StorageFilesystem] 
(
	[StudyStorageGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]') AND name = N'IX_StorageFilesystem_FilesystemGUID')
CREATE NONCLUSTERED INDEX [IX_StorageFilesystem_FilesystemGUID] ON [dbo].[StorageFilesystem] 
(
	[FilesystemGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
GO
/****** Object:  Table [dbo].[WorkQueueUid]    Script Date: 01/09/2008 15:04:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueueUid]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkQueueUid](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WorkQueueInstance_GUID]  DEFAULT (newid()),
	[WorkQueueGUID] [uniqueidentifier] NOT NULL,
	[SeriesInstanceUid] [varchar](64) NULL,
	[SopInstanceUid] [varchar](64) NULL,
	[Failed] [bit] NOT NULL CONSTRAINT [DF_WorkQueueUid_Failed]  DEFAULT ((0)),
	[Duplicate] [bit] NOT NULL CONSTRAINT [DF_WorkQueueUid_Duplicate]  DEFAULT ((0)),
	[Extension] [varchar](10) NULL,
	[FailureCount] [smallint] NOT NULL CONSTRAINT [DF_WorkQueueUid_FailureCount]  DEFAULT ((0)),
 CONSTRAINT [PK_WorkQueueUid] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [QUEUES]
) ON [QUEUES]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkQueueUid]') AND name = N'IX_WorkQueueUid')
CREATE CLUSTERED INDEX [IX_WorkQueueUid] ON [dbo].[WorkQueueUid] 
(
	[WorkQueueGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 65) ON [QUEUES]
GO
/****** Object:  Table [dbo].[RequestAttributes]    Script Date: 01/09/2008 15:03:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequestAttributes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RequestAttributes](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_RequestAttribute_GUID]  DEFAULT (newid()),
	[SeriesGUID] [uniqueidentifier] NOT NULL,
	[RequestedProcedureId] [nvarchar](16) NULL,
	[ScheduledProcedureStepId] [nvarchar](16) NULL,
 CONSTRAINT [PK_RequestAttribute] PRIMARY KEY NONCLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [INDEXES]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RequestAttributes]') AND name = N'IX_RequestAttribute_SeriesGUID')
CREATE CLUSTERED INDEX [IX_RequestAttribute_SeriesGUID] ON [dbo].[RequestAttributes] 
(
	[SeriesGUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Filesystem]    Script Date: 01/09/2008 15:03:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Filesystem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Filesystem](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Filesystem_GUID]  DEFAULT (newid()),
	[FilesystemPath] [nvarchar](256) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[ReadOnly] [bit] NOT NULL,
	[WriteOnly] [bit] NOT NULL,
	[Description] [nvarchar](128) NULL,
	[FilesystemTierEnum] [smallint] NOT NULL,
	[LowWatermark] [decimal](8, 4) NOT NULL CONSTRAINT [DF_Filesystem_LowWatermark]  DEFAULT ((80.00)),
	[HighWatermark] [decimal](8, 4) NOT NULL CONSTRAINT [DF_Filesystem_HighWatermark]  DEFAULT ((90.00)),
 CONSTRAINT [PK_Filesystem] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [STATIC]
) ON [STATIC]
END
GO
/****** Object:  Table [dbo].[PartitionTransferSyntax]    Script Date: 06/24/2008 16:42:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PartitionTransferSyntax]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PartitionTransferSyntax](
	[GUID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_PartitionTransferSyntax_GUID]  DEFAULT (newid()),
	[ServerPartitionGUID] [uniqueidentifier] NOT NULL,
	[ServerTransferSyntaxGUID] [uniqueidentifier] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_PartitionTransferSyntax] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/****** Object:  ForeignKey [FK_Device_ServerPartition]    Script Date: 01/09/2008 15:03:26 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Device_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Device]'))
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_ServerPartition]
GO
/****** Object:  ForeignKey [FK_DevicePreferredTransferSyntax_Device]    Script Date: 01/09/2008 15:03:27 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DevicePreferredTransferSyntax_Device]') AND parent_object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]'))
ALTER TABLE [dbo].[DevicePreferredTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_DevicePreferredTransferSyntax_Device] FOREIGN KEY([DeviceGUID])
REFERENCES [dbo].[Device] ([GUID])
GO
ALTER TABLE [dbo].[DevicePreferredTransferSyntax] CHECK CONSTRAINT [FK_DevicePreferredTransferSyntax_Device]
GO
/****** Object:  ForeignKey [FK_DevicePreferredTransferSyntax_ServerSopClass]    Script Date: 01/09/2008 15:03:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DevicePreferredTransferSyntax_ServerSopClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]'))
ALTER TABLE [dbo].[DevicePreferredTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerSopClass] FOREIGN KEY([ServerSopClassGUID])
REFERENCES [dbo].[ServerSopClass] ([GUID])
GO
ALTER TABLE [dbo].[DevicePreferredTransferSyntax] CHECK CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerSopClass]
GO
/****** Object:  ForeignKey [FK_DevicePreferredTransferSyntax_ServerTransferSyntax]    Script Date: 01/09/2008 15:03:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DevicePreferredTransferSyntax_ServerTransferSyntax]') AND parent_object_id = OBJECT_ID(N'[dbo].[DevicePreferredTransferSyntax]'))
ALTER TABLE [dbo].[DevicePreferredTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerTransferSyntax] FOREIGN KEY([ServerTransferSyntaxGUID])
REFERENCES [dbo].[ServerTransferSyntax] ([GUID])
GO
ALTER TABLE [dbo].[DevicePreferredTransferSyntax] CHECK CONSTRAINT [FK_DevicePreferredTransferSyntax_ServerTransferSyntax]
GO
/****** Object:  ForeignKey [FK_Filesystem_FilesystemTierEnum]    Script Date: 01/09/2008 15:03:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Filesystem_FilesystemTierEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Filesystem]'))
ALTER TABLE [dbo].[Filesystem]  WITH CHECK ADD  CONSTRAINT [FK_Filesystem_FilesystemTierEnum] FOREIGN KEY([FilesystemTierEnum])
REFERENCES [dbo].[FilesystemTierEnum] ([Enum])
GO
ALTER TABLE [dbo].[Filesystem] CHECK CONSTRAINT [FK_Filesystem_FilesystemTierEnum]
GO
/****** Object:  ForeignKey [FK_FilesystemQueue_Filesystem]    Script Date: 01/09/2008 15:03:34 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FilesystemQueue_Filesystem]') AND parent_object_id = OBJECT_ID(N'[dbo].[FilesystemQueue]'))
ALTER TABLE [dbo].[FilesystemQueue]  WITH CHECK ADD  CONSTRAINT [FK_FilesystemQueue_Filesystem] FOREIGN KEY([FilesystemGUID])
REFERENCES [dbo].[Filesystem] ([GUID])
GO
ALTER TABLE [dbo].[FilesystemQueue] CHECK CONSTRAINT [FK_FilesystemQueue_Filesystem]
GO
/****** Object:  ForeignKey [FK_FilesystemQueue_FilesystemQueueTypeEnum]    Script Date: 01/09/2008 15:03:34 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FilesystemQueue_FilesystemQueueTypeEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[FilesystemQueue]'))
ALTER TABLE [dbo].[FilesystemQueue]  WITH CHECK ADD  CONSTRAINT [FK_FilesystemQueue_FilesystemQueueTypeEnum] FOREIGN KEY([FilesystemQueueTypeEnum])
REFERENCES [dbo].[FilesystemQueueTypeEnum] ([Enum])
GO
ALTER TABLE [dbo].[FilesystemQueue] CHECK CONSTRAINT [FK_FilesystemQueue_FilesystemQueueTypeEnum]
GO
/****** Object:  ForeignKey [FK_FilesystemQueue_StudyStorage]    Script Date: 01/09/2008 15:03:34 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FilesystemQueue_StudyStorage]') AND parent_object_id = OBJECT_ID(N'[dbo].[FilesystemQueue]'))
ALTER TABLE [dbo].[FilesystemQueue]  WITH CHECK ADD  CONSTRAINT [FK_FilesystemQueue_StudyStorage] FOREIGN KEY([StudyStorageGUID])
REFERENCES [dbo].[StudyStorage] ([GUID])
GO
ALTER TABLE [dbo].[FilesystemQueue] CHECK CONSTRAINT [FK_FilesystemQueue_StudyStorage]
GO
/****** Object:  ForeignKey [FK_PartitionSopClass_ServerPartition]    Script Date: 01/09/2008 15:03:39 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PartitionSopClass_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[PartitionSopClass]'))
ALTER TABLE [dbo].[PartitionSopClass]  WITH CHECK ADD  CONSTRAINT [FK_PartitionSopClass_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[PartitionSopClass] CHECK CONSTRAINT [FK_PartitionSopClass_ServerPartition]
GO
/****** Object:  ForeignKey [FK_PartitionSopClass_ServerSopClass]    Script Date: 01/09/2008 15:03:39 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PartitionSopClass_ServerSopClass]') AND parent_object_id = OBJECT_ID(N'[dbo].[PartitionSopClass]'))
ALTER TABLE [dbo].[PartitionSopClass]  WITH CHECK ADD  CONSTRAINT [FK_PartitionSopClass_ServerSopClass] FOREIGN KEY([ServerSopClassGUID])
REFERENCES [dbo].[ServerSopClass] ([GUID])
GO
ALTER TABLE [dbo].[PartitionSopClass] CHECK CONSTRAINT [FK_PartitionSopClass_ServerSopClass]
GO
/****** Object:  ForeignKey [FK_Patient_ServerPartition]    Script Date: 01/09/2008 15:03:42 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Patient_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Patient]'))
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_ServerPartition]
GO
/****** Object:  ForeignKey [FK_RequestAttribute_Series]    Script Date: 01/09/2008 15:03:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RequestAttribute_Series]') AND parent_object_id = OBJECT_ID(N'[dbo].[RequestAttributes]'))
ALTER TABLE [dbo].[RequestAttributes]  WITH CHECK ADD  CONSTRAINT [FK_RequestAttribute_Series] FOREIGN KEY([SeriesGUID])
REFERENCES [dbo].[Series] ([GUID])
GO
ALTER TABLE [dbo].[RequestAttributes] CHECK CONSTRAINT [FK_RequestAttribute_Series]
GO
/****** Object:  ForeignKey [FK_Series_ServerPartition]    Script Date: 01/09/2008 15:03:48 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Series_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Series]'))
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_ServerPartition]
GO
/****** Object:  ForeignKey [FK_Series_StatusEnum]    Script Date: 01/09/2008 15:03:48 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Series_StatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Series]'))
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_StatusEnum] FOREIGN KEY([StudyStatusEnum])
REFERENCES [dbo].[StudyStatusEnum] ([Enum])
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_StatusEnum]
GO
/****** Object:  ForeignKey [FK_Series_Study]    Script Date: 01/09/2008 15:03:48 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Series_Study]') AND parent_object_id = OBJECT_ID(N'[dbo].[Series]'))
ALTER TABLE [dbo].[Series]  WITH CHECK ADD  CONSTRAINT [FK_Series_Study] FOREIGN KEY([StudyGUID])
REFERENCES [dbo].[Study] ([GUID])
GO
ALTER TABLE [dbo].[Series] CHECK CONSTRAINT [FK_Series_Study]
GO
/****** Object:  ForeignKey [FK_ServerRule_ServerPartition]    Script Date: 01/09/2008 15:03:54 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerRule_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerRule]'))
ALTER TABLE [dbo].[ServerRule]  WITH CHECK ADD  CONSTRAINT [FK_ServerRule_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[ServerRule] CHECK CONSTRAINT [FK_ServerRule_ServerPartition]
GO
/****** Object:  ForeignKey [FK_ServerRule_ServerRuleApplyTimeEnum]    Script Date: 01/09/2008 15:03:54 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerRule_ServerRuleApplyTimeEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerRule]'))
ALTER TABLE [dbo].[ServerRule]  WITH CHECK ADD  CONSTRAINT [FK_ServerRule_ServerRuleApplyTimeEnum] FOREIGN KEY([ServerRuleApplyTimeEnum])
REFERENCES [dbo].[ServerRuleApplyTimeEnum] ([Enum])
GO
ALTER TABLE [dbo].[ServerRule] CHECK CONSTRAINT [FK_ServerRule_ServerRuleApplyTimeEnum]
GO
/****** Object:  ForeignKey [FK_ServerRule_ServerRuleTypeEnum]    Script Date: 01/09/2008 15:03:54 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerRule_ServerRuleTypeEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerRule]'))
ALTER TABLE [dbo].[ServerRule]  WITH CHECK ADD  CONSTRAINT [FK_ServerRule_ServerRuleTypeEnum] FOREIGN KEY([ServerRuleTypeEnum])
REFERENCES [dbo].[ServerRuleTypeEnum] ([Enum])
GO
ALTER TABLE [dbo].[ServerRule] CHECK CONSTRAINT [FK_ServerRule_ServerRuleTypeEnum]
GO
/****** Object:  ForeignKey [FK_ServiceLock_Filesystem]    Script Date: 01/09/2008 15:04:04 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServiceLock_Filesystem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServiceLock]'))
ALTER TABLE [dbo].[ServiceLock]  WITH CHECK ADD  CONSTRAINT [FK_ServiceLock_Filesystem] FOREIGN KEY([FilesystemGUID])
REFERENCES [dbo].[Filesystem] ([GUID])
GO
ALTER TABLE [dbo].[ServiceLock] CHECK CONSTRAINT [FK_ServiceLock_Filesystem]
GO
/****** Object:  ForeignKey [FK_ServiceLock_ServiceLockTypeEnum]    Script Date: 01/09/2008 15:04:04 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServiceLock_ServiceLockTypeEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServiceLock]'))
ALTER TABLE [dbo].[ServiceLock]  WITH CHECK ADD  CONSTRAINT [FK_ServiceLock_ServiceLockTypeEnum] FOREIGN KEY([ServiceLockTypeEnum])
REFERENCES [dbo].[ServiceLockTypeEnum] ([Enum])
GO
ALTER TABLE [dbo].[ServiceLock] CHECK CONSTRAINT [FK_ServiceLock_ServiceLockTypeEnum]
GO
/****** Object:  ForeignKey [FK_StorageFilesystem_Filesystem]    Script Date: 01/09/2008 15:04:07 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StorageFilesystem_Filesystem]') AND parent_object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]'))
ALTER TABLE [dbo].[StorageFilesystem]  WITH CHECK ADD  CONSTRAINT [FK_StorageFilesystem_Filesystem] FOREIGN KEY([FilesystemGUID])
REFERENCES [dbo].[Filesystem] ([GUID])
GO
ALTER TABLE [dbo].[StorageFilesystem] CHECK CONSTRAINT [FK_StorageFilesystem_Filesystem]
GO
/****** Object:  ForeignKey [FK_StorageFilesystem_StudyStorage]    Script Date: 01/09/2008 15:04:07 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StorageFilesystem_StudyStorage]') AND parent_object_id = OBJECT_ID(N'[dbo].[StorageFilesystem]'))
ALTER TABLE [dbo].[StorageFilesystem]  WITH CHECK ADD  CONSTRAINT [FK_StorageFilesystem_StudyStorage] FOREIGN KEY([StudyStorageGUID])
REFERENCES [dbo].[StudyStorage] ([GUID])
GO
ALTER TABLE [dbo].[StorageFilesystem] CHECK CONSTRAINT [FK_StorageFilesystem_StudyStorage]
GO
/****** Object:  ForeignKey [FK_Study_Patient]    Script Date: 01/09/2008 15:04:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Study_Patient]') AND parent_object_id = OBJECT_ID(N'[dbo].[Study]'))
ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_Patient] FOREIGN KEY([PatientGUID])
REFERENCES [dbo].[Patient] ([GUID])
GO
ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_Patient]
GO
/****** Object:  ForeignKey [FK_Study_ServerPartition]    Script Date: 01/09/2008 15:04:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Study_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[Study]'))
ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_ServerPartition]
GO
/****** Object:  ForeignKey [FK_Study_StatusEnum]    Script Date: 01/09/2008 15:04:14 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Study_StatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[Study]'))
ALTER TABLE [dbo].[Study]  WITH CHECK ADD  CONSTRAINT [FK_Study_StatusEnum] FOREIGN KEY([StudyStatusEnum])
REFERENCES [dbo].[StudyStatusEnum] ([Enum])
GO
ALTER TABLE [dbo].[Study] CHECK CONSTRAINT [FK_Study_StatusEnum]
GO
/****** Object:  ForeignKey [FK_StudyStorage_ServerPartition]    Script Date: 01/09/2008 15:04:19 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudyStorage_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudyStorage]'))
ALTER TABLE [dbo].[StudyStorage]  WITH CHECK ADD  CONSTRAINT [FK_StudyStorage_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[StudyStorage] CHECK CONSTRAINT [FK_StudyStorage_ServerPartition]
GO
/****** Object:  ForeignKey [FK_StudyStorage_StudyStatusEnum]    Script Date: 01/09/2008 15:04:20 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StudyStorage_StudyStatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[StudyStorage]'))
ALTER TABLE [dbo].[StudyStorage]  WITH CHECK ADD  CONSTRAINT [FK_StudyStorage_StudyStatusEnum] FOREIGN KEY([StudyStatusEnum])
REFERENCES [dbo].[StudyStatusEnum] ([Enum])
GO
ALTER TABLE [dbo].[StudyStorage] CHECK CONSTRAINT [FK_StudyStorage_StudyStatusEnum]
GO
/****** Object:  ForeignKey [FK_WorkQueue_Device]    Script Date: 01/09/2008 15:04:24 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_Device]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_Device] FOREIGN KEY([DeviceGUID])
REFERENCES [dbo].[Device] ([GUID])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_Device]
GO
/****** Object:  ForeignKey [FK_WorkQueue_ServerPartition]    Script Date: 01/09/2008 15:04:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_ServerPartition]
GO
/****** Object:  ForeignKey [FK_WorkQueue_StudyStorage]    Script Date: 01/09/2008 15:04:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_StudyStorage]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_StudyStorage] FOREIGN KEY([StudyStorageGUID])
REFERENCES [dbo].[StudyStorage] ([GUID])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_StudyStorage]
GO
/****** Object:  ForeignKey [FK_WorkQueue_WorkQueueStatusEnum]    Script Date: 01/09/2008 15:04:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_WorkQueueStatusEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_WorkQueueStatusEnum] FOREIGN KEY([WorkQueueStatusEnum])
REFERENCES [dbo].[WorkQueueStatusEnum] ([Enum])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_WorkQueueStatusEnum]
GO
/****** Object:  ForeignKey [FK_WorkQueue_WorkQueueTypeEnum]    Script Date: 01/09/2008 15:04:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_WorkQueueTypeEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_WorkQueueTypeEnum] FOREIGN KEY([WorkQueueTypeEnum])
REFERENCES [dbo].[WorkQueueTypeEnum] ([Enum])
GO
ALTER TABLE [dbo].[WorkQueue] CHECK CONSTRAINT [FK_WorkQueue_WorkQueueTypeEnum]
GO
/****** Object:  ForeignKey [FK_WorkQueue_WorkQueuePriorityEnum]    Script Date: 01/09/2008 15:04:30 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueue_WorkQueuePriorityEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueue]'))
ALTER TABLE [dbo].[WorkQueue]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueue_WorkQueuePriorityEnum] FOREIGN KEY([WorkQueuePriorityEnum])
REFERENCES [dbo].[WorkQueuePriorityEnum] ([Enum])
GO

/****** Object:  ForeignKey [FK_WorkQueueUid_WorkQueue]    Script Date: 01/09/2008 15:04:30 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WorkQueueUid_WorkQueue]') AND parent_object_id = OBJECT_ID(N'[dbo].[WorkQueueUid]'))
ALTER TABLE [dbo].[WorkQueueUid]  WITH CHECK ADD  CONSTRAINT [FK_WorkQueueUid_WorkQueue] FOREIGN KEY([WorkQueueGUID])
REFERENCES [dbo].[WorkQueue] ([GUID])
GO
ALTER TABLE [dbo].[WorkQueueUid] CHECK CONSTRAINT [FK_WorkQueueUid_WorkQueue]
GO

/****** Object:  ForeignKey [FK_ServerPartition_DuplicateSopPolicyEnum]    Script Date: 04/29/2008 14:56:18 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ServerPartition_DuplicateSopPolicyEnum]') AND parent_object_id = OBJECT_ID(N'[dbo].[ServerPartition]'))
ALTER TABLE [dbo].[ServerPartition]  WITH CHECK ADD  CONSTRAINT [FK_ServerPartition_DuplicateSopPolicyEnum] FOREIGN KEY([DuplicateSopPolicyEnum])
REFERENCES [dbo].[DuplicateSopPolicyEnum] ([Enum])
GO
ALTER TABLE [dbo].[ServerPartition] CHECK CONSTRAINT [FK_ServerPartition_DuplicateSopPolicyEnum]
GO

/****** Object:  ForeignKey [FK_PartitionTransferSyntax_ServerPartition]    Script Date: 06/24/2008 16:42:53 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PartitionTransferSyntax_ServerPartition]') AND parent_object_id = OBJECT_ID(N'[dbo].[PartitionTransferSyntax]'))
ALTER TABLE [dbo].[PartitionTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_PartitionTransferSyntax_ServerPartition] FOREIGN KEY([ServerPartitionGUID])
REFERENCES [dbo].[ServerPartition] ([GUID])
GO
ALTER TABLE [dbo].[PartitionTransferSyntax] CHECK CONSTRAINT [FK_PartitionTransferSyntax_ServerPartition]
GO
/****** Object:  ForeignKey [FK_PartitionTransferSyntax_ServerTransferSyntax]    Script Date: 06/24/2008 16:42:53 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PartitionTransferSyntax_ServerTransferSyntax]') AND parent_object_id = OBJECT_ID(N'[dbo].[PartitionTransferSyntax]'))
ALTER TABLE [dbo].[PartitionTransferSyntax]  WITH CHECK ADD  CONSTRAINT [FK_PartitionTransferSyntax_ServerTransferSyntax] FOREIGN KEY([ServerTransferSyntaxGUID])
REFERENCES [dbo].[ServerTransferSyntax] ([GUID])
GO
ALTER TABLE [dbo].[PartitionTransferSyntax] CHECK CONSTRAINT [FK_PartitionTransferSyntax_ServerTransferSyntax]
GO