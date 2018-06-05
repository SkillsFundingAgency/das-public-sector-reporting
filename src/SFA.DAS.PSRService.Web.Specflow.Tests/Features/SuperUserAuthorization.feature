Feature: super user access level - MPD-1132
Super user should be able to access all functionality

Background:
Given Full access is granted

Scenario: Super user can view create report page
	Given User navigates to Homepage
	And No current report exists
	When Selects Homepage 'Create a report' Radio button
	And I click on Continue button
	Then user should be taken to the create report page

Scenario: Super user can create a report
	Given User navigates to the Create report page
	And No current report exists
	When I click on Start button
	Then New report is created

Scenario: Super user can edit a report
	Given  A Current report exists
	And the report hasnt been submitted
	When I navigate to the Edit report page
	Then the edit report page is displayed

Scenario: Super user can edit a report question
	Given User navigates to the Edit report page
	When  I clicks on 'Number of employees who work in England' link
	Then the Your employees page is displayed

Scenario: Super user can submit an edited question
	Given User navigates to the 'Your employees' question page
	And the <firstvalue>, <secondvalue> and <thirdValue> have been edited
	When I click on the save question
	Then The 'Your Employees' question values are saved

Scenario: Confirm button is displayed on review details page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review details page
	Then the Review report details page is displayed
	And the confirm submission button should be displayed
	And the confirm submission button should have text 'Confirm'

Scenario: Confirm button is clicked on review details page
	Given I have a valid report
	And the report hasnt been submitted
	And I navigate to Review details page
	When I click the confirm submission button
	Then user is taken to confirm submission page

Scenario: Super user can submit a report
	Given I have a valid report
	And the report hasnt been submitted
	And user navigates to confirm submission page
	When I click the 'Submit your report' button
	Then the report should be submitted
	