Feature: MenuNavigation
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
Given Full access is granted

Scenario: User clicks on menu link
	Given User navigates to Homepage
	When I click on the <menuItem>
	Then i should be taken to <url>

Examples: 
| menuItem   | url |
| home | http://www.bbc.co.uk |
| testuser_2 | Test@153 |


