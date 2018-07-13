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
	When User navigates to the OrganisationName page
	Then organisation name is pre-populated with name from my account
