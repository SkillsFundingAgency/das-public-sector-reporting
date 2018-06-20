﻿Feature: ReportAuditHistory - MPD-1149
	In order to help me review report data
	As a public sector employer
	I should be able to see previous versions of the report

Background: 
	Given Edit access is granted
	Given no current report exists

Scenario: Single user delay between report creation and first edit
Given I create a report at time 14:00
And I set number of employees at period start to '50' at time 14:06
When I navigate to the history view
Then I will see one entry in the history view at time 14:00
And the 14:00 report has  the number of employees at period start as blank 

Scenario: Single user multiple delayed edits
Given I create a report at time 12:52
And I set number of employees at period start to '50' at time 12:58
And I set the number of employees at period end to '25' at time 13:06
When I navigate to the history view
Then there are two entries
And One history entry has time 12:52
And One history entry has time 12:58
And the 12:52 entry has blank number of employees at period start
And the 12:52 has blank number of employees at period end
And 12:58 has '50' employees at period start
And 12:58 has blank number of employees at period end. 

Scenario: Single user multiple edits with audit window
Given I create a report at time 12:52
And I set number of employees at period start to '50' at time 12:53
And I set the number of employees at period end to '25' at time 12:54
When I navigate to the history view
Then there are  no entries

Scenario: Multiple user edits within audit window
Given Bob create a report at time 11:31
And Bob sets number of employees at period start to '50' at time 11:32
And Alice sets number of employees at period end to '25' at time 11:33
When Bob navigates to history view
Then Bob will see one entry with his name at time 11:32


