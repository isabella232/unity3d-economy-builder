using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

namespace YourAppNamespace {

	public class SoomlaAssets : IStoreAssets	{

		public const string ITEM_1_PRODUCT_ID = "item_1";

		public const string ITEM_2_PRODUCT_ID = "item_2";

		public const string ITEM_3_PRODUCT_ID = "item_3";

		public static VirtualGood ITEM_1 = new SingleUseVG(
			"mnb",
			"mnb",
			"item_1",
			new PurchaseWithMarket(ITEM_1_PRODUCT_ID, 0.0)
		);

		public static VirtualGood ITEM_2 = new SingleUseVG(
			"mbnmm",
			"mnbmb",
			"item_2",
			new PurchaseWithMarket(ITEM_2_PRODUCT_ID, 0.0)
		);

		public static VirtualGood ITEM_3 = new SingleUsePackVG(
			"item_2",
			6,
			"mnbjgh",
			"jghjgh",
			"item_3",
			new PurchaseWithMarket(ITEM_3_PRODUCT_ID, 0.0)
		);

		public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
			"General", new List<string>(new string[] {ITEM_1_PRODUCT_ID, ITEM_2_PRODUCT_ID, ITEM_3_PRODUCT_ID})
		);

		public int GetVersion()  {
			return 0;
		}

		public VirtualCurrency[] GetCurrencies()	{
			return new VirtualCurrency[]{};
		}

		public VirtualCurrencyPack[] GetCurrencyPacks()	{
			return new VirtualCurrencyPack[]{};
		}

		public VirtualGood[] GetGoods()	{
			return new VirtualGood[]{ITEM_1, ITEM_2, ITEM_3};
		}

		public VirtualCategory[] GetCategories()	{
			return new VirtualCategory[]{GENERAL_CATEGORY};
		}

	}
}