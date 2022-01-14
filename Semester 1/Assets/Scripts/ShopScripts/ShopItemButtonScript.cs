using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButtonScript : MonoBehaviour
{
    public event Action<Scriptable_Objects_Script> items;

    [SerializeField] private Button m_Button;
    [SerializeField] private TextMeshProUGUI m_NameText;
    [SerializeField] private TextMeshProUGUI m_CostText;
    [HideInInspector]public GameObject itemObject;
    [HideInInspector] public ItemType itemType;


    [SerializeField] private Image imageBox;
    private Scriptable_Objects_Script buttonData;

    public void Ref_ImageCanvas(Image img) { imageBox = img; }

    public void Init(Scriptable_Objects_Script item)
    {
        buttonData = item;

        m_NameText.text = buttonData.m_ItemName;
        m_CostText.text = $"${buttonData.m_Cost}";
        itemObject = buttonData.item;
        itemType = buttonData.m_Type;

    }


    public void BuyItem()
    {
        items?.Invoke(buttonData);
    }

    public void UpdateImage()
    {
        imageBox.sprite = buttonData.m_Sprite;
    }
    public void DefaultImage()
    {
        imageBox.sprite = null;
    }

}
