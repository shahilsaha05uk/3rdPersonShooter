using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : AnimationAbstractScript
{
    public PlayerScript rigidbodyMovement;
    public PlayerHealth playerHealth;
    public GameManager manager;


    private void Start()
    {
        rigidbodyMovement = this.gameObject.GetComponent<PlayerScript>();
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    private void Update()
    {
        if (rigidbodyMovement.sprint == true)
        {
            Run(true);
        }
        else
        {
            Run(false);
        }
    }


    public void Run(bool b)
    {
        if (b)
        {
            rigidbodyMovement.m_animator.SetBool("Run", true);
        }
        else
        {
            rigidbodyMovement.m_animator.SetBool("Run", false);
        }
    }

}
