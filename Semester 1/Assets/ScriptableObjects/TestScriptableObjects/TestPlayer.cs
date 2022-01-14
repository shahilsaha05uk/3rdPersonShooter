using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : testUpgrade
{
    [SerializeField]private int phealth = 0;

    private void Start()
    {
        UpdateHealth(100);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateHealth(100);
        }
    }
    public override void UpdateHealth(int health)
    {
        phealth += health;
        Debug.Log("Health: "+ phealth);
    }

}

