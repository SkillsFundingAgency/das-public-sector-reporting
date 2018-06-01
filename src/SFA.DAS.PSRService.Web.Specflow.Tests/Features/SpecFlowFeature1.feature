Feature: SpecFlowFeature1

Background:
Given Full access is granted

Scenario: User views submitted reports
Given User navigates to Homepage
		When Selects 'View a previously submitted report' Radio button
		And I click on Continue button
		Then I should be on Submitted Reports page