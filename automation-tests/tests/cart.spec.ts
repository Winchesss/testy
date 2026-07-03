import { test, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { ProductsPage } from '../pages/ProductsPage';
import { CartPage } from '../pages/CartPage';
import { users } from '../test-data/users';

test.beforeEach(async ({ page }) => {
  const loginPage = new LoginPage(page);

  await loginPage.open();
  await loginPage.login(users.standard.username, users.standard.password);

  await expect(page).toHaveURL(/inventory/);
});

test('user can add product to cart', async ({ page }) => {
  const productsPage = new ProductsPage(page);
  const cartPage = new CartPage(page);

  await productsPage.addBackpackToCart();
  await productsPage.openCart();

  await expect(cartPage.backpackItem).toBeVisible();
});

test('user can remove product from cart', async ({ page }) => {
  const productsPage = new ProductsPage(page);
  const cartPage = new CartPage(page);

  await productsPage.addBackpackToCart();
  await productsPage.openCart();

  await expect(cartPage.backpackItem).toBeVisible();

  await cartPage.removeBackpack();

  await expect(cartPage.backpackItem).not.toBeVisible();
});