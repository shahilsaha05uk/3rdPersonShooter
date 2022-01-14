using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum States { WANDER, SHOOT};

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private List<Scriptable_Objects_Script> itemCollection;
    
    [SerializeField]  private GameObject enemyWeapon;
    [SerializeField]  private GameObject weaponSpawnPos;
    [HideInInspector] public GameObject target;

    public Vector3 playerPos;
    public States state;

    public ParticleSystem deadParticle;
    public ParticleSystem muzzleFlash;

    [HideInInspector]public NavMeshAgent agent;
    public GameManager manager;

    public int rand;
    public int rotWaitTime;
   
    public bool isAlive;
    public bool inRange;
    
    Ray ray;
    RaycastHit hit;
    
    public float defaultRadius;
    public Vector3 randomDir;

    private void Start()
    {
        Init();
    }


    private void Update()
    {
        ray = new Ray(weaponSpawnPos.transform.position, weaponSpawnPos.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (isAlive)
        {
            switch (state)
            {
                case States.WANDER:
                    if (!inRange)
                    {
                        Debug.Log("Not in Range");

                        if (agent.remainingDistance < agent.stoppingDistance)
                        {
                            Debug.Log("Changing the position");
                            NextPosition();
                        }
                    }
                    else
                    {
                        state = States.SHOOT;
                    }
                    break;
                case States.SHOOT:
                    if (inRange)
                    {
                        agent.ResetPath();

                        Vector3 direction = playerPos - weaponSpawnPos.transform.position;
                        direction.y = 0;
                        Quaternion rot = Quaternion.LookRotation(direction);

                        agent.transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, 0.1f);

                        Shoot(true);
                    }
                    else
                    {
                        state = States.WANDER;
                    }

                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Obstacles"))
        {
            NextPosition();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            inRange = true;
            playerPos = other.transform.position;
            Debug.Log("Player Spotted");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            inRange = false;
            playerPos = Vector3.zero;
        }
    }

    void EquipWeapon()
    {
        rand = Random.Range(0, itemCollection.Count);

        GameObject tempObj = Instantiate(itemCollection[rand].item);

        enemyWeapon = Instantiate(itemCollection[0].item);
        enemyWeapon.transform.SetParent(this.transform);
        enemyWeapon.transform.position = weaponSpawnPos.transform.position;

        Transform[] childObj = enemyWeapon.GetComponentsInChildren<Transform>();

        foreach (var item in childObj)
        {
            if (item.gameObject.CompareTag("Muzzle"))
            {
                muzzleFlash = item.gameObject.GetComponent<ParticleSystem>();
            }
        }

    }
    void Init()
    {
        isAlive = true;
        inRange = false;

        agent = GetComponent<NavMeshAgent>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        defaultRadius = GetComponent<SphereCollider>().radius;
        state = States.WANDER;
    
        EquipWeapon();
        StartCoroutine(OnDead());
    }

    private void NextPosition()
    {
        Vector3 point = Random.insideUnitSphere * 30;
        agent.SetDestination(point);
    }
    public void Shoot(bool b)
    {
        if (b && muzzleFlash!= null)
        {
            muzzleFlash.Play();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player health Reduce");
                    PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                    playerHealth.Damage(-1);

                    if(playerHealth.Health<=0)
                    {
                        Destroy(this.gameObject, 3f);
                    }

                }
            }
        }
    }

    public IEnumerator AlertOn()
    {
        GetComponent<SphereCollider>().radius = defaultRadius * 2;
        yield return new WaitForSeconds(10);
        GetComponent<SphereCollider>().radius = defaultRadius;
    }
    public IEnumerator OnDead()
    {
        while (true)
        {
            if (!isAlive)
            {
                Debug.Log("Dead player");
                GetComponent<CapsuleCollider>().height = 0.2f;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                Destroy(GetComponent<NavMeshAgent>());
                
                Instantiate(deadParticle, transform.position, deadParticle.gameObject.transform.rotation);
                manager.CustomDestroy(this.gameObject,2f);
               
                yield break;
            }
            yield return null;
        }
    }



}
