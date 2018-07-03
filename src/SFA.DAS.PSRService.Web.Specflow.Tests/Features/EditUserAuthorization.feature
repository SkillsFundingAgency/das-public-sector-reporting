Feature: Edit user access level - MPD-1131
Edit user should be able to create and edit a report but not submit a report

Background:
Given Edit access is granted

Scenario: Edit user can view create report page
	Given User navigates to Homepage
	And no current report exists
	When I Select Homepage 'Create a new report' Radio button
	And I click on homepage Continue button
	Then the create report page is displayed

Scenario: Edit user can create a report
	Given User navigates to the Create report page
	And no current report exists
	When I click on Start button
	Then New report is created

Scenario: Edit user can edit a report
	Given a report has been created
	And the report hasnt been submitted
	When I navigate to the Edit report page
	Then the edit report page is displayed

Scenario: Edit user can edit a report question
	Given User navigates to the Edit report page
	When  I clicks on 'Number of employees who work in England' link
	Then the Your employees page is displayed

Scenario: Edit user can submit an edited question
	Given User navigates to the Your employees question page
	And the question values <atStart>, <atEnd> and <newThisPeriod> have been edited
	When I click on the save question
	Then The 'Your Employees' question values are saved

Scenario: Edit user can view the review summary page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review summary page
	Then the Review report details page is displayed

Scenario: Continue button is not available on review summary page
	Given I have a valid report
	And the report hasnt been submitted
	When I navigate to Review summary page
	Then the continue button should not be available