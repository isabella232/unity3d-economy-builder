using UnityEngine;
using System.Collections.Generic;
using Soomla.Store;
using System.IO;

public class MarketInfo
{

	public bool platformOverridesEnabled = false;

	public bool useIos = false;
	public string iosId = "";
	
	public bool useAndroid = false;
	public string androidId = "";
	
	public bool useAmazon = false;
	public string amazonId = "";
	
	public bool useWindowsPhone8 = false;
	public string windowsPhone8Id = "";
	
	public PurchaseType type = null;
	
	public string price = "0.0";
	
	enum Consumable
	{
		nonConsumable = 0,
		consumable,
		description
	}

	Consumable consumable = Consumable.nonConsumable;
	
	public MarketInfo(MarketInfo marketInfo)
	{
		this.useIos = marketInfo.useIos;
		this.iosId = marketInfo.iosId;
		this.useAndroid = marketInfo.useAndroid;
		this.androidId = marketInfo.androidId;
		this.useAmazon = marketInfo.useAmazon;
		this.amazonId = marketInfo.amazonId;
		this.useWindowsPhone8 = marketInfo.useWindowsPhone8;
		this.windowsPhone8Id = marketInfo.windowsPhone8Id;
		this.price = marketInfo.price;
		this.platformOverridesEnabled = useAndroid || useAndroid || useAmazon || useWindowsPhone8;
	}
	
	public MarketInfo() {}

	public void ClearFields()
	{
		this.useIos = false;
		this.iosId = "";
		this.useAndroid = false;
		this.androidId = "";
		this.useAmazon = false;
		this.amazonId = "";
		this.useWindowsPhone8 = false;
		this.windowsPhone8Id = "";
		this.price = "0.0";
		this.platformOverridesEnabled = false;
	}

	public bool ifMarketPurchaseFull()
	{
		if ((this.iosId != "" || this.androidId != "" || this.amazonId != "" || this.windowsPhone8Id != "") && this.price != "") {
			return true;
		} else {
			return false;
		}
	}

	public JSONObject toJSONObject()
	{
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject marketItem = new JSONObject (JSONObject.Type.OBJECT);
	
		if (this.platformOverridesEnabled) {
			if (this.useIos)
			{
				marketItem.AddField ("iosId", this.iosId);
			}
			if (this.useAndroid) 
			{
				marketItem.AddField("androidId", this.androidId);
			}
			if (this.useAmazon)
			{
				marketItem.AddField("amazonId", this.amazonId);
			}
			if (this.useWindowsPhone8)
			{
				marketItem.AddField("windowsPhone8Id", this.windowsPhone8Id);
			}
		}
		marketItem.AddField ("price", this.price);
		marketItem.AddField ("consumable", (int)this.consumable);
		
		json.AddField ("marketItem", marketItem);
		json.AddField("purchaseType", "market");
		
		return json;
	}

	public void fromJSONObject(JSONObject json)
	{
		JSONObject marketItem = json.GetField("marketItem");
		JSONObject jsonIosId = marketItem.GetField ("iosId");

		this.useIos = (jsonIosId != null);
		if (this.useIos)
		{
			this.iosId = jsonIosId.str;
		}
		
		JSONObject jsonAndroidId = marketItem.GetField ("androidId");
		this.useAndroid = (jsonAndroidId != null);
		if (this.useAndroid)
		{
			this.androidId = jsonAndroidId.str;
		}
		
		JSONObject jsonAmazonId = marketItem.GetField ("amazonId");
		this.useAmazon = (jsonAmazonId != null);
		if (this.useAmazon)
		{
			this.amazonId = jsonAmazonId.str;
		}
		
		JSONObject jsonWindowPhone8Id = marketItem.GetField ("windowsPhone8Id");
		this.useWindowsPhone8 = (jsonWindowPhone8Id != null);
		if (this.useWindowsPhone8)
		{
			this.windowsPhone8Id = jsonWindowPhone8Id.str;
		}
		
		this.platformOverridesEnabled = useAndroid || useAndroid || useAmazon || useWindowsPhone8;

		this.price = marketItem.GetField ("price").str;
		this.consumable = (Consumable)int.Parse(marketItem.GetField("consumable").ToString());
	}
}

