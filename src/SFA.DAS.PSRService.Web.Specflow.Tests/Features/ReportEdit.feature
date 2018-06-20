Feature: ReportEdit - MPD-1135
As a PSR user 
I want to be able to edit a report that has already been started 
So that my organisation can submit information.

Background: 
Given Full access is granted
And  a report has been created
And the report hasnt been submitted

Scenario: user can edit a report
	Given User navigates to the Edit report page
	Then the edit report page is displayed

Scenario: user can edit a report question
	Given User navigates to the Edit report page
	When  I click on 'Number of employees who work in England' question link
	Then the Your employees page is displayed

Scenario Outline: user can submit an edited question
	Given User navigates to the Your employees question page
	And the question values <atStart>, <atEnd> and <newThisPeriod> have been edited
	When I click on the save question
	Then The Your Employees question values <atStart>, <atEnd> and <newThisPeriod> have been saved
	Examples: 
	| atStart | atEnd | newThisPeriod |
	| 100     | 150   | 50            |