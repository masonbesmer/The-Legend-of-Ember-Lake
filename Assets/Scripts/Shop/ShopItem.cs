using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Shop/ShopItem", order = 1)]
public class ShopItem : ScriptableObject
{
    // Item prefab (to be instantiated)
    public GameObject itemPrefab = null;

    // Item name
    public string itemName = "New Item";
    // Item description
    public string itemDescription = "New Item Description";
    // Item price
    public int itemPrice = 10;
}
