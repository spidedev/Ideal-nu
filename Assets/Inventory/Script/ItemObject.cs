using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    KeyItem,
    Object,
    Food
}
public abstract class ItemObject : ScriptableObject
{
    public Sprite sprite;
    public ItemType type;
    public string Name;
    public bool canUse, canToss;
    public int exp, health;
    [TextArea(15, 20)] 
    public string Bio;
}
