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
	// for pack
	public string good_itemId = "";
	public string good_amount = "0";

	public enum GoodType
	{
		LifetimeVG = 0,
		EquippableVG,
		SingleUseVG,
		SingleUsePackVG,
		UpgradeVG
	}
	public GoodType goodType;

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
		this.goodType = goodInfo.goodType;
	}

	public void ClearFields()
	{
		this.ID = "";
		this.name = "";
		this.description = "";
		this.typePurchase = PurchaseInfo.Market;
		this.marketInfo.ClearFields();
		this.virtualInfo.ClearFields();
		this.goodType = GoodType.LifetimeVG;
		this.good_itemId = "";
		this.good_amount = "0";
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
		json.AddField ("itemId", this.ID);
		json.AddField ("name", this.name);
		json.AddField ("description", this.description);
		if (this.goodType == ZFGood.GoodType.SingleUsePackVG)
		{
			json.AddField("good_itemId", this.good_itemId);
			json.AddField("good_amount", this.good_amount);
		}
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
		this.ID = json.GetField("itemId").str;
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

		if (this.goodType == ZFGood.GoodType.SingleUsePackVG)
		{
			this.good_itemId = json.GetField("good_itemId").str;
			this.good_amount = json.GetField("good_amount").str;
		}
	}
	
	public enum PurchaseInfo
	{
		Market = 0,
		VirtualItem
	}
	
	public PurchaseInfo typePurchase = PurchaseInfo.Market;
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
	public string currency_itemId = "";
	public string currency_amount = "";
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
		this.currency_itemId = currencyPack.currency_itemId;
		this.currency_amount = currencyPack.currency_amount;
		this.marketInfo = new MarketInfo(currencyPack.marketInfo);
	}
	
	public void ClearFields()
	{
		this.ID = "";
		this.name = "";
		this.description = "";
		this.currency_itemId = "";
		this.currency_amount = "";
		this.marketInfo.ClearFields();
	}

	public bool isCurrencyPackFull()
	{
		if (this.ID == "" || this.name == "" || this.currency_itemId == "" || this.currency_amount == "") {
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
		json.AddField ("currency_itemId", this.currency_itemId);
		json.AddField ("currency_amount", this.currency_amount);
		json.AddField ("purchasableItem", marketInfo.toJSONObject()); 

		return json;
	}

	public void fromJSONObject(JSONObject json)
	{
		this.ID = json.GetField("ID").str;
		this.name = json.GetField("name").str;
		this.description = json.GetField("description").str;
		this.currency_itemId = json.GetField ("currency_itemId").str;
		this.currency_amount = json.GetField ("currency_amount").str;
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
	public List<string> singleUseGoodsIDs;
	public List<string> singleUseGoodsAmounts; 

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
		singleUseGoodsIDs = new List<string> ();
		singleUseGoodsAmounts = new List<string> ();
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

	public void DeleteGood(ZFGood good)
	{
		for ( int i = 0; i < goods.Count; i++)
		{
			if (goods[i].goodType == ZFGood.GoodType.SingleUsePackVG && goods[i].good_itemId == good.ID)
			{
				goods.Remove(goods[i]);
			}
		}
		goods.Remove (good);
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
			JSONObject jsonEquippableVGs = jsonGoods.GetField("equippable");
			JSONObject jsonLifetimeVGs = jsonGoods.GetField("lifetime");
			JSONObject jsonSingleUsePackVGs = jsonGoods.GetField("goodPacks");
			JSONObject jsonSingleUseVGs = jsonGoods.GetField("singleUse");
			JSONObject jsonUpgradeVGs = jsonGoods.GetField("goodUpgrades");
			
			foreach(JSONObject jsonEquippableVG in jsonEquippableVGs.list)
			{
				ZFGood good = new ZFGood();
				good.fromJSONObject(jsonEquippableVG);
				good.goodType = ZFGood.GoodType.EquippableVG;
				goods.Add(good);
			}
			
			foreach(JSONObject jsonLifetimeVG in jsonLifetimeVGs.list)
			{
				ZFGood good = new ZFGood();
				good.fromJSONObject(jsonLifetimeVG);
				good.goodType = ZFGood.GoodType.LifetimeVG;
				goods.Add(good);
			}
			
			foreach(JSONObject jsonSingleUsePackVG in jsonSingleUsePackVGs.list)
			{
				ZFGood good = new ZFGood();
				good.goodType = ZFGood.GoodType.SingleUsePackVG;
				good.fromJSONObject(jsonSingleUsePackVG);
				goods.Add(good);
			}
			
			foreach(JSONObject jsonSingleUseVG in jsonSingleUseVGs.list)
			{
				ZFGood good = new ZFGood();
				good.fromJSONObject(jsonSingleUseVG);
				good.goodType = ZFGood.GoodType.SingleUseVG;
				goods.Add(good);
			}
			
			foreach(JSONObject jsonUpgradeVG in jsonUpgradeVGs.list)
			{
				ZFGood good = new ZFGood();
				good.fromJSONObject(jsonUpgradeVG);
				good.goodType = ZFGood.GoodType.UpgradeVG;
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
		
		JSONObject jsonGoods = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject jsonEquippableVG = new JSONObject (JSONObject.Type.ARRAY);
		JSONObject jsonLifetimeVG = new JSONObject (JSONObject.Type.ARRAY);
		JSONObject jsonSingleUsePackVG = new JSONObject (JSONObject.Type.ARRAY);
		JSONObject jsonSingleUseVG = new JSONObject (JSONObject.Type.ARRAY);
		JSONObject jsonUpgradeVG = new JSONObject (JSONObject.Type.ARRAY);
		for (int i = 0; i < goods.Count; i++)
		{
			switch(goods[i].goodType)
			{
			case ZFGood.GoodType.EquippableVG:
			{
				jsonEquippableVG.Add(goods[i].toJSONObject());
			}
				break;
			case ZFGood.GoodType.LifetimeVG:
			{
				jsonLifetimeVG.Add(goods[i].toJSONObject());
			}
				break;
			case ZFGood.GoodType.SingleUsePackVG:
			{
				jsonSingleUsePackVG.Add(goods[i].toJSONObject());
			}
				break;
			case ZFGood.GoodType.SingleUseVG:
			{
				jsonSingleUseVG.Add(goods[i].toJSONObject());
			}
				break;
			case ZFGood.GoodType.UpgradeVG:
			{
				jsonUpgradeVG.Add(goods[i].toJSONObject());
			}
				break;
			}
		}
		
		jsonGoods.AddField ("lifetime", jsonLifetimeVG);
		jsonGoods.AddField ("equippable", jsonEquippableVG);
		jsonGoods.AddField ("singleUse", jsonSingleUseVG);
		jsonGoods.AddField ("goodPacks", jsonSingleUsePackVG);
		jsonGoods.AddField ("goodUpgrades", jsonUpgradeVG);
		
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

	public void collectSingleUseItems()
	{
		singleUseGoodsIDs.Clear ();
		singleUseGoodsAmounts.Clear ();
		for (int i = 0; i < goods.Count; i++) {
			if(goods[i].goodType == ZFGood.GoodType.SingleUseVG)
			{
				singleUseGoodsIDs.Add(goods[i].ID);
				string str = "0";
				singleUseGoodsAmounts.Add(str);
			}
		}
	}

	public void generateSoomlaWorkFlow()
	{

		//beginning creating script (1)
		string strLibraries = "using UnityEngine;\nusing System.Collections;\nusing System.Collections.Generic;\nusing Soomla.Store;\n\n";
		string strCreatingClass = "namespace YourAppNamespace {\n\n\tpublic class SoomlaAssets : IStoreAssets\t{\n\n";

		//ending creating script()
		string closeScript = "\t}\n}";
		string allVariables = "";
		//making variables (2)
		List<string> variablesStr = new List<string>();	
		for (int i = 0; i < currencies.Count; i++) {
			string item_id = currencies[i].ID.ToUpper() + "_ITEM_ID = \"" + currencies[i].ID + "\";\n\n";
			string str = "\t\tpublic const string " + item_id;
			allVariables += currencies[i].ID.ToUpper() + "_ITEM_ID, ";
			variablesStr.Add(str);
		}

		for (int i = 0; i < currencyPacks.Count; i++) {
			string item_id = currencyPacks[i].ID.ToUpper() + "_PRODUCT_ID = \"" + currencyPacks[i].ID + "\";\n\n";
			string str = "\t\tpublic const string " + item_id;
			allVariables += currencyPacks[i].ID.ToUpper() + "_PRODUCT_ID, ";
			variablesStr.Add(str);
		}

		for (int i = 0; i < goods.Count; i++) {
			string itemOrProduct = "";
			if(goods[i].typePurchase == ZFGood.PurchaseInfo.Market)
				itemOrProduct = "_PRODUCT_ID";
			else
				itemOrProduct = "_ITEM_ID";
			string item_id = goods[i].ID.ToUpper() + itemOrProduct + " = \"" + goods[i].ID + "\";\n\n";
			string str = "\t\tpublic const string " + item_id;
			allVariables += goods[i].ID.ToUpper() + itemOrProduct + ", ";
			variablesStr.Add(str);
		}

		List<string> constructorsStr = new List<string> ();
		//creating constructors for each soomla object
		for (int i = 0; i < currencies.Count; i++) {
			string constructor = currencies[i].ID.ToUpper() + " = new VirtualCurrency(\n" +
				"\t\t\t\"" + currencies[i].name + "\",\n" +
				"\t\t\t\"\",\n\t\t\t" +
					currencies[i].ID.ToUpper() + "_ITEM_ID\n" +
					"\t\t);\n\n";
			string str = "\t\tpublic static VirtualCurrency " + constructor;
			constructorsStr.Add(str);
		}

		for (int i = 0; i < currencyPacks.Count; i++) {
			string constructor = currencyPacks[i].ID.ToUpper() + " = new VirtualCurrencyPack(\n" +
				"\t\t\t\"" + currencyPacks[i].name + "\",\n" +
					"\t\t\t\"" + currencyPacks[i].description + "\",\n" +
					"\t\t\t\"" + currencyPacks[i].ID + "\",\n" +
					"\t\t\t" + currencyPacks[i].currency_amount +",\n" +
					"\t\t\t" + currencyPacks[i].currency_itemId.ToUpper() + "_ITEM_ID,\n" +
					"\t\t\tnew PurchaseWithMarket(" + currencyPacks[i].ID.ToUpper() + "_PRODUCT_ID, " + currencyPacks[i].marketInfo.price + ")\n" +
					"\t\t);\n\n";
			string str = "\t\tpublic static VirtualCurrencyPack " + constructor;
			constructorsStr.Add(str);
		}

		for (int i = 0; i < goods.Count; i++) { 
			string initMethod = "";
			if(goods[i].typePurchase == ZFGood.PurchaseInfo.Market)
				initMethod = "new PurchaseWithMarket(" + goods[i].ID.ToUpper() + "_PRODUCT_ID, " + goods[i].marketInfo.price + ")";
			else
				initMethod = "new PurchaseWithVirtualItem(" + goods[i].virtualInfo.pvi_itemId.ToUpper() + "_ITEM_ID, " + goods[i].virtualInfo.pvi_amount + ")";

			string equipModel = "";
			if(goods[i].goodType == ZFGood.GoodType.EquippableVG)
				equipModel = "\t\t\tEquippableVG.EquippingModel.LOCAL,\n";
			else
				equipModel = "";
			
			string constructor;

			if (goods[i].goodType == ZFGood.GoodType.SingleUsePackVG)
			{
				constructor = goods[i].ID.ToUpper() + " = new " + goods[i].goodType + "(\n" + equipModel +
					"\t\t\t\"" + goods[i].good_itemId + "\",\n" +
					"\t\t\t" + goods[i].good_amount + ",\n" +
					"\t\t\t\"" + goods[i].name + "\",\n" +
						"\t\t\t\"" + goods[i].description + "\",\n" +
						"\t\t\t\"" + goods[i].ID + "\",\n" +
						"\t\t\t" + initMethod + "\n" +
						"\t\t);\n\n";
			}
			else
			{
				constructor = goods[i].ID.ToUpper() + " = new " + goods[i].goodType + "(\n" + equipModel +
					"\t\t\t\"" + goods[i].name + "\",\n" +
						"\t\t\t\"" + goods[i].description + "\",\n" +
						"\t\t\t\"" + goods[i].ID + "\",\n" +
						"\t\t\t" + initMethod + "\n" +
						"\t\t);\n\n";
			}
			string str = "\t\tpublic static VirtualGood " + constructor;
			constructorsStr.Add(str);
		}

		allVariables = allVariables.Remove (allVariables.Length - 2, 2);
		string addGeneralCategory = "\t\tpublic static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(\n" +
						"\t\t\t\"General\", new List<string>(new string[] {" + allVariables + "})\n\t\t);\n\n";

		//get() methods for Soomla objects
		string getVersionMethod = "\t\tpublic int GetVersion()  {\n\t\t\treturn 0;\n\t\t}\n\n";

		string currenciesSequence = "";
		for (int i = 0; i < currencies.Count; i++) {
			currenciesSequence += currencies[i].ID.ToUpper();
			if(i + 1 != currencies.Count)
				currenciesSequence += ", ";
		}
		string getCurrenciesMethod = "\t\tpublic VirtualCurrency[] GetCurrencies()\t{\n" +
			"\t\t\treturn new VirtualCurrency[]{" + currenciesSequence + "};\n\t\t}\n\n";

		string currencyPacksSequence = "";
		for (int i = 0; i < currencyPacks.Count; i++) {
			currencyPacksSequence += currencyPacks[i].ID.ToUpper();
			if(i + 1 != currencyPacks.Count)
				currencyPacksSequence += ", ";
		}
		string getCurrencyPacksMethod = "\t\tpublic VirtualCurrencyPack[] GetCurrencyPacks()\t{\n" +
			"\t\t\treturn new VirtualCurrencyPack[]{" + currencyPacksSequence + "};\n\t\t}\n\n";

		string goodsSequence = "";
		for (int i = 0; i < goods.Count; i++) {
			goodsSequence += goods[i].ID.ToUpper();
			if(i + 1 != goods.Count)
				goodsSequence += ", ";
		}
		string getGoodsMethod = "\t\tpublic VirtualGood[] GetGoods()\t{\n" +
						"\t\t\treturn new VirtualGood[]{" + goodsSequence + "};\n\t\t}\n\n";

		string getCategories = "\t\tpublic VirtualCategory[] GetCategories()\t{\n" +
						"\t\t\treturn new VirtualCategory[]{GENERAL_CATEGORY};\n\t\t}\n\n";

		List<string> productIdentifiers = new List<string> ();

		string path = @"Assets/SoomlaAssets.cs";
		using (StreamWriter sw = File.CreateText(path))
		{
			sw.Write(strLibraries);
			sw.Write(strCreatingClass);
			for (int i = 0; i < variablesStr.Count; i++)
			{
				sw.Write(variablesStr[i]);
			}
			for (int i = 0; i < constructorsStr.Count; i++)
			{
				sw.Write(constructorsStr[i]);
			}
			sw.Write(addGeneralCategory);			//it's optional yet

			sw.Write(getVersionMethod);
			sw.Write(getCurrenciesMethod);
			sw.Write(getCurrencyPacksMethod);
			sw.Write(getGoodsMethod);
			sw.Write(getCategories);				//it's optional yet
			sw.Write(closeScript);
		}
	}
}
