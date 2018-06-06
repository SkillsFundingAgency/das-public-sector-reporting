﻿Feature: ReportEdit - MPD-1135
As a PSR user 
I want to be able to edit a report that has already been started 
So that my organisation can submit information.

Background: 
Given Full access is granted
And  A Current report exists
And the report hasnt been submitted

Scenario: user can edit a report
	Given User navigates to the Edit report page
	Then the edit report page is displayed

Scenario: user can edit a report question
	Given User navigates to the Edit report page
	When  I click on 'Number of employees who work in England' question link
	Then the Your employees page is displayed

Scenario: user can submit an edited question
	Given User navigates to the Your employees question page
	And the question values <firstvalue>, <secondvalue> and <thirdValue> have been edited
	When I click on the save question
	Then The Your Employees question values <firstvalue>, <secondvalue> and <thirdValue> have been saved