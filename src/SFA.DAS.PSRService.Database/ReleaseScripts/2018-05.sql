-- https://skillsfundingagency.atlassian.net/browse/MPD-1405

MERGE [dbo].[Report] AS target
USING (
    SELECT ab.Employerid,
        ab.ReportingPeriod,
        ab.newReportingData
    FROM (
        SELECT a.Employerid,
            a.ReportingPeriod,
            a.FigureA,
            a.FigureB,
            FORMAT(ROUND(((a.FigureB) / CONVERT(DECIMAL, nullif(a.FigureA, 0))), 4), '######0.00%') AS FigureE,
            a.FigureC,
            a.FigureD,
            FORMAT(ROUND(((a.FigureD) / CONVERT(DECIMAL, nullif(a.FigureC, 0))), 4), '######0.00%') AS FigureF,
            a.FigureG,
            a.FigureH,
            FORMAT(ROUND(((a.FigureB) / CONVERT(DECIMAL, nullif(a.FigureH, 0))), 4), '######0.00%') AS FigureI,
            a.SubmittedAt,
            CASE 
				WHEN a.submitted = 0 OR JSON_VALUE([ReportingData], '$.ReportingPercentages[0].EmploymentStarts') IS NOT NULL 
				THEN '' 
				ELSE JSON_MODIFY([ReportingData], '$.ReportingPercentages', JSON_QUERY(N'{"EmploymentStarts":"' + FORMAT(ISNULL(ROUND(((100 * a.FigureB) / CONVERT(DECIMAL, nullif(a.FigureA, 0))), 4), 0), '######0.00') + '","TotalHeadCount":"' + FORMAT(ISNULL(ROUND(((100 * a.FigureD) / CONVERT(DECIMAL, nullif(a.FigureC, 0))), 4), 0), '######0.00') + '","NewThisPeriod":"' + FORMAT(ISNULL(ROUND(((100 * a.FigureB) / CONVERT(DECIMAL, nullif(a.FigureH, 0))), 4), 0), '######0.00') + '"}')) 
			END newReportingData
        FROM (
            SELECT [OrganisationName] AS Organisation_Name,
                [ReportingPeriodLabel] AS Reporting_Period,
                CAST(CASE WHEN base.Answer1_3 IS NULL THEN 0 ELSE base.Answer1_3 END AS FLOAT) AS FigureA,
                CAST(CASE WHEN base.Answer2_3 IS NULL THEN 0 ELSE base.Answer2_3 END AS FLOAT) AS FigureB,
                CAST(CASE WHEN base.Answer1_2 IS NULL THEN 0 ELSE base.Answer1_2 END AS FLOAT) AS FigureC,
                CAST(CASE WHEN base.Answer2_2 IS NULL THEN 0 ELSE base.Answer2_2 END AS FLOAT) AS FigureD,
                CAST(CASE WHEN base.Answer2_1 IS NULL THEN 0 ELSE base.Answer2_1 END AS FLOAT) AS FigureG,
                CAST(CASE WHEN base.Answer1_1 IS NULL THEN 0 ELSE base.Answer1_1 END AS FLOAT) AS FigureH,
                base.*
            FROM (
					SELECT [Employerid],
						[ReportingPeriod],
						[ReportingData],
						'1 April 20' + SUBSTRING([ReportingPeriod], 1, 2) + ' to 31 March 20' + SUBSTRING([ReportingPeriod], 3, 2) AS ReportingPeriodLabel,
						JSON_VALUE([ReportingData], '$.OrganisationName') AS OrganisationName,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Id') AS Question1,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[0].Answer') AS Answer1_1,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[1].Answer') AS Answer1_2,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].Questions[2].Answer') AS Answer1_3,
						CASE JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].CompletionStatus') WHEN 2 THEN 'COMPLETED' ELSE '' END AS Completion1,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Id') AS Question2,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[0].Answer') AS Answer2_1,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[1].Answer') AS Answer2_2,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[1].Questions[2].Answer') AS Answer2_3,
						CASE JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].CompletionStatus') WHEN 2 THEN 'COMPLETED' ELSE '' END AS Completion2,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[2].Id') AS Question3,
						JSON_VALUE([ReportingData], '$.Questions[0].SubSections[2].Questions[0].Answer') AS Answer3_1,
						CASE JSON_VALUE([ReportingData], '$.Questions[0].SubSections[0].CompletionStatus') WHEN 2 THEN 'COMPLETED' ELSE '' END AS Completion3,
						CONVERT(CHAR(10), JSON_VALUE([ReportingData], '$.Submitted.SubmittedAt')) AS SubmittedAt,
						JSON_VALUE([ReportingData], '$.Submitted.SubmittedName') AS SubmittedName,
						JSON_VALUE([ReportingData], '$.Submitted.SubmittedEmail') AS SubmittedEmail,
						Submitted
					FROM [dbo].[Report]
					WHERE JSON_VALUE([ReportingData], '$.OrganisationName') IS NOT NULL
                ) AS base
            ) AS a
        ) AS ab
    WHERE ab.newReportingData <> ''
    ) AS source(Employerid, ReportingPeriod, newReportingData)
    ON (
            target.Employerid = source.Employerid
            AND target.ReportingPeriod = source.ReportingPeriod
            )
WHEN MATCHED
    THEN
        UPDATE
        SET [ReportingData] = source.newReportingData;
