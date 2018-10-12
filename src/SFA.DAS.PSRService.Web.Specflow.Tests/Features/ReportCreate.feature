Feature: ReportCreate - MPD-1134
As a PSR user 
I want to create a report for the first time
So that my organisation can submit information.

Background: 
Given Full access is granted
And no current report exists

Scenario: User sees the 'Create Report' page 
Given User navigates to Homepage
And User selects Homepage Create a report Radio button
When User clicks on homepage Continue button
Then the create report page is displayed

Scenario: User can create a report for the first time
Given User navigates to the Create report page 
When User clicks on Start button
Then a new report is created 
And the edit report page is displayed