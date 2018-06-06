Feature: ReportConfirmationScreen - MPD-1486
 submission user will be presented with page that states this is final prior to clicking submit button.

 Background: 
 Given Full access is granted

Scenario: Valid report not submitted shows submit Confirmation page
	Given I have a valid report
	And the report hasnt been submitted
	And user navigates to confirm submission page
	Then the submit confirmation page should be displayed

Scenario: Invalid report not submitted shows edit page
	Given I have an invalid report
	And the report hasnt been submitted
	And user navigates to confirm submission page
	Then the Home page should be displayed

Scenario: Submitted report shows home page
	Given I have an invalid report
	And the report has been submitted
	And user navigates to confirm submission page
	Then the Home page should be displayed

Scenario: Submit confirmation page back button takes user to summary page
	Given I have a valid report
	And the report hasnt been submitted
	And user navigates to confirm submission page
	When I click the back button
	Then the Summary page should be displayed

Scenario: Submit confirmation page 'return to your report' link takes user to summary page
	Given I have a valid report
	And the report hasnt been submitted
	And user navigates to confirm submission page
	When I click the 'return to your report' button
	Then the Summary page should be displayed

Scenario: User submits a completed report
	Given I have a valid report
	And the report hasnt been submitted
	And user navigates to confirm submission page
	When I click the 'Submit your report' button
	Then the report should be submitted
	And i should be shown the report submitted page