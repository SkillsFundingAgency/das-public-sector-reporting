Feature: QuestionCompletionStatus - PD-1137
	In order to help me provide apprentice report data
	As a public sector employer
	I want to be informed when sections of the report are complete 


Background:
Given Edit access is granted

Scenario: Your Employees Incomplete
	Given A Current report exists
	Given I navigates to the Your employees question page
	And I answer new at start question
	And I answer new at end question
	But I do not answer new this period question
	When I click Continue
	Then I am returned to report edit page
    And no completion status is shown for your employees

Scenario: Your Employees Complete
	Given A Current report exists
	Given I navigates to the Your employees question page
	And I answer new at start question
	And I answer new at end question
	And I answer new this period question
	When I click Continue
	Then I am returned to report edit page
    And no completion status is for your employees is COMPLETE

Scenario: Your Apprentices Incomplete
	Given A Current report exists
	Given I navigates to the Your apprentices question page
	And I answer new at start question
	And I answer new at end question
	But I do not answer new this period question
	When I click Continue
	Then I am returned to report edit page
    And no completion status is shown for your apprentices

Scenario: Your Apprentices Complete
	Given A Current report exists
	Given I navigates to the Your apprentices question page
	And I answer new at start question
	And I answer new at end question
	And I answer new this period question
	When I click Continue
	Then I am returned to report edit page
    And no completion status is for your apprentices is COMPLETE
