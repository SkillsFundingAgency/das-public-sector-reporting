Feature: ReportCalculations - MPD-1147
	In order to meet apprenticeship targets
	As a public sector employer
	I should be shown reporting percentages based on the answers I provide to questions

Background:
Given Edit access is granted
And a report has been created

Scenario Outline: Reporting Percentages
Given User navigates to the Your employees question page
And User answers the Your Employees new at start question with <employeesAtStart>
And User answers the Your Employees new at end question with <employeesAtEnd> 
And User answers the Your Employees new this period question with <employeesNewInPeriod>
And User clicks Continue on Your Employees question page
And User navigates to the Your apprentices question page
And User answers the Your Apprentices new at start question with <apprenticesAtStart>
And User answers the Your Apprentices new at end question with <apprenticesAtEnd>
And User answers the Your Apprentices new this period question with <apprenticesNewInPeriod>
And User clicks Continue on Your Apprentices question page
When User navigates to Review summary page
Then Reporting percentages employment starts is <EmploymentStarts>
And <EmploymentStarts> is saved as reporting percentages employment starts
And Reporting percentages total head count is <TotalHeadCount>
And <TotalHeadCount> is saved as reporting percentages total head count
And Reporting percentages new this period is <NewThisPeriod>
And <NewThisPeriod> is saved as reporting percentages new this period
Examples: 
| employeesAtStart | employeesAtEnd | employeesNewInPeriod | apprenticesAtStart | apprenticesAtEnd | apprenticesNewInPeriod | EmploymentStarts | TotalHeadCount | NewThisPeriod |
| 250                         | 150                       | 0                                       | 25                             | 30                           | 5                                         | 0                               | 20                        | 2                         |
| 300                         | 350                      | 50                                     | 0                               | 50                           | 50                                      | 100                           | 14.29                   | 16.67                 |
| 500                         | 500                      | 0                                       | 0                               | 30                           | 30                                      | 0                               | 6                           | 6                        |