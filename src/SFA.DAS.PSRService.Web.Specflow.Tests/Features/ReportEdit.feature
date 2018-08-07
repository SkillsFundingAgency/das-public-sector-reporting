﻿Feature: ReportEdit - MPD-1135
As a PSR user 
I want to be able to edit a report that has already been started 
So that my organisation can submit information.

Background: 
Given Edit access is granted
And  a report has been created
And the report has not been submitted

Scenario: user can edit a report
	Given User navigates to the Edit report page
	Then the edit report page is displayed

Scenario: user can edit a report question
	Given User navigates to the Edit report page
	When  User clicks on 'Number of employees who work in England' question link
	Then the Your employees page is displayed

Scenario Outline: user can update YourEmployees question
	Given User navigates to the Your employees question page
	And the your employees question values <atStart>, <atEnd> and <newThisPeriod> have been edited
	When User clicks on the save question
	Then The Your Employees question values <atStart>, <atEnd> and <newThisPeriod> have been saved
	Examples: 
	| atStart | atEnd | newThisPeriod |
	| 100     | 150   | 50            |
	| 1000    | 750   | 0             |
	| 850     | 1000  | 150           |
	| 850000  | 1000000  | 150        |
	
Scenario Outline: user can update YourApprentices question
	Given User navigates to the Your apprentices question page
	And the your apprentices question values <atStart>, <atEnd> and <newThisPeriod> have been edited
	When User clicks on the save question
	Then The Your Apprentices question values <atStart>, <atEnd> and <newThisPeriod> have been saved
	Examples: 
	| atStart | atEnd | newThisPeriod |
	| 100     | 150   | 50            |
	| 1000    | 750   | 0             |
	| 850     | 1000  | 150           |
	| 850000  | 1000000  | 150        |

Scenario Outline: user can update FullTimeEquivalents question
	Given User navigates to the Full time equivalent question page
	And the full time equivalents question value <atStart> has been edited
	When User clicks on the save question
	Then The full time equivalents question value <atStart> has been saved
	Examples: 
	| atStart |
	| 100     |
	| 1000    |
	| 850     |
	| 850000  |
	| 1000000  | 

Scenario Outline: user can update Outline Actions question
	Given User navigates to the Outline Actions question page
	And User answers Outline Actions question with <answer_text>
	When User clicks on the save question
	Then The outline actions question value <answer_text> has been saved
Examples: 
| answer_text                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| "one"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| "two"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| "Maecenas enim lorem, pharetra sed iaculis eleifend, blandit ultrices urna. Nullam at cursus magna, et auctor lectus. Maecenas consectetur, nibh in dignissim dapibus, tortor magna fringilla urna, eget tempor tortor nibh vitae felis. Fusce auctor vitae nunc nec maximus. Praesent augue lacus, venenatis quis nisi eget, suscipit faucibus eros. Nullam maximus nibh non dictum tempus. Duis ac diam non ipsum ullamcorper fringilla eu ut felis. Sed massa justo, commodo et lacus at, fringilla dignissim tellus. Ut porttitor sem et lectus mattis commodo. Suspendisse iaculis dignissim mauris, in porta libero. Sed bibendum sapien nulla, sed sodales diam sagittis at. Curabitur id ipsum aliquet, cursus diam et, lacinia tortor. Nulla leo est, dapibus eget lacus ac, fringilla faucibus orci. Pellentesque nunc lectus, vehicula non arcu quis, ultrices viverra mi. Fusce ultricies sodales nisl, vitae hendrerit nunc suscipit ut. " |

Scenario Outline: user can update Challenges question
	Given User navigates to the Challenges question page
	And User answers Challenges question with <answer_text>
	When User clicks on the save question
	Then The Challenges question value <answer_text> has been saved
Examples: 
| answer_text |
| "one"       |
| "two"       |
|       Donec at iaculis mi. Duis in tempor libero. Aenean tortor augue, venenatis et leo in, laoreet bibendum sem. In scelerisque nisi et tincidunt maximus. Proin sagittis consequat velit, fermentum iaculis elit placerat et. Proin quis luctus tortor. Nullam nec tellus laoreet, tincidunt neque et, eleifend enim. Aliquam rutrum erat nunc, non congue sapien laoreet in. Aliquam eleifend mauris nunc. Sed at massa vulputate, laoreet ipsum eu, auctor neque. Maecenas elementum sed risus id posuere. Curabitur sit amet sem sed libero gravida efficitur vitae in erat. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vestibulum feugiat pulvinar ex, vitae aliquet eros ullamcorper ac. Phasellus vel sapien vitae odio vestibulum rutrum sed id ligula. Phasellus in felis id magna convallis semper quis non lectus.       |		
