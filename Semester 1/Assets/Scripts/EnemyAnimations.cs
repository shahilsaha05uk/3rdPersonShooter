using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        StartCoroutine(OnDead());
        //StartCoroutine(OnWalk());
    }

    private IEnumerator OnDead()
    {
        while (true)
        {
            if (GetComponent<EnemyScript>().isAlive == false)
            {
                anim.SetBool("isDead", true);
                Destroy(this.gameObject, 4f);
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator OnWalk()
    {
        while (true)
        {
            if (GetComponent<EnemyScript>().inRange)
            {
                anim.SetBool("Walk", false);
            }
            else
            {
                anim.SetBool("Walk", true);
            }
            yield return null;
        }
    }
}
