using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
public class SoomlaAssets : IStoreAssets {
	
	public const string CURRENCY_1_ITEM_ID = "currency_1";
	
	public const string CURRENCY_2_ITEM_ID = "currency_2";
	
	
	public const string CURRENCYPACK_1_ITEM_ID = "currencypack_1";
	public const string CURRENCYPACK_1_PRODUCT_ID = "productId";
	
	public const string CURRENCYPACK_2_ITEM_ID = "currencypack_2";
	public const string CURRENCYPACK_2_PRODUCT_ID = "productId";
	
	
	public const string ITEM_1_ITEM_ID = "item_1";
	public const string ITEM_1_PRODUCT_ID = "productId";
	
	
	public static VirtualCurrency CURRENCY_1 = new VirtualCurrency(
		"Currency Name",
		"",
		CURRENCY_1_ITEM_ID
	);
	
	public static VirtualCurrency CURRENCY_2 = new VirtualCurrency(
		"Currency Name",
		"",
		CURRENCY_2_ITEM_ID
	);
	
	public static VirtualCurrencyPack CURRENCYPACK_1 = new VirtualCurrencyPack(
		"Currency Pack Name",
		"Currency Pack Description",
		CURRENCYPACK_1_ITEM_ID,
		10,
		CURRENCY_1_ITEM_ID,
		new PurchaseWithMarket(CURRENCYPACK_1_PRODUCT_ID, 0.99)
	);
	
	public static VirtualCurrencyPack CURRENCYPACK_2 = new VirtualCurrencyPack(
		"Currency Pack Name",
		"Currency Pack Description",
		CURRENCYPACK_2_ITEM_ID,
		10,
		CURRENCY_1_ITEM_ID,
		new PurchaseWithMarket(CURRENCYPACK_2_PRODUCT_ID, 0.99)
	);
	
	public static VirtualGood ITEM_1 = new SingleUseVG(
		"Virtual Good Name",
		"Virtual Good Description",
		ITEM_1_ITEM_ID,
		new PurchaseWithMarket(ITEM_1_PRODUCT_ID, 0.99)
	);
	
	
	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
		"General", new List<string>(new string[] {ITEM_1_ITEM_ID})
	);
	
	public int GetVersion() {
		return 0;
	}
	
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{CURRENCY_1, CURRENCY_2};
	}
	
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[]{CURRENCYPACK_1, CURRENCYPACK_2};
	}
	
	public VirtualGood[] GetGoods() {
		return new VirtualGood[]{ITEM_1};
	}
	
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}
	
}
