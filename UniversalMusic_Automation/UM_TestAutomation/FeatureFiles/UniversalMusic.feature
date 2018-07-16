@FunctionalTests
Feature: UniversalMusic

Background:  User is on the Angular js Conduit home page and is not logged in
Given user is on the Conduit Angularjs home page
	And user is not logged in
	And the home page is shown

#Scenario 1:
Scenario:User is logged on signup
When the user signs up with credentials
| username       | password       | email                      |
| randomuser_123 | randomuser_123 | randomuser_123@gmail.co.uk |
Then the user is automatically logged in
	And the users name is displayed

#Scenario 2
Scenario: User name is displayed when logged in
When the user logs in with credentials
| username       | password       | email                      |
| randomuser_123 | randomuser_123 | randomuser_123@gmail.co.uk |
Then the user is logged in
	And the users name is displayed

#Scenario 3: 
Scenario: Home page shows global feeds and popular tags
Then the global feeds and popular tags are displayed

#Scenario 4:
Scenario: User liking an article is redirected to signup if not logged in
When a user tries to like an article
Then the user is directed to the Sign up area

#Scenario 5: 
Scenario: Validation error for duplicate username signup
When the user tries to sign up with 'a username' that already exists
| username       | password       | email                     |
| randomuser_123 | randomuser_123 | randomuser321@gmail.co.uk |
Then a validation error is displayed
| validationError                 |
| username has already been taken |

#Scenario 6:
Scenario: Validation error for duplicate email signup
When the user tries to sign up with 'an email address' that already exists
| username       | password       | email                      |
| randomuser_321 | randomuser_123 | randomuser_123@gmail.co.uk |
Then a validation error is displayed
| validationError              |
| email has already been taken |
