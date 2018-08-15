Feature: Homepage
	Homepage report creation flow should behave consistently with MPD-1701
		
Scenario: Edit user can create report from Homepage
	Given no current report exists
	And Edit access is granted
	And User navigates to Homepage
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	Then the create report page is displayed

Scenario: Super user can create report from Homepage
	Given no current report exists
	And Full access is granted
	And User navigates to Homepage
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	Then the create report page is displayed
	
Scenario: View user report shown not yet created page from Homepage
	Given no current report exists
	And View access is granted
	And User navigates to Homepage
	And User selects View report radio button
	When User clicks on homepage Continue button
	Then report not yet created page is shown
	
Scenario: Edit user is shown already submitted message from Homepage
	Given Edit access is granted
	And A valid report has been created
	And the report has been submitted
	And User navigates to Homepage
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	Then report already submitted page is shown

Scenario: Super user is shown already submitted message from Homepage
	Given Full access is granted
	And A valid report has been created
	And the report has been submitted
	And User navigates to Homepage
	And User selects Homepage Create a report Radio button
	When User clicks on homepage Continue button
	Then report already submitted page is shown

Scenario: View user is shown already submitted message from Homepage
	Given View access is granted
	And A valid report has been created
	And the report has been submitted
	And User navigates to Homepage
	And User selects View report radio button
	When User clicks on homepage Continue button
	Then report already submitted page is shown

Scenario: View user is able to return to Homepage from not yet created page
	Given no current report exists
	And View access is granted
	And User navigates to not yet created page
	When User clicks the not yet created page back link
	Then the Home page should be displayed

Scenario: Edit user is is able to return to Homepage from already submitted page
	Given Edit access is granted
	And A valid report has been created
	And the report has been submitted
	And User navigates to report already submitted page
	When User clicks the report already submitted page back link
	Then the Home page should be displayed

Scenario: Super user is is able to return to Homepage from already submitted page
	Given Full access is granted
	And A valid report has been created
	And the report has been submitted
	And User navigates to report already submitted page
	When User clicks the report already submitted page back link
	Then the Home page should be displayed

Scenario: View user is is able to return to Homepage from already submitted page
	Given View access is granted
	And A valid report has been created
	And the report has been submitted
	And User navigates to report already submitted page
	When User clicks the report already submitted page back link
	Then the Home page should be displayed