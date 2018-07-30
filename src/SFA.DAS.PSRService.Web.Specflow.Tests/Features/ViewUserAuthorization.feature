Feature: View user access level - MPD-1130
View user should be able to view the current and previously submitted reports but not edit or submit a report

Background:
Given View access is granted

Scenario: View user can view report but not create report page
	Given no current report exists
	When User navigates to Homepage
	Then View Report radio button does exist
	But Create Report radio button does not exist

Scenario: View user cannot edit a report
	Given  A Current report exists
	And the report has not been submitted
	When User navigates to the Edit report page
	Then the Home page should be displayed

Scenario: View user cannot edit a report question
	Given A Current report exists
	When User navigates to the Edit report page
	Then the Home page should be displayed

Scenario: View user can view the review details page
	Given A valid report has been created
	And the report has not been submitted
	When User navigates to Review summary page
	Then the View report details page is displayed

Scenario: Confirm button is not available on review details page
	Given A valid report has been created
	And the report has not been submitted
	When User navigates to Review summary page
	Then the confirm submission button should not be available
