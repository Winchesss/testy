import { test, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { ProductsPage } from '../pages/ProductsPage';
import { users } from '../test-data/users';

test.beforeEach(async ({ page }) => {
  const loginPage = new LoginPage(page);

  await loginPage.open();
  await loginPage.login(users.standard.username, users.standard.password);

  await expect(page).toHaveURL(/inventory/);
});

test('product list is visible after login', async ({ page }) => {
  const productsPage = new ProductsPage(page);

  await expect(productsPage.pageTitle).toBeVisible();
  await expect(productsPage.productNames.first()).toBeVisible();

  const productCount = await productsPage.productNames.count();

  expect(productCount).toBeGreaterThan(0);
});

test('user can sort products by price from low to high', async ({ page }) => {
  const productsPage = new ProductsPage(page);

  await productsPage.sortByPriceLowToHigh();

  const actualPrices = await productsPage.getProductPrices();
  const expectedPrices = [...actualPrices].sort((a, b) => a - b);

  expect(actualPrices).toEqual(expectedPrices);
});