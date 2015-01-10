using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Soomla.Store;

public class SoomlaEditorWindow : EditorWindow {

	private List<string> singleUseItems = new List<string> ();
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
		
		this.ShowData();

		if(GUI.Button(new Rect(400, 220, 200, 50), "Generate"))
		{
			editorData.WriteToJSONFile(editorData.toJSONObject());
			editorData.generateSoomlaWorkFlow();
		}
	}


	void ShowData()
	{
		scrollPos = GUILayout.BeginScrollView (scrollPos, GUILayout.Width (this.position.width), GUILayout.Height (200));
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
		GUILayout.EndScrollView ();
	}

	void ShowGoods()
	{
		ShowHintFieldsForGoods ();

		for (int i = 0; i < editorData.goods.Count; i++)
		{
			GUILayout.BeginVertical();
			{
				ZFGood virtualGood = editorData.goods[i];
				ShowGood(virtualGood);
			}
			GUILayout.EndVertical();
		}
		ShowGood (editorData.newGood, true);
	}

	void ShowHintFieldsForGoods()	
	{
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label("ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
			GUILayout.Label("Name:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
			GUILayout.Label("Description:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.2f));
			GUILayout.Label("PurchaseType:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
			GUILayout.Label("GoodType:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
			GUILayout.Label("Price:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.06f));
		}	
		GUILayout.EndHorizontal ();
	}

	void ShowGood(ZFGood good, bool isNewGood = false)	
	{
		GUILayout.BeginHorizontal();
		{ 
			good.ID = GUILayout.TextField (good.ID, 16, EditorStyles.textField, GUILayout.Width (this.position.width*0.1f));
			good.ID = Regex.Replace(good.ID, "\n", "");
			good.name = GUILayout.TextField (good.name, 16, EditorStyles.textField, GUILayout.Width (this.position.width*0.1f));
			good.name = Regex.Replace(good.name, "\n", "");
			good.description = GUILayout.TextField (good.description, 100, EditorStyles.textField, GUILayout.Width (this.position.width*0.2f));
			good.description = Regex.Replace(good.description, "\n", "");
			good.typePurchase = (ZFGood.PurchaseInfo)EditorGUILayout.EnumPopup (good.typePurchase, EditorStyles.popup, GUILayout.Width(this.position.width*0.15f));
			good.goodType = (ZFGood.GoodType)EditorGUILayout.EnumPopup (good.goodType, EditorStyles.popup, GUILayout.Width(this.position.width*0.15f));
			if (good.typePurchase == ZFGood.PurchaseInfo.PurchaseWithMarket)
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
					
					int index = EditorGUILayout.Popup(indexInArray, currencyNames.ToArray(), GUILayout.Width(this.position.width*0.1f)); 
					good.virtualInfo.pvi_itemId = editorData.currencies[index].ID;
				}

				GUI.enabled = true;
			}
			if ( good.goodType == ZFGood.GoodType.SingleUsePackVG )
			{

				int id = 0;
				int index = EditorGUILayout.Popup(id, singleUseItems.ToArray(), GUILayout.Width(this.position.width*0.1f));
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
				if(GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(this.position.width*0.07f)))
				{
					editorData.goods.Remove(good);
				}
			}
		}
		GUILayout.EndHorizontal ();

		if (good.typePurchase == ZFGood.PurchaseInfo.PurchaseWithMarket)
		{
			GUILayout.BeginVertical();
			{
				GUILayout.BeginHorizontal();
				{
					good.marketInfo.useIos = GUILayout.Toggle(good.marketInfo.useIos, "iOSAppStore", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
					GUI.enabled = good.marketInfo.useIos;
					good.marketInfo.iosId = GUILayout.TextField(good.marketInfo.iosId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					good.marketInfo.useAndroid = GUILayout.Toggle(good.marketInfo.useAndroid, "GooglePlay", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
					GUI.enabled = good.marketInfo.useAndroid;
					good.marketInfo.androidId = GUILayout.TextField(good.marketInfo.androidId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					good.marketInfo.useAmazon = GUILayout.Toggle(good.marketInfo.useAmazon, "Amazon", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
					GUI.enabled = good.marketInfo.useAmazon;
					good.marketInfo.amazonId = GUILayout.TextField(good.marketInfo.amazonId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				{
					good.marketInfo.useWindowsPhone8 = GUILayout.Toggle(good.marketInfo.useWindowsPhone8, "WindowsPhone8", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
					GUI.enabled = good.marketInfo.useWindowsPhone8;
					good.marketInfo.windowsPhone8Id = GUILayout.TextField(good.marketInfo.windowsPhone8Id, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
					GUI.enabled = true;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		} 
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
		this.ShowHintFieldsForCurrencies ();

		for (int i = 0; i < editorData.currencies.Count; i++)
		{
			GUILayout.BeginVertical();
			{
				this.ShowCurrency(editorData.currencies[i]);
			}
			GUILayout.EndVertical();
		}
		ShowCurrency (editorData.newCurrency, true);
	}

	void ShowHintFieldsForCurrencies()
	{
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label("ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.15f));
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
				if(GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(this.position.width*0.1f)))
				{
					editorData.DeleteCurrency(currency);
				}
			}
		}
		GUILayout.EndHorizontal();
	}

	void ShowCurrencyPacks()
	{
		ShowHintFieldsForCurrencyPacks ();
		
		for (int i = 0; i < editorData.currencyPacks.Count; i++)
		{
			GUILayout.BeginVertical();
			{
				ZFCurrencyPack currencyPack = editorData.currencyPacks[i];
				showCurrencyPack(currencyPack);
			}
			GUILayout.EndVertical();
		}
		showCurrencyPack (editorData.newCurrencyPack, true);
	}

	void ShowHintFieldsForCurrencyPacks()
	{
		GUILayout.BeginHorizontal ();
		{
			GUILayout.Label("ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
			GUILayout.Label("Name:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
			GUILayout.Label("Description:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.2f));
			GUILayout.Label("Price:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.06f));
			GUILayout.Label("Amount:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.06f));
			GUILayout.Label("Currency ID:", EditorStyles.boldLabel, GUILayout.Width(this.position.width*0.1f));
		}
		GUILayout.EndHorizontal ();
	}

	void showCurrencyPack(ZFCurrencyPack currencyPack, bool isNewCurrencyPack = false)
	{
		GUILayout.BeginHorizontal ();
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
				if(GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(this.position.width*0.15f)))
				{
					editorData.currencyPacks.Remove(currencyPack);
				}
			}
			
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.BeginVertical();
		{
			GUILayout.BeginHorizontal();
			{
				currencyPack.marketInfo.useIos = GUILayout.Toggle(currencyPack.marketInfo.useIos, "iOSAppStore", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
				GUI.enabled = currencyPack.marketInfo.useIos;
				currencyPack.marketInfo.iosId = GUILayout.TextField(currencyPack.marketInfo.iosId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				currencyPack.marketInfo.useAndroid = GUILayout.Toggle(currencyPack.marketInfo.useAndroid, "GooglePlay", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
				GUI.enabled = currencyPack.marketInfo.useAndroid;
				currencyPack.marketInfo.androidId = GUILayout.TextField(currencyPack.marketInfo.androidId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				currencyPack.marketInfo.useAmazon = GUILayout.Toggle(currencyPack.marketInfo.useAmazon, "Amazon", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
				GUI.enabled = currencyPack.marketInfo.useAmazon;
				currencyPack.marketInfo.amazonId = GUILayout.TextField(currencyPack.marketInfo.amazonId, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				currencyPack.marketInfo.useWindowsPhone8 = GUILayout.Toggle(currencyPack.marketInfo.useWindowsPhone8, "WindowsPhone8", EditorStyles.toggle, GUILayout.Width(this.position.width*0.1f));
				GUI.enabled = currencyPack.marketInfo.useWindowsPhone8;
				currencyPack.marketInfo.windowsPhone8Id = GUILayout.TextField(currencyPack.marketInfo.windowsPhone8Id, EditorStyles.textField, GUILayout.Width(this.position.width*0.15f));
				GUI.enabled = true;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	void ShowCategories()
	{
		
	}
}