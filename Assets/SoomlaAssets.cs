using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/// <summary>
/// This class defines our game's economy, which includes virtual goods, virtual currencies
/// and currency packs, virtual categories
/// </summary>
public class SoomlaAssets : IStoreAssets {
	
	/** Static Final Members **/
	
	public const string CURRENCY_COINS_ITEM_ID = "currency_coins";
	
	
	public const string BAG_OF_COINS_ITEM_ID = "bag_of_coins";
	#if UNITY_IOS
	public const string BAG_OF_COINS_PRODUCT_ID = "com.my.game.packs.bag_of_coins";
	#elif UNITY_ANDROID
	public const string BAG_OF_COINS_PRODUCT_ID = "com.my.game.packs.bag_of_coins";
	#else
	public const string BAG_OF_COINS_PRODUCT_ID = "";
	#endif
	
	
	public const string DOUBLE_BARREL_RIFLE_ITEM_ID = "double_barrel_rifle";
	
	public const string SNIPER_RIFLE_ITEM_ID = "sniper_rifle";
	
	public const string REMOVE_ADS_ITEM_ID = "remove_ads";
	#if UNITY_IOS
	public const string REMOVE_ADS_PRODUCT_ID = "com.my.game.goods.remove_ads";
	#elif UNITY_ANDROID
	public const string REMOVE_ADS_PRODUCT_ID = "com.my.game.goods.remove_ads";
	#else
	public const string REMOVE_ADS_PRODUCT_ID = "";
	#endif
	
	public const string FIVE_CLIPS_ITEM_ID = "five_clips";
	
	public const string SINGLE_CLIP_ITEM_ID = "single_clip";
	
	
	
	/** Virtual Currencies **/
	
	public static VirtualCurrency CURRENCY_COINS = new VirtualCurrency(
		"Coins",					//name
		"",				//description
		CURRENCY_COINS_ITEM_ID				//item id
	);
	
	
	/** Virtual Currency Packs **/
	
	public static VirtualCurrencyPack BAG_OF_COINS = new VirtualCurrencyPack(
		"Bag of Coins",					//name
		"The most affordable pack",				//description
		BAG_OF_COINS_ITEM_ID,				//item id
		1000,				//number of currencies in the pack
		CURRENCY_COINS_ITEM_ID,				//the currency associated with this pack
		new PurchaseWithMarket(BAG_OF_COINS_PRODUCT_ID, 1.99)
	);
	
	
	/** Virtual Goods **/
	
	public static VirtualGood DOUBLE_BARREL_RIFLE = new EquippableVG(
		EquippableVG.EquippingModel.LOCAL,
		"Double Barrel Rifle",					//name
		"Use this gun for extra fire power and less accuracy",				//description
		DOUBLE_BARREL_RIFLE_ITEM_ID,				//item id
		new PurchaseWithVirtualItem(CURRENCY_COINS_ITEM_ID, 500)				//the way this virtual good is purchased
	);
	
	public static VirtualGood SNIPER_RIFLE = new EquippableVG(
		EquippableVG.EquippingModel.LOCAL,
		"Sniper Rifle",					//name
		"Use this gun for less power and much more accuracy",				//description
		SNIPER_RIFLE_ITEM_ID,				//item id
		new PurchaseWithVirtualItem(CURRENCY_COINS_ITEM_ID, 800)				//the way this virtual good is purchased
	);
	
	public static VirtualGood REMOVE_ADS = new LifetimeVG(
		"Remove Ads",					//name
		"Clean up your shooting field from ads forever",				//description
		REMOVE_ADS_ITEM_ID,				//item id
		new PurchaseWithMarket(REMOVE_ADS_PRODUCT_ID, 1.99)				//the way this virtual good is purchased
	);
	
	public static VirtualGood FIVE_CLIPS = new SingleUsePackVG(
		"single_clip",				//item id
		5,				//number of goods in the pack
		"Single clip",					//name
		"A single clip gives you 30 bullets to shoot",				//description
		FIVE_CLIPS_ITEM_ID,				//item id
		new PurchaseWithVirtualItem(CURRENCY_COINS_ITEM_ID, 40)				//the way this virtual good is purchased
	);
	
	public static VirtualGood SINGLE_CLIP = new SingleUseVG(
		"Single clip",					//name
		"A single clip gives you 30 bullets to shoot",				//description
		SINGLE_CLIP_ITEM_ID,				//item id
		new PurchaseWithVirtualItem(CURRENCY_COINS_ITEM_ID, 10)				//the way this virtual good is purchased
	);
	
	
	/** Virtual Categories **/
	// The muffin rush theme doesn't support categories, so we just put everything under a general category.
	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
		"General", new List<string>(new string[] {DOUBLE_BARREL_RIFLE_ITEM_ID, SNIPER_RIFLE_ITEM_ID, REMOVE_ADS_ITEM_ID, FIVE_CLIPS_ITEM_ID, SINGLE_CLIP_ITEM_ID})
	);
	
	/// <summary>
	/// see parent.
	/// </summary>
	public int GetVersion() {
		return 0;
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{CURRENCY_COINS};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[]{BAG_OF_COINS};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualGood[] GetGoods() {
		return new VirtualGood[]{DOUBLE_BARREL_RIFLE, SNIPER_RIFLE, REMOVE_ADS, FIVE_CLIPS, SINGLE_CLIP};
	}
	
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}
	
}
