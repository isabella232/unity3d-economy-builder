using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.Example {

	public class SoomlaWorkFlowAssets : IStoreAssets	{

		public const string CURRENCY_1_ITEM_ID = "currency_1";

		public const string CURRENCY_2_ITEM_ID = "currency_2";

		public const string CURRENCYPACK_2_PRODUCT_ID = "currencypack_2";

		public const string ITEM_33_PRODUCT_ID = "item_33";

		public const string ITEM_5_ITEM_ID = "item_5";

		public const string ITEM_6_PRODUCT_ID = "item_6";

		public const string ITEM_4_ITEM_ID = "item_4";

		public static VirtualCurrency CURRENCY_1 = new VirtualCurrency(
			"dsa",
			"",
			CURRENCY_1_ITEM_ID
		);

		public static VirtualCurrency CURRENCY_2 = new VirtualCurrency(
			"dsad",
			"",
			CURRENCY_2_ITEM_ID
		);

		public static VirtualCurrencyPack CURRENCYPACK_2 = new VirtualCurrencyPack(
			"gold bar",
			"Buy please!",
			"currencypack_2",
			20,
			CURRENCY_2_ITEM_ID,
			new PurchaseWithMarket(CURRENCYPACK_2_PRODUCT_ID, 2.33)
		);

		public static VirtualGood ITEM_33 = new LifetimeVG(
			"fdaf",
			"dasdas",
			"item_33",
			new PurchaseWithMarket(ITEM_33_PRODUCT_ID, 0.0)
		);

		public static VirtualGood ITEM_5 = new LifetimeVG(
			"dsaads",
			"dasdas",
			"item_5",
			new PurchaseWithVirtualItem(CURRENCY_2_ITEM_ID, 32)
		);

		public static VirtualGood ITEM_6 = new LifetimeVG(
			"ghfghfg",
			"hjkljb",
			"item_6",
			new PurchaseWithMarket(ITEM_6_PRODUCT_ID, 0.0)
		);

		public static VirtualGood ITEM_4 = new LifetimeVG(
			"dsa",
			"ewe",
			"item_4",
			new PurchaseWithVirtualItem(CURRENCY_1_ITEM_ID, 2)
		);

		public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
			"General", new List<string>(new string[] {CURRENCY_1_ITEM_ID, CURRENCY_2_ITEM_ID, CURRENCYPACK_2_PRODUCT_ID, ITEM_33_PRODUCT_ID, ITEM_5_ITEM_ID, ITEM_6_PRODUCT_ID, ITEM_4_ITEM_ID})
		);

		public int GetVersion()  {
			return 0;
		}

		public VirtualCurrency[] GetCurrencies()	{
			return new VirtualCurrency[]{CURRENCY_1, CURRENCY_2};
		}

		public VirtualCurrencyPack[] GetCurrencyPacks()	{
			return new VirtualCurrencyPack[]{CURRENCYPACK_2};
		}

		public VirtualGood[] GetGoods()	{
			return new VirtualGood[]{ITEM_33, ITEM_5, ITEM_6, ITEM_4};
		}

		public VirtualCategory[] GetCategories()	{
			return new VirtualCategory[]{GENERAL_CATEGORY};
		}

	}
}