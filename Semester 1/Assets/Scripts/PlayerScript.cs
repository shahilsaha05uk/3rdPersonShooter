using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using Cinemachine;

public enum PlayerMovement { WALK, RUN, IDLE }; 
public class PlayerScript : MonoBehaviour
{
    public GameObject weapon;
    public Animator m_animator;
    private NavMeshAgent agent;

    public RaycastWeapon raycastWeapon;
    public ParticleSystem deadParticle;

    //Variables for locomotion
    public float moveSpeed;
    public float walk;
    public float run;
    public float idle;

    public bool isJumping;
    public bool isMoving;
    public bool sprint;
    public bool isAlive;
    private bool reload;

    public float h, v;
    [SerializeField]private float m_speed;

    [Space(10)][Header("Camera Shake")]
    public CinemachineFreeLook freeLook;

    Ray ray;
    public GameObject crossHair;
    public GameManager manager;
    public Camera cam;

    public Vector3 viewPoint;
    public Vector3 midPoint;
    public Vector3 weaponOffset;
    private Vector3 inputMove;
    
    public AnimatorClipInfo[] animationInfo;
    public LayerMask mask;

    public PlayerMovement movement;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        agent = GetComponent<NavMeshAgent>();

        crossHair = manager.crosshair;
        StartCoroutine(OnDead());

    }
    private void Start()
    {
        Init();
    }

    void Update()
    {
        //if(raycastWeapon.totalBullets <0 && raycastWeapon!= null)
        //{
            //raycastWeapon.StopFiring();
       // }
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);
        ray = cam.ViewportPointToRay(Vector3.one * 0.5f);

        if (isAlive)
        {
            //Input Checks
            float hori = Input.GetAxis("Horizontal");
            float verti = Input.GetAxis("Vertical");

            v += Input.GetAxis("Mouse Y");
            h += Input.GetAxis("Mouse X");

            inputMove = new Vector3(hori, 0, verti);
            
            Controls();
        }
    }
    private void FixedUpdate()
    {
        SpeedSet();
        MoveAnim();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("healthPickup") && GetComponent<PlayerHealth>().Health != 100)
        {

            GetComponent<PlayerHealth>().Health = 100;

            Destroy(other.gameObject);
            Debug.Log("Health Pickup");
        }
    }

    void Init()
    {
        isAlive = true;
        reload = false;

        midPoint = cam.ScreenToViewportPoint(crossHair.transform.position);
        m_animator.SetBool("Run", false);
        movement = PlayerMovement.WALK;
        
        StartCoroutine(Reload());
    }

    public void Controls()
    {
        //Sprint Check
        if (Input.GetKeyDown(KeyCode.LeftShift) && !sprint)
        {
            sprint = true;
            movement = PlayerMovement.RUN;

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && sprint)
        {
            sprint = false;
            movement = PlayerMovement.WALK;
        }
        //Shoot
        if (raycastWeapon != null)
        {
            if (Input.GetMouseButton(0) && !GameManager.canvasOpened)
            {
                if (manager.remainingBullets > 0)
                {
                    raycastWeapon.startBulletSound = true;
                    Shoot(true);
                }
                else
                {
                    raycastWeapon.StopFiring();
                }
            }
        }
        //Reload
        if (Input.GetKeyDown(KeyCode.R) && !reload)
        {
            reload = true;
        }

    }
    private void MoveAnim()
    {
        if (isAlive)
        {
            transform.rotation = Quaternion.Euler(0f, 1f * h, 0f);
            m_animator.SetFloat("VelocityZ", inputMove.z);
            m_animator.SetFloat("VelocityX", inputMove.x);
            transform.Translate(inputMove * Time.deltaTime * moveSpeed);
        }
    }
    public void SpeedSet()
    {
        switch (movement)
        {
            case PlayerMovement.WALK:
                moveSpeed = walk;
                break;
            case PlayerMovement.RUN:
                moveSpeed = run;
                break;
            case PlayerMovement.IDLE:
                moveSpeed = idle;
                break;
        }
    }

    public void Shoot(bool b)
    {
        if (b && raycastWeapon!= null && raycastWeapon.totalBullets>0)
        {
            RaycastHit hit;

            raycastWeapon.StartFiring();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("Hit " + hit.collider.name);

                if(hit.collider.CompareTag("Enemy"))
                {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    enemyHealth.Health(-1);
                }
            }
        }
    }
    private IEnumerator Reload()
    {
        while (isAlive)
        {
            if ((raycastWeapon != null && raycastWeapon.remainingBullets <= 0) || reload)
            {
                raycastWeapon.remainingBullets = 0;
                m_animator.SetTrigger("Reload");

                animationInfo = m_animator.GetCurrentAnimatorClipInfo(0);
                foreach (var item in animationInfo)
                {
                    if (item.clip.name == "Reload")
                    {
                        float reloadClipLength = item.clip.length;
                        yield return StartCoroutine(Wait(reloadClipLength));
                        raycastWeapon.totalRefills--;
                        raycastWeapon.remainingBullets = raycastWeapon.totalBullets;

                        m_animator.ResetTrigger("Reload");
                        break;
                    }
                }
                reload = false;
            }

            yield return null;
        }

    }
    private IEnumerator Wait(float timer)
    {
        yield return new WaitForSeconds(timer);
    }
    public IEnumerator OnDead()
    {
        while (true)
        {
            if(!isAlive)
            {
                Debug.Log("Dead player");
                GetComponent<CapsuleCollider>().height = 0.2f;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Destroy(GetComponent<NavMeshAgent>());
                Instantiate(deadParticle, transform.position, deadParticle.gameObject.transform.rotation);
                manager.CustomDestroy(this.gameObject,2f);
                m_animator.SetBool("isDead", true);
                yield break;
            }
            yield return null;
        }
    }

}
