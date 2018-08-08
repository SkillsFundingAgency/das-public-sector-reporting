Feature: QuestionOrganisationName - MPD-1141, MPD-1140
	In order to submit apprentice report data
	As a public sector employer
	I want to enter my organisation name

Background:
Given Edit access is granted

Scenario: Organisation Name is pre-populated
	Given no current report exists
	And User creates a new report
	And User navigates to the Edit report page
	When  User clicks on 'Your organisation''s name' question link
	Then organisation name is pre-populated with name from my account

Scenario: Organisation Name Incomplete
	Given a report has been created
	And User navigates to the Organisation Name question page
	And User answers Organisation Name question with ""
	When User clicks Continue on Organisation Name question page
	Then User is returned to report edit page
    And no completion status is shown for organisation name
		
Scenario: Organisation Name Complete
	Given a report has been created
	And User navigates to the Organisation Name question page
	And User answers Organisation Name question with "some text"
	When User clicks Continue on Organisation Name question page
	Then User is returned to report edit page
    And completion status for organisation name is COMPLETE

Scenario Outline: user can update Organisation Name question
	Given a report has been created
	And User navigates to the Organisation Name question page
	And User answers Organisation Name question with <answer_text>
	When User clicks Continue on Organisation Name question page
	Then The organisation question value <answer_text> has been saved
Examples: 
| answer_text |
| "one"       |
| "two"       |