public class VirtualInfo
{
	public string pvi_itemId = "";
	public string pvi_amount = "0";
	
	public VirtualInfo(VirtualInfo virtualInfo)
	{
		this.pvi_itemId = virtualInfo.pvi_itemId;
		this.pvi_amount = virtualInfo.pvi_amount;
	}
	
	public VirtualInfo() { }
	
	public void ClearFields()
	{
		this.pvi_itemId = "";
		this.pvi_amount = "0";
	}
	
	public bool ifVirtualPurchaseFull()
	{
		if (pvi_itemId != "" && this.pvi_amount != "")
			return true;
		else
			return false;
	}
	
	public JSONObject toJSONObject()
	{
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		
		json.AddField ("pvi_itemId", this.pvi_itemId);
		json.AddField ("pvi_amount", this.pvi_amount);
		json.AddField("purchaseType", "virtualItem");
		return json;
	}
	
	public void fromJSONObject(JSONObject json)
	{
		this.pvi_itemId = json.GetField ("pvi_itemId").str;
		this.pvi_amount = json.GetField ("pvi_amount").str;
	}
}
public class ZFGood	
{
	public string ID = "";
	public string name = "";
	public string description = "";
	public MarketInfo marketInfo = null;
	public VirtualInfo virtualInfo = null;

	
	public ZFGood()
	{
		this.ID = "item_";
		this.marketInfo = new MarketInfo();
		this.virtualInfo = new VirtualInfo();
	}
	
	public ZFGood(ZFGood goodInfo)
	{
		this.ID = goodInfo.ID;
		this.name = goodInfo.name;
		this.description = goodInfo.description;
		this.typePurchase = goodInfo.typePurchase;
		this.marketInfo = new MarketInfo(goodInfo.marketInfo);
		this.virtualInfo = new VirtualInfo(goodInfo.virtualInfo);
	}

	public void ClearFields()
	{
		this.ID = "";
		this.name = "";
		this.description = "";
		this.typePurchase = PurchaseInfo.Market;
		this.marketInfo.ClearFields();
		this.virtualInfo.ClearFields();
	}

	public bool ifGoodFull()
	{
		if (this.ID == "" || this.name == "" || this.description == "") {
			return false;
		} else {
			if(this.typePurchase == PurchaseInfo.Market)
			{
				if(this.marketInfo.ifMarketPurchaseFull())
					return true;
				else
					return false;
			}
			else
			{
				if(this.virtualInfo.ifVirtualPurchaseFull())
					return true;
				else
					return false;
			}
		}
	}
	
	public JSONObject toJSONObject()
	{
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.AddField ("ID", this.ID);
		json.AddField ("name", this.name);
		json.AddField ("description", this.description);
		if (this.typePurchase == PurchaseInfo.Market) 
		{
			json.AddField("purchasableItem", marketInfo.toJSONObject());
			
		}
		else if (this.typePurchase == PurchaseInfo.VirtualItem)
		{
			json.AddField("purchasableItem", virtualInfo.toJSONObject());
		}
		
		return json;
	}
	
	public void fromJSONObject(JSONObject json)
	{
		this.ID = json.GetField("ID").str;
		this.name = json.GetField("name").str;
		this.description = json.GetField("description").str;
		JSONObject jsonPurchasebleItem = json.GetField ("purchasableItem");
		string purchaseTypeString = jsonPurchasebleItem.GetField ("purchaseType").str;
		if (string.Equals(purchaseTypeString, "market"))
		{
			this.typePurchase = PurchaseInfo.Market;
			this.marketInfo.fromJSONObject(jsonPurchasebleItem);
		}
		else
		{
			this.typePurchase = PurchaseInfo.VirtualItem;
			this.virtualInfo.fromJSONObject(jsonPurchasebleItem);
		}
	}
	
	public enum PurchaseInfo
	{
		Market = 0,
		VirtualItem
	}
	
	public PurchaseInfo typePurchase = PurchaseInfo.Market;

	public enum GoodType
	{
		lifetime = 0,
		equippable,
		singleUse,
		goodPacks,
		goodUpgrades,
		singleUsePack
	}

