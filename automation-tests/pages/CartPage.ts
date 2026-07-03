import { Page, Locator } from '@playwright/test';

export class CartPage {
  readonly page: Page;
  readonly backpackItem: Locator;
  readonly removeBackpackButton: Locator;
  readonly checkoutButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.backpackItem = page.getByText('Sauce Labs Backpack');
    this.removeBackpackButton = page.locator('[data-test="remove-sauce-labs-backpack"]');
    this.checkoutButton = page.locator('[data-test="checkout"]');
  }

  async removeBackpack() {
    await this.removeBackpackButton.click();
  }

  async goToCheckout() {
    await this.checkoutButton.click();
  }
}