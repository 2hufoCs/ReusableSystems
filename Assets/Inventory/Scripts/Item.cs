using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotTag { None, Head, Chest, Legs, Feet, Weapon, Shield, Accessory }

[CreateAssetMenu(menuName = "Item", fileName = "NewItem")]
public class Item : ScriptableObject
{
    [Header("Must be unique")]
    public int id;

    [Header("Basic required stuff")]
    new public string name;
    public string description;
    public Sprite sprite;
    public int basePrice;

    public SlotTag itemTag;

    [Header("If the item can be equipped")]
    public GameObject equipmentPrefab;
}
