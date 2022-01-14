using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    #region Health Variables

    [Space(10)]
    [Header("Common")]
    public int health;
    public int armour = 100;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armourText;
    public Color defaultColor;
    public bool wasHit;

    #endregion

    private void Awake()
    {
        health = 100;

        healthText.text = $"Health: {health}";

        defaultColor = new Color32((byte)0f, (byte)115f, (byte)0f, (byte)255f);
        healthText.color = defaultColor;

    }


    public void Health(int h)
    {
        health += h;
        healthText.text = $"Health: {health}";

        if (health<=0)
        {
            health = 0;
            GetComponent<EnemyScript>().isAlive = false;
        }

        if (health <= 70 && health >= 60)
        {
            HealthBarColorChange((ColorChange)0);
        }
        else if (health < 60 && health >= 30)
        {
            HealthBarColorChange((ColorChange)1);
        }
        else if (health < 30)
        {
            HealthBarColorChange((ColorChange)2);
        }

        healthText.text = $"Health: {health}";
        wasHit = true;
        StartCoroutine(GetComponent<EnemyScript>().AlertOn());

    }
    void HealthBarColorChange(ColorChange c)
    {
        Color newColor;
        switch (c)
        {
            case ColorChange.LIGHT_GREEN:
                newColor = new Color32((byte)10f, (byte)255f, (byte)0f, (byte)255f);
                healthText.color = newColor;
                break;
            case ColorChange.YELLOW:
                newColor = new Color32((byte)255f, (byte)245f, (byte)0f, (byte)255f);
                healthText.color = newColor;
                break;
            case ColorChange.RED:
                newColor = new Color32((byte)255f, (byte)42f, (byte)0f, (byte)255f);
                healthText.color = newColor;
                break;
            case ColorChange.BLUE:
                newColor = new Color32((byte)23f, (byte)124f, (byte)155f, (byte)0f);
                armourText.color = newColor;
                break;

        }
    }
}
