import { test, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { ProductsPage } from '../pages/ProductsPage';
import { CartPage } from '../pages/CartPage';
import { CheckoutPage } from '../pages/CheckoutPage';
import { users, checkoutUser } from '../test-data/users';

test.beforeEach(async ({ page }) => {
  const loginPage = new LoginPage(page);
  const productsPage = new ProductsPage(page);
  const cartPage = new CartPage(page);

  await loginPage.open();
  await loginPage.login(users.standard.username, users.standard.password);

  await expect(page).toHaveURL(/inventory/);

  await productsPage.addBackpackToCart();
  await productsPage.openCart();

  await expect(cartPage.backpackItem).toBeVisible();

  await cartPage.goToCheckout();
});

test('user can complete checkout with valid data', async ({ page }) => {
  const checkoutPage = new CheckoutPage(page);

  await checkoutPage.fillCustomerData(
    checkoutUser.firstName,
    checkoutUser.lastName,
    checkoutUser.postalCode
  );

  await checkoutPage.continueCheckout();

  await expect(checkoutPage.overviewTitle).toBeVisible();
  await expect(page.getByText('Sauce Labs Backpack')).toBeVisible();

  await checkoutPage.finishCheckout();

  await expect(checkoutPage.successMessage).toBeVisible();
});

test('user cannot continue checkout with empty required fields', async ({ page }) => {
  const checkoutPage = new CheckoutPage(page);

  await checkoutPage.continueCheckout();

  await expect(checkoutPage.errorMessage).toBeVisible();
  await expect(checkoutPage.errorMessage).toContainText('First Name is required');
});