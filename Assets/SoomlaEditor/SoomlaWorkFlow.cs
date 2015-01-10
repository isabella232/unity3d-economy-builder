using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.Example {

	public class SoomlaWorkFlowAssets : IStoreAssets	{

		public const string CURRENCY_1_ITEM_ID = "currency_1";

		public const string CURRENCY_2_ITEM_ID = "currency_2";

		public const string CURRENCYPACK_2_PRODUCT_ID = "currencypack_2";

		public const string ITEM_2_PRODUCT_ID = "item_2";

		public const string ITEM__ITEM_ID = "item_";

		public const string ITEM_3_ITEM_ID = "item_3";

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
			"Buy, bitch!",
			"currencypack_2",
			20,
			CURRENCY_2_ITEM_ID,
			new PurchaseWithMarket(CURRENCYPACK_2_PRODUCT_ID, 2.33)
		);

		public static VirtualGood ITEM_2 = new LifetimeVG(
			"fdf",
			"fds",
			"item_2",
			new PurchaseWithMarket(ITEM_2_PRODUCT_ID, 1.99)
		);

		public static VirtualGood ITEM_ = new LifetimeVG(
			"dsa",
			"dasdas",
			"item_",
			new PurchaseWithVirtualItem(CURRENCY_1_ITEM_ID, 2)
		);

		public static VirtualGood ITEM_3 = new LifetimeVG(
			"daa",
			"assa",
			"item_3",
			new PurchaseWithVirtualItem(CURRENCY_1_ITEM_ID, 2)
		);

		public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
			"General", new List<string>(new string[] {CURRENCY_1_ITEM_ID, CURRENCY_2_ITEM_ID, CURRENCYPACK_2_PRODUCT_ID, ITEM_2_PRODUCT_ID, ITEM__ITEM_ID, ITEM_3_ITEM_ID})
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
			return new VirtualGood[]{ITEM_2, ITEM_, ITEM_3};
		}

		public VirtualCategory[] GetCategories()	{
			return new VirtualCategory[]{GENERAL_CATEGORY};
		}

	}
}