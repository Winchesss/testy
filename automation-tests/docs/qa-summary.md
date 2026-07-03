# QA Summary

## Project Overview

This project demonstrates manual testing and automated end-to-end testing skills using Playwright and TypeScript.

The tested application is SauceDemo, a demo e-commerce web application.

## What Was Tested

The following areas were tested:

- login,
- product list,
- product sorting,
- cart,
- checkout,
- form validation,
- error handling.

## Automated Tests

Automated tests were created for the following scenarios:

- successful login,
- login with invalid password,
- product list visibility,
- sorting products by price,
- adding product to cart,
- removing product from cart,
- successful checkout,
- checkout validation with empty fields.

## Tools Used

- Playwright,
- TypeScript,
- Node.js,
- GitHub Actions,
- Markdown documentation.

## Test Design Approach

The tests were designed using:

- Page Object Model,
- reusable test data,
- positive and negative scenarios,
- clear assertions,
- readable test names.

## Result

The automated test suite validates the main e-commerce user flows and can be executed locally or in GitHub Actions.