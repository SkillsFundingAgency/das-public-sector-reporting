Feature: View user access level - MPD-1130
View user should be able to view the current and previously submitted reports but not edit or submit a report

Background:
Given View access is granted

Scenario: View user cannot view create report page
	Given No current report exists
	When User navigates to Homepage
	Then Create Report radio button does not exist

Scenario: View user cannot edit a report
	Given  A Current report exists
	And the report hasnt been submitted
	When I navigate to the Edit report page
	Then the Home page should be displayed

Scenario: View user cannot edit a report question
	Given A Current report exists
	When User navigates to the Edit report page
	Then the Home page should be displayed

Scenario: View user can view the review details page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review details page
	Then the Review report details page is displayed

Scenario: Confirm button is not available on review details page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review details page
	Then the confirm submission button should not be available