using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    #region Health Variables
    private GameManager manager;
    public TextMeshProUGUI armourText;
    public TextMeshProUGUI healthText;

    public int health;
    public int armour = 100;
    public bool wasHit;
    #endregion

    private void OnEnable()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        health = 100;

        healthText.text = $"Health: {health}";
        healthText.color = manager.defaultColor;
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
            healthText.color = manager.green;
        }
        else if (health < 60 && health >= 30)
        {
            healthText.color = manager.yellow;
        }
        else if (health < 30)
        {
            healthText.color = manager.red;
        }

        healthText.text = $"Health: {health}";
        wasHit = true;
        StartCoroutine(GetComponent<EnemyScript>().AlertOn());
    }
}
