CREATE TABLE [dbo].[AuditHistory]
(
	[Id] INT NOT NULL CONSTRAINT [PK_AuditHistory] PRIMARY KEY IDENTITY(1,1), 
    [UpdatedUtc] DATETIME NOT NULL, 
    [ReportingData] NVARCHAR(MAX) NOT NULL, 
    [UpdatedBy] NVARCHAR(MAX) NOT NULL, 
    [ReportId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_AuditHistory_Report] FOREIGN KEY (ReportId) REFERENCES [Report]([Id])
)
