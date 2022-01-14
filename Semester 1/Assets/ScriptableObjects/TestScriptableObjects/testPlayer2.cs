using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer2 : testUpgrade
{
    [SerializeField] int shealth;

    private void Start()
    {
        UpdateHealth(20);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            UpdateHealth(100);
        }
    }

    public override void UpdateHealth(int health)
    {
        shealth += health;
        Debug.Log("Health: " + shealth);
    }
}
