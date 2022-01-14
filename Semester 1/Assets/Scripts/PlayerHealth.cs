using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealth : MonoBehaviour
{


    bool isDead;
    private GameManager manager;
    [Space(10)]
    [Header("Player")]
    public Image fillColor;
    public Slider healthBar;
    public Slider armourBar;

    public bool armourBought;
    public int Health { get; set; }
    public int armour = 100;

    public Color32 fullHealth;
    public Color32 midHealth;
    public Color32 lowHealth;


    [Space(10)]
    [Header("Common")]
    //public int Health;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armourText;
    public Color defaultColor;

    private void Start()
    {
        manager = GetComponent<PlayerScript>().manager;

        Health = 100;

        healthBar = manager.playerHealthBar;
        healthText = manager.playerHealthText;

        armourBar = manager.playerArmourBar;
        armourText = manager.playerArmourText;

        fillColor = manager.playerHealthFillColor;
        
        
        defaultColor = new Color32((byte)0f, (byte)115f, (byte)0f, (byte)255f);

        healthBar.minValue = 0;
        healthBar.maxValue = 100;
        healthBar.value = 100;
        healthText.color = defaultColor;
        fillColor.color = defaultColor;

        armourBar.gameObject.SetActive(false);
        healthText.text = $"Health: {Health}";

    }

    private void Update()
    {
        HealthBarUpdate();

        if(Health <=0)
        {
            this.gameObject.GetComponent<PlayerScript>().isAlive = false;
        }

        if (armourBought)
        {
            EnableArmourBar();
            armourBought = false;
        }
    }

    public void Damage(int h)
    {
        if (armour > 0)
        {
            Armour(h);
        }
        else
        {
            Health += h;
        }
    }

    public void Armour(int h)
    {
        armour += h;
        armourBar.value = armour;

        armourText.text = $"Armour: {armourBar.value}";

    }
    public void HealthBarUpdate()
    {
        healthBar.value = Health;

        if (Health <= 70 && Health >= 60 || Health>70)
        {
            healthText.color = fullHealth;
            fillColor.color = fullHealth;
        }
        else if (Health < 60 && Health >= 30)
        {
            healthText.color = midHealth;
            fillColor.color = midHealth;
        }
        else if (Health < 30)
        {
            healthText.color = lowHealth;
            fillColor.color = lowHealth;
        }

        healthText.text = $"Health: {Health}";
    }
    public void EnableArmourBar()
    {
        armour = 100;
        armourBar.gameObject.SetActive(true);
        armourBar.minValue = 0;
        armourBar.maxValue = 100;
        armourBar.value = armour;
        armourText.text = $"Armour: {armourBar.value}";

    }

}
