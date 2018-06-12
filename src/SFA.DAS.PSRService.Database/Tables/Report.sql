﻿CREATE TABLE [dbo].[Report]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(), 
    [EmployerId] NVARCHAR(50) NOT NULL, 
    [ReportingPeriod] NVARCHAR(4) NOT NULL, 
    [ReportingData] NVARCHAR(MAX) NOT NULL, 
    [Submitted] BIT NOT NULL DEFAULT 0, 
    [AuditWindowStartUtc] DATETIME NULL, 
    [UpdatedUtc] DATETIME NULL, 
    [UpdatedBy] NVARCHAR(MAX) NULL
)
GO
CREATE UNIQUE NONCLUSTERED INDEX [IXU_Report_EmployerIdReportingPeriod] ON [dbo].[Report]
(
    [EmployerId] ASC,
    [ReportingPeriod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
