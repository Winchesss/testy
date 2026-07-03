import { Page, Locator } from '@playwright/test';

export class ProductsPage {
  readonly page: Page;
  readonly pageTitle: Locator;
  readonly cartLink: Locator;
  readonly backpackAddButton: Locator;
  readonly backpackRemoveButton: Locator;
  readonly productNames: Locator;
  readonly productPrices: Locator;
  readonly sortDropdown: Locator;

  constructor(page: Page) {
    this.page = page;
    this.pageTitle = page.getByText('Products');
    this.cartLink = page.locator('[data-test="shopping-cart-link"]');
    this.backpackAddButton = page.locator('[data-test="add-to-cart-sauce-labs-backpack"]');
    this.backpackRemoveButton = page.locator('[data-test="remove-sauce-labs-backpack"]');
    this.productNames = page.locator('.inventory_item_name');
    this.productPrices = page.locator('.inventory_item_price');
    this.sortDropdown = page.locator('[data-test="product-sort-container"]');
  }

  async addBackpackToCart() {
    await this.backpackAddButton.click();
  }

  async removeBackpackFromProductsPage() {
    await this.backpackRemoveButton.click();
  }

  async openCart() {
    await this.cartLink.click();
  }

  async sortByPriceLowToHigh() {
    await this.sortDropdown.selectOption('lohi');
  }

  async getProductPrices(): Promise<number[]> {
    const pricesText = await this.productPrices.allTextContents();

    return pricesText.map((price) =>
      Number(price.replace('$', '').trim())
    );
  }
}