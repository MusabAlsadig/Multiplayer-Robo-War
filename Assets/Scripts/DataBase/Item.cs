using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{ 
    public enum Type
    {
        Guns,

    }

    public string name;
    public int price;
    public bool owned;
    public GameObject prefab;
}

[System.Serializable]
public class ItemCollection
{
    public string name;
    public Item.Type type;
    public Item[] Items;
}
