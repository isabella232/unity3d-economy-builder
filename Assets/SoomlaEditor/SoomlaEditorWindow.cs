	using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Soomla.Store;

public class EconomyBuilder : EditorWindow 
{

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

	//string[] goodTypeOptions = {"Add Virtual Good", "Single Use", "Lifetime", "Equippable", "Upgradable", "Single Use Pack"};
	//without upgradable
	string[] goodTypeOptions = {"Add Virtual Good", "Single Use", "Lifetime", "Equippable", "Single Use Pack"}; 
	int goodTypeIndex = 0;
	
	private screens screenNumber = screens.goods;

	[MenuItem ("Window/Economy Builder")]
	static void Init ()
	{
		EconomyBuilder window = (EconomyBuilder)EditorWindow.GetWindow (typeof (EconomyBuilder));
		window.InitSoomlaEditorData ();
	}

	void InitSoomlaEditorData()
	{
		editorData = new SoomlaEditorData ();
		editorData.ReadFromJSONFile ();
		editorData.updateSingleUseItems();
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
			
			if (GUILayout.Toggle(screenNumber == screens.currencyPacks, "Currency Packs", EditorStyles.toolbarButton))
				screenNumber = screens.currencyPacks;
			
//			if (GUILayout.Toggle(screenNumber == screens.categories, "Categories", EditorStyles.toolbarButton))
//			{
//				screenNumber = screens.categories;
//			}
		}
		EditorGUILayout.EndHorizontal();
		/*
		if(GUILayout.Button("Generate"))
		{
			if(!editorData.areUniqueGoods() || !editorData.areUniqueCurrencies() || !editorData.areUniqueCurrencyPacks())
			{
				EditorUtility.DisplayDialog("ERROR", editorData.getResponseAboutSameItems(), "Ok");
			}
			else
			{
				editorData.WriteToJSONFile(editorData.toJSONObject());
				editorData.generateSoomlaAssets();
			}
		}*/

