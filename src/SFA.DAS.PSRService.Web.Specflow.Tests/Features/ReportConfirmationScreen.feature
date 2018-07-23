Feature: ReportConfirmationScreen - MPD-1486
 submission user will be presented with page that states this is final prior to clicking submit button.

 Background: 
 Given Full access is granted
 And a report has been created

Scenario: Valid report not submitted shows submit Confirmation page
	Given A valid report
	And the report has not been submitted
	And user navigates to confirm submission page
	Then the submit report confirmation page should be displayed

Scenario: Invalid report not submitted shows review summary page
	Given An invalid report
	And the report has not been submitted
	And user navigates to confirm submission page
	Then the Review report details page is displayed

Scenario: Submitted report shows home page
	Given An invalid report
	And the report has been submitted
	And user navigates to confirm submission page
	Then the Home page should be displayed

Scenario: Submit confirmation page back button takes user to summary page
	Given A valid report
	And the report has not been submitted
	And user navigates to confirm submission page
	When user clicks the confirm submission back button
	Then the Report Summary page should be displayed

Scenario: Submit confirmation page 'return to your report' link takes user to summary page
	Given A valid report
	And the report has not been submitted
	And user navigates to confirm submission page
	When user clicks the return to your report button
	Then the Report Summary page should be displayed

Scenario: User submits a completed report
	Given A valid report
	And the report has not been submitted
	And user navigates to confirm submission page
	When user clicks the submit your report button
	Then the report should be submitted
	And the report submitted page should be displayed