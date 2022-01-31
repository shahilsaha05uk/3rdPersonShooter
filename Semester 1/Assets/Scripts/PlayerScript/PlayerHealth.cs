using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    private GameManager manager;
    public Image fillColor;
    public Slider healthBar;
    public Slider armourBar;

    public bool armourBought;
    public int Health { get; set; }
    public int armour = 100;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armourText;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Health = 100;
        armourBar = manager.playerArmourBar;
        armourText = manager.playerArmourText;
        armourBar.gameObject.SetActive(false);

        EnableHealthBar();
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
            healthText.color = manager.green;
            fillColor.color = manager.green;
        }
        else if (Health < 60 && Health >= 30)
        {
            healthText.color = manager.yellow;
            fillColor.color = manager.yellow;
        }
        else if (Health < 30)
        {
            healthText.color = manager.red;
            fillColor.color = manager.red;
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
    public void EnableHealthBar()
    {
        fillColor = manager.playerHealthFillColor;

        healthBar = manager.playerHealthBar;
        healthText = manager.playerHealthText;

        healthBar.minValue = 0;
        healthBar.maxValue = 100;
        healthBar.value = 100;

        healthText.color = manager.defaultColor;
        fillColor.color = manager.defaultColor;

        healthText.text = $"Health: {Health}";
    }

}
