using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Shop List", menuName ="Items", order =1)]
public class Scriptable_Objects_Script : ScriptableObject
{
    public ItemType m_Type;
    public Sprite m_Sprite;
    public string m_ItemName;
    public int m_Cost;
    public GameObject item;

    public int gunAmmo;
    public int gunMagazine;

    public ItemType returnItemType() { return m_Type; }

}

public enum ItemType
{
    NONE,
    ARMOUR,
    RIFLES,
    AMMO,
    GUN,
    PISTOL
}