# QA Automation Portfolio — E-commerce Tests

This project is a QA Automation portfolio project created to demonstrate manual testing and automated end-to-end testing skills.

The tested application is a demo e-commerce website:
https://www.saucedemo.com/

## Project Goal

The goal of this project is to show practical QA skills that are useful in a real software testing job or internship.

This repository includes:

* manual test documentation,
* automated UI tests,
* negative and positive test scenarios,
* Page Object Model structure,
* test data separation,
* Playwright HTML report,
* GitHub Actions CI workflow.

## Tech Stack

* Playwright
* TypeScript
* Node.js
* GitHub Actions
* Markdown documentation

## Tested Features

The automated tests cover the main user flows of the demo e-commerce application.

### Login

* successful login with valid credentials,
* login with invalid password,
* login with empty fields,
* validation of error messages.

### Products

* product list visibility,
* adding product to cart,
* removing product from cart,
* product sorting.

### Cart

* opening the cart,
* verifying added products,
* removing products from the cart.

### Checkout

* successful checkout flow,
* checkout with missing required fields,
* validation of checkout error messages.

## Test Types

This project contains examples of:

* smoke tests,
* regression tests,
* positive test scenarios,
* negative test scenarios,
* UI end-to-end tests,
* manual test cases,
* sample bug reports.

## Project Structure

```text
qa-automation-portfolio/
├── docs/
│   ├── test-plan.md
│   ├── bug-reports.md
│   └── qa-summary.md
├── pages/
│   ├── LoginPage.ts
│   ├── ProductsPage.ts
│   ├── CartPage.ts
│   └── CheckoutPage.ts
├── tests/
│   ├── login.spec.ts
│   ├── products.spec.ts
│   ├── cart.spec.ts
│   └── checkout.spec.ts
├── test-data/
│   └── users.ts
├── .github/
│   └── workflows/
│       └── playwright.yml
├── playwright.config.ts
├── package.json
└── README.md
```

## How to Install

Clone the repository:

```bash
git clone https://github.com/Winchesss/testy
```

Go to the project folder:

```bash
cd qa-automation-portfolio
```

Install dependencies:

```bash
npm install
```

Install Playwright browsers:

```bash
npx playwright install
```

## How to Run Tests

Run all tests:

```bash
npx playwright test
```

Run tests in headed mode:

```bash
npx playwright test --headed
```

Run tests in UI mode:

```bash
npx playwright test --ui
```

Run a specific test file:

```bash
npx playwright test tests/login.spec.ts
```

## Test Report

After running the tests, open the HTML report:

```bash
npx playwright show-report
```

The report contains information about:

* passed tests,
* failed tests,
* test duration,
* screenshots and traces if configured.

## Example Test Scenarios

### TC-001: Successful Login

**Preconditions:**
User is on the login page.

**Steps:**

1. Enter a valid username.
2. Enter a valid password.
3. Click the Login button.

**Expected Result:**
User is redirected to the products page.

### TC-002: Login With Invalid Password

**Preconditions:**
User is on the login page.

**Steps:**

1. Enter a valid username.
2. Enter an invalid password.
3. Click the Login button.

**Expected Result:**
An error message is displayed and the user is not logged in.

### TC-003: Add Product to Cart

**Preconditions:**
User is logged in.

**Steps:**

1. Select a product.
2. Click the Add to cart button.
3. Open the cart.

**Expected Result:**
The selected product is visible in the cart.

### TC-004: Checkout With Missing Required Fields

**Preconditions:**
User has a product in the cart.

**Steps:**

1. Open the checkout page.
2. Leave required fields empty.
3. Click the Continue button.

**Expected Result:**
A validation error message is displayed.

## Sample Bug Report

### BUG-001: Checkout Form Allows Empty Required Field

**Severity:** Medium
**Priority:** High
**Environment:** Chrome, Windows 11

**Steps to Reproduce:**

1. Log in as a standard user.
2. Add a product to the cart.
3. Go to checkout.
4. Leave one required field empty.
5. Click Continue.

**Actual Result:**
The application allows the user to continue.

**Expected Result:**
The application should display a validation error message.

**Status:** Sample bug report for portfolio purposes.

## GitHub Actions

This project includes a GitHub Actions workflow that runs Playwright tests automatically after each push or pull request to the main branch.

The CI workflow:

* installs project dependencies,
* installs Playwright browsers,
* runs automated tests,
* uploads the Playwright HTML report as an artifact.


## Author

Created by: Agent Smith

This project was created as part of my learning way.