	public GoodType goodType = GoodType.singleUse;
}

public class ZFCurrency
{
	public string ID = "currency_";
	public string name = "";

	public ZFCurrency() { }
	public ZFCurrency(ZFCurrency currency)
	{
		this.ID = currency.ID;
		this.name = currency.name;
	}

	public void ClearFields()
	{
		this.ID = "";
		this.name = "";
	}

	public bool isCurrencyFull()
	{
		if (this.ID == "" || this.name == "") {
			return false;
		} else {
			return true;
		}
	}

	public JSONObject toJSONObject()
	{
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.AddField ("ID", this.ID);
		json.AddField ("name", this.name);
		return json;
	}
	
	public void fromJSONObject(JSONObject json)
	{
		this.ID = json.GetField("ID").str;
		this.name = json.GetField("name").str;
	}
}

public class ZFCurrencyPack	
{
	public string ID = "currencypack_";
	public string name = "";
	public string description = "";
	public MarketInfo marketInfo = null;
	
	
	public ZFCurrencyPack()
	{
		this.marketInfo = new MarketInfo();
	}
	
	public ZFCurrencyPack(ZFCurrencyPack currencyPack)
	{
		this.ID = currencyPack.ID;
		this.name = currencyPack.name;
		this.description = currencyPack.description;
		this.marketInfo = new MarketInfo(currencyPack.marketInfo);
	}
	
	public void ClearFields()
	{
		this.ID = "";
		this.name = "";
		this.description = "";
		this.marketInfo.ClearFields();
	}

	public bool isCurrencyPackFull()
	{
		if (this.ID == "" || this.name == "") {
			return false;
		} else {
			return true;
		}
	}
	
	public JSONObject toJSONObject()
	{
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.AddField ("ID", this.ID);
		json.AddField ("name", this.name);
		json.AddField ("description", this.description);
		json.AddField ("purchasableItem", marketInfo.toJSONObject()); 

		return json;
	}

	public void fromJSONObject(JSONObject json)
	{
		this.ID = json.GetField("ID").str;
		this.name = json.GetField("name").str;
		this.description = json.GetField("description").str;
		JSONObject jsonPurchasebleItem = json.GetField ("purchasableItem");
//		string purchaseTypeString = jsonPurchasebleItem.GetField ("purchaseType").str;
//		if (string.Equals(purchaseTypeString, "market"))
//		{
//			this.typePurchase = PurchaseInfo.PurchaseWithMarket;
			this.marketInfo.fromJSONObject(jsonPurchasebleItem);
//		}
//		else
//		{
//			this.typePurchase = PurchaseInfo.PurchaseWithVirtualItem;
//			this.virtualInfo.fromJSONObject(jsonPurchasebleItem);
//		}
	}
}

public class SoomlaEditorData 
{
	public SoomlaEditorData()
	{
		this.InitObjects ();
	}
	
	public ZFGood newGood;
	public List<ZFGood> goods;
	

	public ZFCurrency newCurrency;
	public List<ZFCurrency> currencies;

	public ZFCurrencyPack newCurrencyPack;
	public List<ZFCurrencyPack> currencyPacks;

	public struct ZFCategory	
	{
		
	}
	public ZFCategory newCategory;
	public List<ZFCategory> categories;


	private void InitObjects()	
	{
		newGood = new ZFGood();
		goods = new List<ZFGood> ();
		newCurrency = new ZFCurrency ();
		currencies = new List<ZFCurrency> ();
		newCurrencyPack = new ZFCurrencyPack ();
		currencyPacks = new List<ZFCurrencyPack> ();
		newCategory = new ZFCategory ();
		categories = new List<ZFCategory> ();
	}

	public void AddGood(ZFGood.GoodType goodType) {
		ZFGood good = new ZFGood(newGood);
		good.goodType = goodType;
		good.ID = "item_" + (goods.Count + 1);
		goods.Add(good);
	}

	public bool isUniqueGood (ZFGood good)
	{
		for (int i = 0; i < this.goods.Count; i++) 
		{
			ZFGood currentGood = this.goods[i];
			
			if(currentGood.ID == good.ID || currentGood.name == good.name)
			{
				return false;
			}
		}
		return true;
	}

