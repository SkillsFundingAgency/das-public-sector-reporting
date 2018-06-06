Feature: Edit user access level - MPD-1131
Edit user should be able to create and edit a report but not submit a report

Background:
Given Edit access is granted

Scenario: Edit user can view create report page
	Given User navigates to Homepage
	And No current report exists
	When Selects Homepage 'Create a report' Radio button
	And I click on Continue button
	Then the create report page is displayed

Scenario: Edit user can create a report
	Given User navigates to the Create report page
	And No current report exists
	When I click on Start button
	Then New report is created

Scenario: Edit user can edit a report
	Given  A Current report exists
	And the report hasnt been submitted
	When I navigate to the Edit report page
	Then the edit report page is displayed

Scenario: Edit user can edit a report question
	Given User navigates to the Edit report page
	When  I clicks on 'Number of employees who work in England' link
	Then the Your employees page is displayed

Scenario: Edit user can submit an edited question
	Given User navigates to the 'Your employees' question page
	And the <firstvalue>, <secondvalue> and <thirdValue> have been edited
	When I click on the save question
	Then The 'Your Employees' question values are saved

Scenario: Edit user can view the review summary page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review summary page
	Then the Review report details page is displayed

Scenario: Confirm button is not available on review summary page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review summary page
	Then the confirm submission button should not be available