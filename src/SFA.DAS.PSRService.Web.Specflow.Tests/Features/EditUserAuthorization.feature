Feature: Edit user access level - MPD-1131
Edit user should be able to create and edit a report but not submit a report

Background:
Given Edit access is granted

Scenario: Edit user can view create report page
	Given  no current report exists
	And User navigates to Homepage
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	Then the create report page is displayed

Scenario: Edit user can create a report
	Given no current report exists
	And User navigates to the Create report page
	When User clicks on Start button
	Then New report is created

Scenario: Edit user can edit a report
	Given a report has been created
	And the report has not been submitted
	When User navigates to the Edit report page
	Then the edit report page is displayed

Scenario: Edit user can edit a report question
	Given a report has been created
	And  User navigates to the Edit report page
	When  User clicks on 'Number of employees who work in England' question link
	Then the Your employees page is displayed

Scenario Outline: Edit user can submit an edited question
	Given a report has been created
	And User navigates to the Your employees question page
	And the your employees question values <atStart>, <atEnd> and <newThisPeriod> have been edited
	When User clicks on the save question
	Then The Your Employees question values <atStart>, <atEnd> and <newThisPeriod> have been saved
	Examples: 
	| atStart | atEnd | newThisPeriod |
	| 100     | 150   | 50            |

Scenario: Edit user can view the review summary page
	Given A valid report has been created
	And the report has not been submitted
	When User navigates to Review summary page
	Then the Review report details page is displayed

Scenario: Edit user sees Continue button on review summary page
	Given A valid report has been created
	And the report has not been submitted
	When User navigates to Review summary page
	Then the continue button should be available

Scenario: Edit user clicks Continue button on review summary page
	Given A valid report has been created
	And the report has not been submitted
	And User navigates to Review summary page
	When user clicks the continue button
	Then the edit complete page is displayed
