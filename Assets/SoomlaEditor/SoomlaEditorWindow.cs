using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Soomla.Store;

public class SoomlaEditorWindow : EditorWindow 
{

	private int idForSinglePack = 0;
	private Vector2 scrollPos = Vector2.zero;
	private SoomlaEditorData editorData;
	private bool inited = false;

	private enum screens
	{
		goods = 0,
		currencies,
		currencyPacks,
		categories,
	};

	string[] goodTypeOptions = {"Add Virtual Good", "Single Use", "Lifetime", "Equippable", "Upgradable", "Single Use Pack"};
	int goodTypeIndex = 0;
	
	private screens screenNumber = screens.goods;

	[MenuItem ("Window/Soomla Editor Window")]
	static void Init ()
	{
		SoomlaEditorWindow window = (SoomlaEditorWindow)EditorWindow.GetWindow (typeof (SoomlaEditorWindow));
		window.InitSoomlaEditorData ();
	}

	void InitSoomlaEditorData()
	{
		editorData = new SoomlaEditorData ();
		editorData.ReadFromJSONFile ();
		editorData.collectSingleUseItems();
		inited = true;
	}

	public void OnGUI () 
	{
		if (!inited) {
			InitSoomlaEditorData();
		}

		EditorGUILayout.BeginHorizontal();
		{
			//EditorGUILayout.Toggle(
			if (GUILayout.Toggle(screenNumber == screens.goods, "Goods", EditorStyles.toolbarButton))
				screenNumber = screens.goods;
			
			if (GUILayout.Toggle(screenNumber == screens.currencies, "Currencies", EditorStyles.toolbarButton))
				screenNumber = screens.currencies;
			
			if (GUILayout.Toggle(screenNumber == screens.currencyPacks, "Currencies Packs", EditorStyles.toolbarButton))
				screenNumber = screens.currencyPacks;
			
			if (GUILayout.Toggle(screenNumber == screens.categories, "Categories", EditorStyles.toolbarButton))
			{
				screenNumber = screens.categories;
			}
		}
		EditorGUILayout.EndHorizontal();
		if(GUILayout.Button("Generate"))
		{
			editorData.WriteToJSONFile(editorData.toJSONObject());
			editorData.generateSoomlaWorkFlow();
		}

		this.ShowData();

	}


	void ShowData()
	{
		{
			switch (screenNumber) {
				case screens.goods:
					ShowGoods ();
					break;
				case screens.currencies:
					ShowCurrencies ();
					break;
				case screens.currencyPacks:
					ShowCurrencyPacks ();
					break;
				case screens.categories:
					ShowCategories ();
					break;
				default:
					break;
			}
		}
	}

	void ShowGoods()
	{
		goodTypeIndex = EditorGUILayout.Popup(goodTypeIndex, goodTypeOptions, GUILayout.Width(100));

		ShowHintFieldsForGoods ();
		
		scrollPos = GUILayout.BeginScrollView (scrollPos, GUILayout.Width (this.position.width), GUILayout.Height (340));

		if (goodTypeIndex > 0)
		{
			if (goodTypeIndex == 1) {
				editorData.AddGood(ZFGood.GoodType.SingleUseVG);
			}
			else if (goodTypeIndex == 2) {
				editorData.AddGood(ZFGood.GoodType.LifetimeVG);
			}
			else if (goodTypeIndex == 3) {
				editorData.AddGood(ZFGood.GoodType.EquippableVG);
			}
			else if (goodTypeIndex == 4) {
				editorData.AddGood(ZFGood.GoodType.UpgradeVG);
			}
			else if (goodTypeIndex == 5) {
				editorData.AddGood(ZFGood.GoodType.SingleUsePackVG);
			}

			editorData.collectSingleUseItems();

			ShowGood (editorData.newGood, true);
			goodTypeIndex = 0;
		}
		
		for (int i = 0; i < editorData.goods.Count; i++)
		{
			GUILayout.BeginVertical();
			{
				ZFGood virtualGood = editorData.goods[i];
				ShowGood(virtualGood);
			}
			GUILayout.EndVertical();
		}

//		ShowGood (editorData.newGood, true);
		GUILayout.EndScrollView ();
	}

