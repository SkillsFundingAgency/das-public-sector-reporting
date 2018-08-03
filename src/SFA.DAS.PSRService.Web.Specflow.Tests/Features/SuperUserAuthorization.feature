Feature: super user access level - MPD-1132
Super user should be able to access all functionality

Background:
Given Full access is granted

Scenario: Super user can view create report page
	Given User navigates to Homepage
	And no current report exists
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	Then the create report page is displayed

Scenario: Super user can create a report
	Given User navigates to the Create report page
	And no current report exists
	When User clicks on Start button
	Then New report is created

Scenario: Super user can edit a report
	Given  A Current report exists
	And the report has not been submitted
	When User navigates to the Edit report page
	Then the edit report page is displayed

Scenario: Super user can edit a report question
	Given a report has been created
	And User navigates to the Edit report page
	When  User clicks on 'Number of employees who work in England' question link
	Then the Your employees page is displayed

Scenario Outline: Super user can submit an edited question
	Given a report has been created
	And User navigates to the Your employees question page
	And the your employees question values <atStart>, <atEnd> and <newThisPeriod> have been edited
	When User clicks on the save question
	Then The Your Employees question values <atStart>, <atEnd> and <newThisPeriod> have been saved
	Examples: 
	| atStart | atEnd | newThisPeriod |
	| 100     | 150   | 50            |

Scenario: Super user clicks Continue button on review summary page
	Given A valid report has been created
	And the report has not been submitted
	And User navigates to Review summary page
	When user clicks the continue button
	Then the confirm submission page is displayed
	And the confirm submission button should have text 'Submit your report'

Scenario: Super user can submit a report
	Given A valid report has been created
	And the report has not been submitted
	And user navigates to confirm submission page
	When user clicks the submit your report button
	Then the report should be submitted
	