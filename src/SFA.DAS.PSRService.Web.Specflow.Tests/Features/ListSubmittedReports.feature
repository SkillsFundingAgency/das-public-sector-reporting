Feature:List Submitted Reports - MPD-1151

Background:
Given Full access is granted

Scenario: User Navigates to submitted reports page
		Given User navigates to Homepage
		When Selects Homepage 'View a previously submitted report' Radio button
		And I click on homepage Continue button
		Then I should be taken to the Submitted Reports page
		And back link is shown as 'Back'

Scenario: User Views previously submitted reports when no reports submitted
	Given A Current report exists
	And the report hasnt been submitted
	And user navigates to previously submitted reports page
	Then I should see the message 'There are currently no submitted reports to show' on the previously submitted page

#Scenario: User Views previously submitted reports when one report submitted
#		Given A Current report exists
#	And the report has been submitted
#	And user navigates to previously submitted reports page
#	Then I should see one submitted report displayed in list
#
#	Scenario: User on previously submitted reports page can click back button
#	Given A Current report exists
#	And the report has been submitted
#	And user navigates to previously submitted reports page
#	When user clicks the back button
#	Then the user is displayed the homepage
	