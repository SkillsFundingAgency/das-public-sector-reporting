Feature: SubmitButtonError - MPD-1148
*As a* Product Owner 
*I want* to notify the PSR user via an error message
*So that* PSR users are aware that mandatory information needs to be completed in order to maintain data integrity

Background:
Given Full access is granted
And a report has been created
And the report has not been submitted
And An invalid report

Scenario: User tries to submit a report
Given User navigates to Review summary page
Then the Report Summary page should be displayed
And the error summary is displayed at the top of the page
And the Continue button is not displayed