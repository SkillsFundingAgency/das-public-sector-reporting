Feature: ReportCalculations - MPD-1147
	In order to meet apprenticeship targets
	As a public sector employer
	I should be shown reporting percentages based on the answers I provide to questions

Scenario Outline: Reporting Percentages
Given I have entered <employeesAtStart>, <employeesAtEnd>, <employeesNewInPeriod>, <apprenticesAtStart>, <apprenticesAtEnd> and <apprenticesNewInPeriod>
Then I should see <EmploymentStarts>, <TotalHeadCount> and <NewThisPeriod> percentages
Examples: 
| employeesAtStart | employeesAtEnd | employeesNewInPeriod | apprenticesAtStart | apprenticesAtEnd | apprenticesNewInPeriod | EmploymentStarts | TotalHeadCount | NewThisPeriod |
| 250              | 150            | 0                    | 25                 | 30               | 5                      | 0                | 3.33           | 2.00          |
| 300              | 350            | 50                   | 0                  | 50               | 50                     | 0                | 14.29          | 16.67         |
| 500             | 500            | 0                    | 0                 | 30               | 30                      | 0                | 6.00           | 6.00          |