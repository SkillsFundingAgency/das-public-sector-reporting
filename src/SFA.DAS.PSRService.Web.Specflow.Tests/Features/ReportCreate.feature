Feature: ReportCreate - MPD-1134
As a PSR user 
I want to create a report for the first time
So that my organisation can submit information.

Background: 
Given Full access is granted
And no current report exists
And no partially completed report exists

Scenario: User sees the 'Create Report' page 
Given user navigates to the home page
When user selects the 'create a report' radio button 
And user clicks 'continue'
Then the 'Create Report' page is displayed

Scenario: User can create a report for the first time
Given user navigates to the 'Create Report' page 
When user clicks the 'start' button 
Then a new report is created 
And the 'Edit report' page is displayed