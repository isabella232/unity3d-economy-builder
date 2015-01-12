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
	
	
	public const string ITEM_65_ITEM_ID = "item_65";
	public const string ITEM_65_PRODUCT_ID = "productId";
	
	public const string ITEM_653_ITEM_ID = "item_653";
	public const string ITEM_653_PRODUCT_ID = "productId";
	
	public const string ITEM_3_ITEM_ID = "item_3";
	#if UNITY_IOS
	public const string ITEM_3_PRODUCT_ID = "dsad";
	#elif UNITY_ANDROID
	public const string ITEM_3_PRODUCT_ID = "dsad";
	#else
	public const string ITEM_3_PRODUCT_ID = "";
	#endif
	
	public const string ITEM_1_ITEM_ID = "item_1";
	public const string ITEM_1_PRODUCT_ID = "";
	
	public const string ITEM_2_ITEM_ID = "item_2";
	public const string ITEM_2_PRODUCT_ID = "";
	
	
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
	
	public static VirtualGood ITEM_65 = new EquippableVG(
		EquippableVG.EquippingModel.LOCAL,
		"1",
		"Virtual Good Description",
		ITEM_65_ITEM_ID,
		new PurchaseWithMarket(ITEM_65_PRODUCT_ID, 0.99)
	);
	
	public static VirtualGood ITEM_653 = new LifetimeVG(
		"Virtual Good Name",
		"Virtual Good Description",
		ITEM_653_ITEM_ID,
		new PurchaseWithMarket(ITEM_653_PRODUCT_ID, 0.99)
	);
	
	public static VirtualGood ITEM_3 = new SingleUsePackVG(
		"item_2",
		0,
		"mnbjgh",
		"jghjgh",
		ITEM_3_ITEM_ID,
		new PurchaseWithMarket(ITEM_3_PRODUCT_ID, 0)
	);
	
	public static VirtualGood ITEM_1 = new SingleUseVG(
		"mnb",
		"mnb",
		ITEM_1_ITEM_ID,
		new PurchaseWithMarket(ITEM_1_PRODUCT_ID, 0)
	);
	
	public static VirtualGood ITEM_2 = new SingleUseVG(
		"mbnmm",
		"mnbmb",
		ITEM_2_ITEM_ID,
		new PurchaseWithMarket(ITEM_2_PRODUCT_ID, 0)
	);
	
	
	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
		"General", new List<string>(new string[] {CURRENCY_1_ITEM_ID, CURRENCY_2_ITEM_ID, CURRENCYPACK_1_ITEM_ID, CURRENCYPACK_2_ITEM_ID, ITEM_65_ITEM_ID, ITEM_653_ITEM_ID, ITEM_3_ITEM_ID, ITEM_1_ITEM_ID, ITEM_2_ITEM_ID})
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
		return new VirtualGood[]{ITEM_65, ITEM_653, ITEM_3, ITEM_1, ITEM_2};
	}
	
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}
	
}
