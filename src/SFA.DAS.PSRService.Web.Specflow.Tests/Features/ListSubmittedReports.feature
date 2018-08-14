Feature:List Submitted Reports - MPD-1151, MPD-1149

Background:
Given Full access is granted

Scenario: User Navigates to submitted reports page
	Given User navigates to Homepage
	And User selects Homepage View a previously submitted report Radio button
	When User clicks on homepage Continue button
	Then User should be taken to the Submitted Reports page
	And back link is shown as 'Back'

Scenario: User Views previously submitted reports when no reports submitted
	Given A Current report exists
	And the report has not been submitted
	And user navigates to previously submitted reports page
	Then the User should see the message 'There are currently no submitted reports to show' on the previously submitted page

Scenario: User Views previously submitted reports when one report submitted
	Given a report has been created
	And the report has been submitted
	And user navigates to previously submitted reports page
	Then the user should see one submitted report displayed in list

Scenario: User on previously submitted reports page can click back button
	Given a report has been created
	And the report has been submitted
	And user navigates to previously submitted reports page
	When user clicks the List Submitted Reports back button
	Then the user is returned to the homepage

Scenario: User Views previously submitted reports detail when one report submitted
	Given a report has been created
	And the report has been submitted
	And user navigates to previously submitted reports page
	When user clicks on View link for report 1
	Then the Report Summary page with submitted report details should be displayed
