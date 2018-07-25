Feature: QuestionOrganisationName - MPD-1141, MPD-1140
	In order to submit apprentice report data
	As a public sector employer
	I want to enter my organisation name

Background:
Given Edit access is granted

Scenario: Organisation Name is pre-populated
	Given no current report exists
	And User navigates to Homepage
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	And User clicks on Start button
	And  User clicks on 'Your organisation''s name' question link
	Then organisation name is pre-populated with name from my account
