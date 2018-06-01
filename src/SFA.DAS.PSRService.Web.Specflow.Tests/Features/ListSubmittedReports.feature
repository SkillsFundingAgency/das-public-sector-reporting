Feature:List Submitted Reports - MPD-1151

Background:
Given Full access is granted

Scenario: User Navigates to submitted reports page
		Given User navigates to Homepage
		When Selects Homepage 'View a previously submitted report' Radio button
		And I click on Continue button
		Then I should be taken to the Submitted Reports page
		And back link is shown as 'Back'

Scenario: User Views previously submitted reports when no reports submitted
	Given There are no submitted reports 
	When User navigates to Submitted reports page
	Then I should see the message 'There are currently no submitted reports to show'

Scenario: User Views previously submitted reports when one report submitted
	Given There is one submitted report
	When User navigates to Submitted reports page
	Then I should see one submitted report displayed in list