	public bool isUniqueCurrency(ZFCurrency currency)
	{
		for (int i = 0; i < this.currencies.Count; i++)	
		{
			ZFCurrency currentCurrency = this.currencies[i];

			if(currentCurrency.ID == currency.ID || currentCurrency.name == currency.name)
			{
				return false;
			}
		}
		return true;
	}

	public bool isUniqueCurrencyPack(ZFCurrencyPack currencyPack)
	{
		for (int i = 0; i < this.currencyPacks.Count; i++) 
		{
			ZFCurrencyPack currentCurrencyPack = this.currencyPacks[i];

			if(currentCurrencyPack.ID == currencyPack.ID || currentCurrencyPack.name == currencyPack.name)
			{
				return false;
			}
		}
		return true;
	}

	public void DeleteCurrency(ZFCurrency currency)
	{
		for (int i = 0; i < goods.Count; i++) 
		{
			if (goods[i].typePurchase == ZFGood.PurchaseInfo.VirtualItem && goods[i].virtualInfo.pvi_itemId == currency.ID)
			{
				goods.Remove(goods[i]);
			}
		}
		currencies.Remove (currency);
	}

	public void ReadFromJSONFile()
	{
		string path = @"SoomlaAssets.json";
		string jsonString = "";
		if (File.Exists(path))
		{
			using (StreamReader sr = File.OpenText(path))
			{
				jsonString = sr.ReadToEnd();
			}
		}
		else
		{
			Debug.Log ("File not exists");
			return;
		}
		JSONObject json = new JSONObject (jsonString);
		Debug.Log (json.ToString ());

		this.ParseJSONObject (json);
	}

	public void ParseJSONObject(JSONObject json)
	{
		JSONObject jsonCurrencies = json.GetField ("currencies");
		JSONObject jsonGoods = json.GetField ("goods");
		JSONObject jsonCurrencyPacks = json.GetField ("currencyPacks");
		if (jsonCurrencies.IsNull == false) 
		{
			foreach(JSONObject jsonCurrency in jsonCurrencies.list)
			{
				ZFCurrency currency = new ZFCurrency();
				currency.fromJSONObject(jsonCurrency);
				currencies.Add(currency);
			}
		}
		if (jsonGoods.IsNull == false) 
		{
			foreach(JSONObject jsonGood in jsonGoods.list)
			{
				ZFGood good = new ZFGood();
				good.fromJSONObject(jsonGood);
				goods.Add(good);
			}
		}

		if (jsonCurrencyPacks.IsNull == false)
		{
			foreach(JSONObject jsonCurrencyPack in jsonCurrencyPacks.list)
			{
				ZFCurrencyPack currencyPack = new ZFCurrencyPack();
				currencyPack.fromJSONObject(jsonCurrencyPack);
				currencyPacks.Add(currencyPack);
			}
		}
	}
	
	public void WriteToJSONFile(JSONObject obj)
	{
		string stringJSON = obj.ToString ();
		string path = @"SoomlaAssets.json";
		using (StreamWriter sw = File.CreateText(path))
		{
			sw.Write(stringJSON);
		}
		Debug.Log("Write to JSON");
	}

	public JSONObject toJSONObject()
	{
		JSONObject jsonCurrencies = new JSONObject(JSONObject.Type.ARRAY);
		for (int i = 0; i < currencies.Count; i++)
		{
			jsonCurrencies.Add(currencies[i].toJSONObject());
		}

		JSONObject jsonGoods = new JSONObject(JSONObject.Type.ARRAY);
		for (int i = 0; i < goods.Count; i++)
		{
			jsonGoods.Add(goods[i].toJSONObject());
		}

		JSONObject jsonCurrencyPacks = new JSONObject(JSONObject.Type.ARRAY);
		for (int i = 0; i < currencyPacks.Count; i++)
		{
			jsonCurrencyPacks.Add(currencyPacks[i].toJSONObject());
		}

		JSONObject json = new JSONObject (JSONObject.Type.OBJECT);
		json.AddField ("currencies", jsonCurrencies);
		json.AddField ("goods", jsonGoods);
		json.AddField ("currencyPacks", jsonCurrencyPacks);
		return json;
	}


}
