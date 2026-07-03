# Test Plan — SauceDemo E-commerce Application

## 1. Objective

The objective of this test plan is to verify the main user flows of the SauceDemo e-commerce application.

The project includes both manual test documentation and automated end-to-end tests.

## 2. Application Under Test

Application: SauceDemo  
URL: https://www.saucedemo.com/

## 3. Scope of Testing

The following areas are included in the test scope:

- login,
- product list,
- product sorting,
- cart,
- checkout,
- form validation,
- error messages.

## 4. Out of Scope

The following areas are not covered in this version:

- payment integration,
- database validation,
- performance testing,
- accessibility testing,
- security testing.

## 5. Test Types

The project includes:

- smoke tests,
- regression tests,
- positive test scenarios,
- negative test scenarios,
- UI end-to-end tests.

## 6. Test Environment

Browser: Chromium  
Operating System: Windows 11  
Automation Tool: Playwright  
Language: TypeScript

## 7. Test Data

Valid user:

- username: standard_user
- password: secret_sauce

Invalid login data:

- username: standard_user
- password: wrong_password

Checkout data:

- first name: Agent
- last name: Smith
- postal code: 12345

## 8. Entry Criteria

Testing can start when:

- the application is available,
- test environment is ready,
- test data is known,
- basic test scenarios are prepared.

## 9. Exit Criteria

Testing can be finished when:

- all high-priority scenarios are executed,
- all automated tests pass,
- test results are reviewed,
- detected issues are documented.

## 10. Risks

Potential risks:

- test application may be temporarily unavailable,
- UI locators may change,
- network issues may affect test execution.