		if(editorData != null)
			this.ShowData();
		else
			InitSoomlaEditorData();

	}


	void ShowData()
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

	void ShowGoods()
	{
		EditorGUILayout.BeginHorizontal ();
		goodTypeIndex = EditorGUILayout.Popup(goodTypeIndex, goodTypeOptions, GUILayout.Width(100));

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
			/*else if (goodTypeIndex == 4) {
				editorData.AddGood(ZFGood.GoodType.UpgradeVG);
			}*/
			else if (goodTypeIndex == 4) {
				editorData.AddGood(ZFGood.GoodType.SingleUsePackVG);
			}

			editorData.updateSingleUseItems();

			goodTypeIndex = 0;
		}

		if(GUILayout.Button("Generate", GUILayout.Width(100), GUILayout.Height(15)))
		{
			if(!editorData.areUniqueGoods() || !editorData.areUniqueCurrencies() || !editorData.areUniqueCurrencyPacks())
			{
				EditorUtility.DisplayDialog("ERROR", editorData.getResponseAboutSameItems(), "Ok");
			}
			else
			{
				editorData.WriteToJSONFile(editorData.toJSONObject());
				editorData.generateSoomlaAssets();
				EditorUtility.DisplayDialog("", "File has been saved to path:/nAssets/SoomlaAssets.cs", "Ok");
			}
		}
		EditorGUILayout.EndHorizontal ();

		scrollPos = GUILayout.BeginScrollView (scrollPos);

		for (int i = 0; i < editorData.goods.Count; i++)
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			ShowGood(editorData.goods[i]);
			EditorGUILayout.EndVertical();	
		}

		GUILayout.EndScrollView ();
	}

	void ShowGood(ZFGood good)	
	{

		EditorGUILayout.BeginHorizontal();
		good.render = EditorGUILayout.Foldout(good.render, good.goodType.ToString() + "[" + good.ID + "]");
		GUIContent deleteButtonContent = new GUIContent ("X", "Press the button if you want to delete object");
		if(GUILayout.Button(deleteButtonContent, EditorStyles.miniButton, GUILayout.Width(20))) {
			editorData.DeleteGood(good);
		}
		EditorGUILayout.EndHorizontal();
        
		if (good.render) {
			EditorGUI.indentLevel++;
			good.ID = EditorGUILayout.TextField("Item ID ", good.ID);
			good.name = EditorGUILayout.TextField("Name", good.name);
			good.name = Regex.Replace(good.name, "\n", "");
			good.description = EditorGUILayout.TextField("Description", good.description);
			good.description = Regex.Replace(good.description, "\n", "");
			good.typePurchase = (ZFGood.PurchaseInfo)EditorGUILayout.EnumPopup("Purchase With", good.typePurchase);
			EditorGUI.indentLevel++;
            if (good.typePurchase == ZFGood.PurchaseInfo.Market) {
				good.marketInfo.price = EditorGUILayout.FloatField("Price", good.marketInfo.price);

				good.marketInfo.productId = EditorGUILayout.TextField("Product ID", good.marketInfo.productId);
                
				EditorGUI.indentLevel++;
                good.marketInfo.useIos = EditorGUILayout.BeginToggleGroup("iOS", good.marketInfo.useIos);
				if (good.marketInfo.useIos) {
					EditorGUI.indentLevel++;
					good.marketInfo.iosId = EditorGUILayout.TextField(good.marketInfo.iosId);
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.EndToggleGroup();
                
				good.marketInfo.useAndroid = EditorGUILayout.BeginToggleGroup("Android", good.marketInfo.useAndroid);
				if (good.marketInfo.useAndroid) {
					EditorGUI.indentLevel++;
					good.marketInfo.androidId = EditorGUILayout.TextField(good.marketInfo.androidId);
					EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndToggleGroup();

				EditorGUI.indentLevel--;
            } else {
				if (editorData.currencies.Count > 0) {
					good.virtualInfo.pvi_amount = EditorGUILayout.IntField("Price", good.virtualInfo.pvi_amount);
					
					int indexInArray = 0;
					List <string> currencyNames = new List<string>();
					for (int i = 0; i < editorData.currencies.Count; i++)
					{
						currencyNames.Add(editorData.currencies[i].name);
						if (good.virtualInfo.pvi_itemId == null) {
							indexInArray = 0;
						}
						else if (editorData.currencies[i].ID == good.virtualInfo.pvi_itemId)
						{
							indexInArray = i;
						}
					}
					
					int index = EditorGUILayout.Popup("VG", indexInArray, currencyNames.ToArray()); 
					good.virtualInfo.pvi_itemId = editorData.currencies[index].ID;
				} else {
					EditorGUILayout.HelpBox("You have no defined currencies", MessageType.Warning, true);
				}
			}
			EditorGUI.indentLevel--;
            
            if (good.goodType == ZFGood.GoodType.SingleUsePackVG) {
				editorData.updateSingleUseItems();
				if (editorData.singleUseGoodsIDs.Count > 0) {
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
					int index = EditorGUILayout.Popup("Single Use Item", indexInArray, editorData.singleUseGoodsIDs.ToArray()); 
					good.good_itemId = editorData.singleUseGoodsIDs[index];

					EditorGUI.indentLevel++;
					good.good_amount = EditorGUILayout.IntField("Amount", good.good_amount);
					EditorGUI.indentLevel--;
				} else {
					EditorGUILayout.HelpBox("You should define a Single Use Item before", MessageType.Warning, true);
                }
			}

		    EditorGUI.indentLevel--;
		}
	}

	void ShowCurrencies()
	{
		if (GUILayout.Button ("Add Currency", EditorStyles.miniButton, GUILayout.Width(100))) 
		{
			editorData.AddCurrency();
		}

		scrollPos = GUILayout.BeginScrollView (scrollPos);
        
        for (int i = 0; i < editorData.currencies.Count; i++)
		{
			GUILayout.BeginVertical(GUI.skin.box);
			{
				this.ShowCurrency(editorData.currencies[i]);
			}
			GUILayout.EndVertical();
		}

		GUILayout.EndScrollView();
	}

	void ShowCurrency(ZFCurrency currency)
	{

		EditorGUILayout.BeginHorizontal();
		currency.render = EditorGUILayout.Foldout(currency.render, currency.ID);
		if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20))) {
			editorData.DeleteCurrency(currency);
		}
		EditorGUILayout.EndHorizontal();
		
		if (currency.render) {
            EditorGUI.indentLevel++;
            currency.ID = EditorGUILayout.TextField("Item ID ", currency.ID);
			currency.name = EditorGUILayout.TextField("Name", currency.name);
			currency.name = Regex.Replace(currency.name, "\n", "");
			EditorGUI.indentLevel--;
		}
	}

	void ShowCurrencyPacks()
	{
		if (GUILayout.Button ("Add Currency Pack", EditorStyles.miniButton, GUILayout.Width(100))) 
		{
			editorData.AddCurrencyPack();
		}

		scrollPos = GUILayout.BeginScrollView (scrollPos);
		for (int i = 0; i < editorData.currencyPacks.Count; i++)
		{
			GUILayout.BeginVertical(GUI.skin.box);
			{
				ZFCurrencyPack currencyPack = editorData.currencyPacks[i];
				ShowCurrencyPack(currencyPack);
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndScrollView();
	}

	void ShowCurrencyPack(ZFCurrencyPack currencyPack)
	{

		EditorGUILayout.BeginHorizontal();
		currencyPack.render = EditorGUILayout.Foldout(currencyPack.render, currencyPack.ID);
		if(GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20))) {
			editorData.currencyPacks.Remove(currencyPack);
		}
		EditorGUILayout.EndHorizontal();
		
		if (currencyPack.render) {
			EditorGUI.indentLevel++;
			currencyPack.ID = EditorGUILayout.TextField("Item ID ", currencyPack.ID);
			currencyPack.name = EditorGUILayout.TextField("Name", currencyPack.name);
			currencyPack.name = Regex.Replace(currencyPack.name, "\n", "");
			currencyPack.description = EditorGUILayout.TextField("Description", currencyPack.description);
			currencyPack.description = Regex.Replace(currencyPack.description, "\n", "");

			// market info
			currencyPack.marketInfo.price = EditorGUILayout.FloatField("Price", currencyPack.marketInfo.price);
			
			currencyPack.marketInfo.productId = EditorGUILayout.TextField("Product ID", currencyPack.marketInfo.productId);
			
			EditorGUI.indentLevel++;
			currencyPack.marketInfo.useIos = EditorGUILayout.BeginToggleGroup("iOS", currencyPack.marketInfo.useIos);
			if (currencyPack.marketInfo.useIos) {
				EditorGUI.indentLevel++;
				currencyPack.marketInfo.iosId = EditorGUILayout.TextField(currencyPack.marketInfo.iosId);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndToggleGroup();
			
			currencyPack.marketInfo.useAndroid = EditorGUILayout.BeginToggleGroup("Android", currencyPack.marketInfo.useAndroid);
			if (currencyPack.marketInfo.useAndroid) {
				EditorGUI.indentLevel++;
				currencyPack.marketInfo.androidId = EditorGUILayout.TextField(currencyPack.marketInfo.androidId);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndToggleGroup();
			EditorGUI.indentLevel--;

            // currency data
			if (editorData.currencies.Count > 0) {
				int indexInArray = 0;
				List <string> currencyNames = new List<string>();
				for (int i = 0; i < editorData.currencies.Count; i++)
				{
					currencyNames.Add(editorData.currencies[i].name);
					if (currencyPack.currency_itemId == null) {
						indexInArray = 0;
					}
					else if (editorData.currencies[i].ID == currencyPack.currency_itemId)
					{
						indexInArray = i;
					}
				}
				
				int index = EditorGUILayout.Popup("Currency", indexInArray, currencyNames.ToArray()); 
				currencyPack.currency_itemId = editorData.currencies[index].ID;
				EditorGUI.indentLevel++;
				currencyPack.currency_amount = EditorGUILayout.IntField("Amount", currencyPack.currency_amount);
				EditorGUI.indentLevel--;
			} else {
                EditorGUILayout.HelpBox("You have no defined currencies", MessageType.Warning, true);
            }
            
		
            EditorGUI.indentLevel--;
        }

	}

	void ShowCategories()
	{
		
	}
}