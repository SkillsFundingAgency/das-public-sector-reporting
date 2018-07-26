Feature: QuestionCompletionStatus - PD-1137
	In order to help me provide apprentice report data
	As a public sector employer
	I want to be informed when sections of the report are complete 


Background:
Given Edit access is granted
And a report has been created

Scenario: Your Employees Incomplete
	Given User navigates to the Your employees question page
	And User answers the Your Employees new at start question
	And User answers the Your Employees new at end question
	But User does not answer the Your Employees new this period question
	When User clicks Continue on Your Employees question page
	Then User is returned to report edit page
    And no completion status is shown for your employees

Scenario: Your Employees Complete
	Given User navigates to the Your employees question page
	And User answers the Your Employees new at start question
	And User answers the Your Employees new at end question
	And User answers the Your Employees new this period question
	When User clicks Continue on Your Employees question page
	Then User is returned to report edit page
    And completion status for your employees is COMPLETE

Scenario: Your Apprentices Incomplete
	Given User navigates to the Your apprentices question page
	And User answers the Your Apprentices new at start question
	And User answers the Your Apprentices new at end question
	But User does not answer the Your Apprentices new this period question
	When User clicks Continue on Your Apprentices question page
	Then User is returned to report edit page
    And no completion status is shown for your apprentices

Scenario: Your Apprentices Complete
	Given User navigates to the Your apprentices question page
	And User answers the Your Apprentices new at start question
	And User answers the Your Apprentices new at end question
	And User answers the Your Apprentices new this period question
	When User clicks Continue on Your Apprentices question page
	Then User is returned to report edit page
    And completion status for your apprentices is COMPLETE

Scenario: Full Time Equivalent Incomplete
	Given User navigates to the Full time equivalent question page
	When User clicks Continue on Full time equivalent question page
	Then User is returned to report edit page
    And no completion status is shown for full time equivalent
		
	Scenario: Full Time Equivalent Complete
	Given User navigates to the Full time equivalent question page
	And User answers full time equivalents question
	When User clicks Continue on Full time equivalent question page
	Then User is returned to report edit page
    And completion status for full time equivalent is COMPLETE