	void ShowHintFieldsForGoods()	
	{
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label("Good Type:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
			GUILayout.Label("Item ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.10f));
			GUILayout.Label("Name:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.10f));
			GUILayout.Label("Description:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.2f));
			GUILayout.Label("Purchase With:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
			GUILayout.Label("Price:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.06f));
		}	
		GUILayout.EndHorizontal ();
	}

	void ShowGood(ZFGood good, bool isNewGood = false)	
	{
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		{ 
			GUILayout.Label(good.goodType.ToString(), EditorStyles.label, GUILayout.Width(this.position.width*0.15f));
			good.ID = GUILayout.TextField (good.ID, 16, EditorStyles.textField, GUILayout.Width (this.position.width*0.10f));
			good.ID = Regex.Replace(good.ID, "\n", "");
			good.name = GUILayout.TextField (good.name, 16, EditorStyles.textField, GUILayout.Width (this.position.width*0.10f));
			good.name = Regex.Replace(good.name, "\n", "");
			good.description = GUILayout.TextField (good.description, 100, EditorStyles.textField, GUILayout.Width (this.position.width*0.2f));
			good.description = Regex.Replace(good.description, "\n", "");
			good.typePurchase = (ZFGood.PurchaseInfo)EditorGUILayout.EnumPopup (good.typePurchase, EditorStyles.popup, GUILayout.Width(this.position.width*0.15f));
			if (good.typePurchase == ZFGood.PurchaseInfo.Market)
			{
				good.marketInfo.price = GUILayout.TextField (good.marketInfo.price, 4, EditorStyles.textField, GUILayout.Width(this.position.width*0.06f));
				good.marketInfo.price = Regex.Replace (good.marketInfo.price, "[^0-9, .]", "");
				good.marketInfo.price = editSettingsForPrice(good.marketInfo.price);
			}
			else
			{
				if (editorData.currencies.Count == 0)
				{
					GUI.enabled = false;
				}

				good.virtualInfo.pvi_amount = GUILayout.TextField (good.virtualInfo.pvi_amount, 5, EditorStyles.textField, GUILayout.Width(this.position.width*0.06f));
				good.virtualInfo.pvi_amount = Regex.Replace (good.virtualInfo.pvi_amount, "[^0-9]", "");
					
				if (editorData.currencies.Count != 0)
				{
					int indexInArray = 0;
					List <string> currencyNames = new List<string>();
					for (int i = 0; i < editorData.currencies.Count; i++)
					{
						currencyNames.Add(editorData.currencies[i].name);
						if (good.virtualInfo.pvi_itemId == null)
						{
							indexInArray = 0;
						}
						else if (editorData.currencies[i].ID == good.virtualInfo.pvi_itemId)
						{
							indexInArray = i;
						}
					}
					
					int index = EditorGUILayout.Popup(indexInArray, currencyNames.ToArray(), GUILayout.Width(this.position.width*0.14f)); 
					good.virtualInfo.pvi_itemId = editorData.currencies[index].ID;
				}

				GUI.enabled = true;
			}

			if(isNewGood)
			{
				if (GUILayout.Button ("Add Good", EditorStyles.miniButton, GUILayout.Width(this.position.width*0.07f))) 
				{
					if(good.ifGoodFull() && editorData.isUniqueGood(good))
					{
						editorData.goods.Add (new ZFGood(good));
						Debug.Log(editorData.toJSONObject().ToString());
						editorData.newGood.ClearFields();
						editorData.newGood.ID = "item_" + (editorData.goods.Count+1);
					}
				}

			}
			else
			{
				if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
				{
					editorData.DeleteGood(good);
				}
			}
		}
		GUILayout.EndHorizontal ();

		if (good.typePurchase == ZFGood.PurchaseInfo.Market)
		{
			good.marketInfo.platformOverridesEnabled = EditorGUILayout.BeginToggleGroup("Platform overrides", good.marketInfo.platformOverridesEnabled);
			if (good.marketInfo.platformOverridesEnabled) {
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(20);
					good.marketInfo.useIos = GUILayout.Toggle(good.marketInfo.useIos, "iOSAppStore", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = good.marketInfo.useIos;
					good.marketInfo.iosId = GUILayout.TextField(good.marketInfo.iosId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(20);
					good.marketInfo.useAndroid = GUILayout.Toggle(good.marketInfo.useAndroid, "GooglePlay", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = good.marketInfo.useAndroid;
					good.marketInfo.androidId = GUILayout.TextField(good.marketInfo.androidId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(20);
					good.marketInfo.useAmazon = GUILayout.Toggle(good.marketInfo.useAmazon, "Amazon", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = good.marketInfo.useAmazon;
					good.marketInfo.amazonId = GUILayout.TextField(good.marketInfo.amazonId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					GUILayout.Space(20);
					good.marketInfo.useWindowsPhone8 = GUILayout.Toggle(good.marketInfo.useWindowsPhone8, "WindowsPhone8", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = good.marketInfo.useWindowsPhone8;
					good.marketInfo.windowsPhone8Id = GUILayout.TextField(good.marketInfo.windowsPhone8Id, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
			}
			EditorGUILayout.EndToggleGroup();
		}



		if (good.goodType == ZFGood.GoodType.SingleUsePackVG) 
		{
			GUILayout.BeginHorizontal();
			if (editorData.singleUseGoodsIDs.Count == 0)
			{
				GUI.enabled = false;
			}
			good.good_amount = GUILayout.TextField (good.good_amount, 5, EditorStyles.textField, GUILayout.Width(this.position.width*0.06f));
			good.good_amount = Regex.Replace (good.good_amount, "[^0-9]", "");

			int indexInArray = 0;
			if (editorData.singleUseGoodsIDs.Count != 0)
			{
				for (int i = 0; i < editorData.singleUseGoodsIDs.Count; i++)
				{
					if (editorData.singleUseGoodsIDs[i] == good.good_itemId)
					{
						indexInArray = i;
					}
				}
			}
			int index = EditorGUILayout.Popup(indexInArray, editorData.singleUseGoodsIDs.ToArray(), GUILayout.Width(this.position.width*0.1f)); 
			good.good_itemId = editorData.singleUseGoodsIDs[index];


			GUI.enabled = true;
			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();
	}

	string editSettingsForPrice(string str)
	{
		bool isPointAlready = false;

		for (int i = 0; i < str.Length; i++)
		{
			char sym = str[i];
			if(sym == '.')
			{
				if(str.Length == 1)
				{
					str = "0.";
				}
				else
				{
					if(!isPointAlready)
						isPointAlready = true;
					else
					{
						str = str.Remove(i);
					}
				}
			}
		}

		return str;
	}
	
	void ShowCurrencies()
	{
		if (GUILayout.Button ("Add Currency", EditorStyles.miniButton, GUILayout.Width(100))) 
		{
			ZFCurrency currency = new ZFCurrency(editorData.newCurrency);
			currency.ID = "currency_" + (editorData.currencies.Count + 1);
			editorData.currencies.Add(currency);
		}

		this.ShowHintFieldsForCurrencies ();
		
		scrollPos = GUILayout.BeginScrollView (scrollPos, GUILayout.Width (this.position.width), GUILayout.Height (310));

		for (int i = 0; i < editorData.currencies.Count; i++)
		{
			GUILayout.BeginVertical("box");
			{
				this.ShowCurrency(editorData.currencies[i]);
			}
			GUILayout.EndVertical();
		}

		GUILayout.EndScrollView();
	}

	void ShowHintFieldsForCurrencies()
	{
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label("Item ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
			GUILayout.Label("Name:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
		}	
		GUILayout.EndHorizontal ();
	}

	void ShowCurrency(ZFCurrency currency, bool isNewCurrency = false)
	{
		GUILayout.BeginHorizontal();
		{ 
			currency.ID = GUILayout.TextField (currency.ID, 16, EditorStyles.textField, GUILayout.Width (this.position.width*0.15f));
			currency.ID = Regex.Replace(currency.ID, "\n", "");
			currency.name = GUILayout.TextField (currency.name, 16, EditorStyles.textField, GUILayout.Width (this.position.width*0.15f));
			currency.name = Regex.Replace(currency.name, "\n", "");

			if(isNewCurrency)
			{
				if (GUILayout.Button ("Add Currency", EditorStyles.miniButton, GUILayout.Width(this.position.width*0.1f))) 
				{
					if(currency.isCurrencyFull() && editorData.isUniqueCurrency(currency))
					{
						editorData.currencies.Add (new ZFCurrency(currency));
						Debug.Log(editorData.toJSONObject().ToString());
						editorData.newCurrency.ClearFields();
						editorData.newCurrency.ID = "currency_" + (editorData.currencies.Count+1);
					}
				}
			}
			else
			{
				if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
				{
					editorData.DeleteCurrency(currency);
				}
			}
		}
		GUILayout.EndHorizontal();
	}

	void ShowCurrencyPacks()
	{
		if (GUILayout.Button ("Add Currency Pack", EditorStyles.miniButton, GUILayout.Width(100))) 
		{
			ZFCurrencyPack currencyPack = new ZFCurrencyPack(editorData.newCurrencyPack);
			currencyPack.ID = "currencypack_" + (editorData.currencyPacks.Count + 1);
			editorData.currencyPacks.Add (new ZFCurrencyPack(currencyPack));
		}

		ShowHintFieldsForCurrencyPacks ();
		
		scrollPos = GUILayout.BeginScrollView (scrollPos, GUILayout.Width (this.position.width), GUILayout.Height (310));
		for (int i = 0; i < editorData.currencyPacks.Count; i++)
		{
			GUILayout.BeginVertical();
			{
				ZFCurrencyPack currencyPack = editorData.currencyPacks[i];
				showCurrencyPack(currencyPack);
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndScrollView();
	}

	void ShowHintFieldsForCurrencyPacks()
	{
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label("Item ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
			GUILayout.Label("Name:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
			GUILayout.Label("Description:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.2f));
			GUILayout.Label("Price:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.06f));
			GUILayout.Label("Amount:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.06f));
			GUILayout.Label("Currency:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
		}
		GUILayout.EndHorizontal ();
	}

	void showCurrencyPack(ZFCurrencyPack currencyPack, bool isNewCurrencyPack = false)
	{
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		{
			currencyPack.ID = GUILayout.TextField(currencyPack.ID, 16, EditorStyles.textField, GUILayout.Width(this.position.width*0.1f));
			currencyPack.name = GUILayout.TextField(currencyPack.name, 16, EditorStyles.textField, GUILayout.Width(this.position.width*0.1f));
			currencyPack.description = GUILayout.TextField(currencyPack.description, 16, EditorStyles.textField, GUILayout.Width(this.position.width*0.2f));
			currencyPack.marketInfo.price = GUILayout.TextField(currencyPack.marketInfo.price, 5, EditorStyles.textField, GUILayout.Width(this.position.width*0.06f));
			currencyPack.marketInfo.price = editSettingsForPrice(currencyPack.marketInfo.price);

			if (editorData.currencies.Count == 0)
			{
				GUI.enabled = false;
			}
			
			currencyPack.currency_amount = GUILayout.TextField (currencyPack.currency_amount, 5, EditorStyles.textField, GUILayout.Width(this.position.width*0.06f));
			currencyPack.currency_amount = Regex.Replace (currencyPack.currency_amount, "[^0-9]", "");
			
			if (editorData.currencies.Count != 0)
			{
				int indexInArray = 0;
				List <string> currencyNames = new List<string>();
				for (int i = 0; i < editorData.currencies.Count; i++)
				{
					currencyNames.Add(editorData.currencies[i].name);
					if (currencyPack.currency_itemId == null)
					{
						indexInArray = 0;
					}
					else if (editorData.currencies[i].ID == currencyPack.currency_itemId)
					{
						indexInArray = i;
					}
				}
				
				int index = EditorGUILayout.Popup(indexInArray, currencyNames.ToArray(), GUILayout.Width(this.position.width*0.1f)); 
				currencyPack.currency_itemId = editorData.currencies[index].ID;
			}
			
			GUI.enabled = true;

			if(isNewCurrencyPack)
			{
				if (GUILayout.Button ("Add Currency Pack", EditorStyles.miniButton, GUILayout.Width(this.position.width*0.15f))) 
				{
					if(currencyPack.isCurrencyPackFull() && editorData.isUniqueCurrencyPack(currencyPack))
					{
						editorData.currencyPacks.Add (new ZFCurrencyPack(currencyPack));
						Debug.Log(editorData.toJSONObject().ToString());
						editorData.newCurrencyPack.ClearFields();
						editorData.newCurrencyPack.ID = "currencypack_" + (editorData.currencyPacks.Count+1);
					}
				}
			}
			else
			{
				if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20)))
				{
					editorData.currencyPacks.Remove(currencyPack);
				}
			}
			
		}
		GUILayout.EndHorizontal ();
		
		currencyPack.marketInfo.platformOverridesEnabled = EditorGUILayout.BeginToggleGroup("Platform overrides", currencyPack.marketInfo.platformOverridesEnabled);
		if (currencyPack.marketInfo.platformOverridesEnabled) {
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				currencyPack.marketInfo.useIos = GUILayout.Toggle(currencyPack.marketInfo.useIos, "iOSAppStore", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = currencyPack.marketInfo.useIos;
				currencyPack.marketInfo.iosId = GUILayout.TextField(currencyPack.marketInfo.iosId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				currencyPack.marketInfo.useAndroid = GUILayout.Toggle(currencyPack.marketInfo.useAndroid, "GooglePlay", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = currencyPack.marketInfo.useAndroid;
				currencyPack.marketInfo.androidId = GUILayout.TextField(currencyPack.marketInfo.androidId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				currencyPack.marketInfo.useAmazon = GUILayout.Toggle(currencyPack.marketInfo.useAmazon, "Amazon", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = currencyPack.marketInfo.useAmazon;
				currencyPack.marketInfo.amazonId = GUILayout.TextField(currencyPack.marketInfo.amazonId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Space(20);
				currencyPack.marketInfo.useWindowsPhone8 = GUILayout.Toggle(currencyPack.marketInfo.useWindowsPhone8, "WindowsPhone8", EditorStyles.toggle, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = currencyPack.marketInfo.useWindowsPhone8;
				currencyPack.marketInfo.windowsPhone8Id = GUILayout.TextField(currencyPack.marketInfo.windowsPhone8Id, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
		}
		EditorGUILayout.EndToggleGroup();
		GUILayout.EndVertical();
	}

	void ShowCategories()
	{
		
	}
}