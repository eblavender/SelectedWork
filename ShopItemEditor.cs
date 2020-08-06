using Doozy.Engine.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ShopItem))]
public class ShopItemEditor : Editor
{
    // OnInspector GUI
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ShopItem item = (ShopItem)target;

        GUILayout.Space(10f);
        GUILayout.Label("Shop Item Info", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        if(item.itemType != ItemType.PlayerObject)
        {
            item.coinCostText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Coin Cost Text", item.coinCostText, typeof(TextMeshProUGUI), true);
            item.rubyCostText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Ruby Cost Text", item.rubyCostText, typeof(TextMeshProUGUI), true);
            item.buyWithCoins = (UIButton)EditorGUILayout.ObjectField("Buy with Coins Button", item.buyWithCoins, typeof(UIButton), true);
            item.buyWithPremium = (UIButton)EditorGUILayout.ObjectField("Buy with Rubies Button", item.buyWithPremium, typeof(UIButton), true);
        }

        switch (item.itemType)
        {
            case ItemType.Boss:
                item.bossType = (BossType)EditorGUILayout.EnumPopup("Boss Type", item.bossType);
                item.element = (BossElement)EditorGUILayout.EnumPopup("Boss Element", item.element);
                item.image = (RawImage)EditorGUILayout.ObjectField("Smokey Image", item.image, typeof(RawImage), true);
                item.bossDescription = (GameObject)EditorGUILayout.ObjectField("Boss Desciption", item.bossDescription, typeof(GameObject), true);
                break;
            case ItemType.PlayerObject:
                item.objectType = (ObjectType)EditorGUILayout.EnumPopup("Object Type", item.objectType);
                break;
            case ItemType.Currency:
                item.buyAmount = EditorGUILayout.IntField("Buy Amount", item.buyAmount);
                item.gainedCurrency = (CurrencyType)EditorGUILayout.EnumPopup("Currency Gained Type", item.gainedCurrency);
                break;
        }

        switch (item.costCurrency)
        {
            case CurrencyType.Coins:
                item.coinCost = EditorGUILayout.IntField("Coin Cost", item.coinCost);
                break;
            case CurrencyType.Rubies:
                item.rubyCost = EditorGUILayout.IntField("Ruby Cost", item.rubyCost);
                break;
            case CurrencyType.CoinsOrRubies:
                item.coinCost = EditorGUILayout.IntField("Coin Cost", item.coinCost);
                item.rubyCost = EditorGUILayout.IntField("Ruby Cost", item.rubyCost);
                break;
        }

        switch (item.buyType)
        {
            case BuyType.Repeatable:
                item.buyLimit = EditorGUILayout.IntField("Buy Limit", item.buyLimit);
                break;
            case BuyType.OneTime:
                item.isBought = EditorGUILayout.Toggle("Already Bought", item.isBought);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(item, "Equipment Item");
            EditorUtility.SetDirty(item);
        }
    }
}
