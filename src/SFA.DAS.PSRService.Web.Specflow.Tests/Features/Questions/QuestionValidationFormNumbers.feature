Feature: QuestionValidationFormNumbers - MPD-1143
*As a* Product Owner 
*I want* Form Validation 
*So that* data quality is maintained.

#Steps need to be check for consistancy of naming convention
Background:
Given user is already logged in as a super user
And a report has been created 
And a report has not been submitted

Scenario: Clicking outside the @start field 
Given user is in the @start field
When user enters any value other than a whole number
And clicks outside the field
Then display "Must be a number between 0 and 9" above the text field

Scenario: Clicking outside the @end field 
Given user is in the @end field
When user enters any value other than a whole number
And clicks outside the field
Then display "Must be a number between 0 and 9" above the text field

Scenario: Clicking outside the @newthisperiod field 
Given user is in the @newthisperiod field
When user enters any value other than a whole number
And clicks outside the field
Then display "Must be a number between 0 and 9" above the text field

Scenario: Clicking 'Continue' if any of the three fields are of incorrect format
Given user navigates to the Your Employee question page
When user enters any value in this page other than a whole number in one or more fields
And clicks the 'Continue' button
Then display the following message at the top of the page with red borders
"Please fix these errors"
"Must be a number between 0 and 9"
And the 'Your Employee' question page is displayed