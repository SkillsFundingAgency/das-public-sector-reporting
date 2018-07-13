Feature: SubmitButtonError - MPD-1148
*As a* Product Owner 
*I want* to notify the PSR user via an error message
*So that* PSR users are aware that mandatory information needs to be completed in order to maintain data integrity

Background:
Given Full access is granted
And A Current report exists
And the report hasnt been submitted
And An invalid report

Scenario: User tries to submit a report
Given User navigates to Review summary page
Then the Summary page should be displayed
And display the error summary widget at the top of the page
And do not display the Report 'Submit' button