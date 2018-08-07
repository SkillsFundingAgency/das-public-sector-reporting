Feature: QuestionCompletionStatus - PD-1137
	In order to help me provide apprentice report data
	As a public sector employer
	I want to be informed when sections of the report are complete 


Background:
Given Edit access is granted
And a report has been created

Scenario: Your Employees Incomplete
	Given User navigates to the Your employees question page
	And User answers the Your Employees new at start question with 5
	And User answers the Your Employees new at end question with 10
	But User does not answer the Your Employees new this period question
	When User clicks Continue on Your Employees question page
	Then User is returned to report edit page
    And no completion status is shown for your employees

Scenario: Your Employees Complete
	Given User navigates to the Your employees question page
	And User answers the Your Employees new at start question with 1
	And User answers the Your Employees new at end question with 1
	And User answers the Your Employees new this period question with 0
	When User clicks Continue on Your Employees question page
	Then User is returned to report edit page
    And completion status for your employees is COMPLETE

Scenario: Your Apprentices Incomplete
	Given User navigates to the Your apprentices question page
	And User answers the Your Apprentices new at start question with 3
	And User answers the Your Apprentices new at end question with 3
	But User does not answer the Your Apprentices new this period question
	When User clicks Continue on Your Apprentices question page
	Then User is returned to report edit page
    And no completion status is shown for your apprentices

Scenario: Your Apprentices Complete
	Given User navigates to the Your apprentices question page
	And User answers the Your Apprentices new at start question with 3
	And User answers the Your Apprentices new at end question with 5
	And User answers the Your Apprentices new this period question with 2
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
	And User answers full time equivalents question with 4
	When User clicks Continue on Full time equivalent question page
	Then User is returned to report edit page
    And completion status for full time equivalent is COMPLETE

Scenario: Outline Actions Incomplete
	Given User navigates to the Outline Actions question page
	When User clicks Continue on Outline Actions question page
	Then User is returned to report edit page
    And no completion status is shown for outline actions
		
Scenario: Outline Actions Complete
	Given User navigates to the Outline Actions question page
	And User answers Outline Actions question with "some text"
	When User clicks Continue on Outline Actions question page
	Then User is returned to report edit page
    And completion status for outline actions is COMPLETE

Scenario: Challenges Incomplete
	Given User navigates to the Challenges question page
	When User clicks Continue on Challenges question page
	Then User is returned to report edit page
    And no completion status is shown for challenges
		
Scenario: Challenges Complete
	Given User navigates to the Challenges question page
	And User answers Challenges question with "some text"
	When User clicks Continue on Challenges question page
	Then User is returned to report edit page
    And completion status for challenges is COMPLETE

	Scenario: Target Plans Incomplete
	Given User navigates to the Target Plans question page
	When User clicks Continue on Target Plans question page
	Then User is returned to report edit page
    And no completion status is shown for target plans
		
Scenario: Target Plans Complete
	Given User navigates to the Target Plans question page
	And User answers Target Plans question with "some text"
	When User clicks Continue on Target Plans question page
	Then User is returned to report edit page
    And completion status for target plans is COMPLETE

Scenario: Anything Else Incomplete
	Given User navigates to the Anything Else question page
	When User clicks Continue on Anything Else question page
	Then User is returned to report edit page
    And no completion status is shown for anything else
		
Scenario: Anything Else Complete
	Given User navigates to the Anything Else question page
	And User answers Anything Else question with 'some text'
	When User clicks Continue on Anything Else question page
	Then User is returned to report edit page
    And completion status for anything else is COMPLETE
