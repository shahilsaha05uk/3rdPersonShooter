using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Space(10)][Header("Canvas")]

    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject restartCanvas;
    [SerializeField] private GameObject optionCanvas;
    [SerializeField] private GameObject gameLogPanel;
    public GameObject crosshair;

    [Space(10)][Header("Bool Checks")]
    public static bool canvasOpened;
    public bool playerIsDead;
    public bool enemyIsDead;
    public bool gameIsPaused;

    [Space(10)][Header("Text Updates")]
    [SerializeField] private TextMeshProUGUI txt_totalEnemy;
    [SerializeField] private TextMeshProUGUI txt_enemyLeft;
    [SerializeField] private TextMeshProUGUI txt_playerKills;
    [SerializeField] private TextMeshProUGUI txt_gunAmmo;
    [SerializeField] private TextMeshProUGUI txt_gunMagzine;
    [SerializeField] private TextMeshProUGUI txt_message;

    [SerializeField] private TextMeshProUGUI txt_levelUp;
    [SerializeField] private TextMeshProUGUI timerTxt;

    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerArmourText;

    [Space(10)]
    [Header("Gun Info")]
    public int remainingBullets;
    public int totalMagazines;
    public int totalBulletsInMagazine;

    [Space(10)]
    [Header("Level")]
    public int level;
    public bool levelCompleted = true;

    private RaycastWeapon weapon;
    public int buyRefills;

    [Space(2)][Header("Color Pallete")]
    public Color defaultColor;
    public Color green;
    public Color yellow;
    public Color red;

    [Space(10)][Header("Player Refs")]
    public Slider playerHealthBar;
    public Slider playerArmourBar;
    public Image playerHealthFillColor;

    public TextMeshProUGUI txt_Money;
    public List<GameObject> healthPickupList;

    public GameObject playerWeaponSpawnPoint;
    public GameObject[] healthPickupSpawnPoints;
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;
    
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public List<GameObject> enemyList;

    public GameObject healthPickup;

    public GameObject tempHealthPick;
    public GameObject tempWeapon;
    public GameObject tempPlayer;
    public GameObject tempEnemy;

    public static int playerKills;
    public int enemyCount;
    public int enemyKilled;
    public int money;
    public ParticleSystem isDeadParticle;

    public string messageToDisplay;
    public float timerForMessageDisplay;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        healthPickupList = new List<GameObject>();
        Cursor.lockState = CursorLockMode.Confined;
        level = 1;
        levelCompleted = false;

        shopCanvas.SetActive(false);
        restartCanvas.SetActive(false);
        gameLogPanel.SetActive(false);
        txt_levelUp.gameObject.SetActive(false);
        txt_message.gameObject.SetActive(false);
        optionCanvas.SetActive(false);

        gameIsPaused = true;
        playerIsDead = false;

        LevelIncrease(true);
        PauseGame(true);

        playerWeaponSpawnPoint = tempPlayer.transform.Find("WeaponSpawnPoint").gameObject;

        StartCoroutine(CountDownTimer(10));
        StartCoroutine(LevelManager());
        StartCoroutine(MessageCanvasUpdater());
        StartCoroutine(Controls());
        StartCoroutine(HUD_Update());
    }
    private IEnumerator Controls()
    {
        while (true)
        {
            //Buy Manu
            if ((Input.GetKeyDown(KeyCode.B) && !shopCanvas.activeInHierarchy) && !gameIsPaused)
            {
                shopCanvas.SetActive(true);
                canvasOpened = true;
                PauseGame(true);
            }
            else if (Input.GetKeyDown(KeyCode.B) && shopCanvas.activeInHierarchy)
            {
                shopCanvas.SetActive(false);
                canvasOpened = false;
                PauseGame(false);
            }

            //Option Menu
            if (Input.GetKeyDown(KeyCode.Escape) && !gameIsPaused)
            {
                canvasOpened = true;
                optionCanvas.SetActive(true);
                PauseGame(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && gameIsPaused)
            {
                canvasOpened = false;
                optionCanvas.SetActive(false);
                PauseGame(false);
            }

            //GameLog Canvas
            if (Input.GetKeyDown(KeyCode.Tab) && !gameLogPanel.activeInHierarchy)
            {
                canvasOpened = true;
                gameLogPanel.SetActive(true);
            }
            else if (Input.GetKeyUp(KeyCode.Tab) && gameLogPanel.activeInHierarchy)
            {
                gameLogPanel.SetActive(false);
                canvasOpened = false;
            }

            //Pause Game
            if (Input.GetKeyDown(KeyCode.P) && !gameIsPaused)
            {
                PauseGame(true);
            }
            else if (Input.GetKeyDown(KeyCode.P) && gameIsPaused)
            {
                PauseGame(false);
            }

            yield return null;
        }
    }
    public IEnumerator HUD_Update()
    {
        while (true)
        {

            txt_Money.text = $"Money: {money}";
            txt_totalEnemy.text = "Total Enemy: " + enemyCount;
            txt_enemyLeft.text = "Enemies Left: " + (enemyCount - enemyKilled);
            txt_playerKills.text = "Your Kills: " + playerKills;

            if (tempPlayer != null)
            {
                weapon = tempPlayer.GetComponent<PlayerScript>().raycastWeapon;
                if (tempPlayer.GetComponent<PlayerScript>().raycastWeapon != null)
                {
                    totalBulletsInMagazine = weapon.totalBullets;

                    remainingBullets = weapon.remainingBullets;
                    totalMagazines = weapon.totalRefills + buyRefills;

                    txt_gunAmmo.text = "Ammo: " + remainingBullets + $" | {totalBulletsInMagazine}";
                    txt_gunMagzine.text = "Magazines: " + totalMagazines;
                }
            }
            yield return null;
        }
    }
    public IEnumerator MessageCanvasUpdater()
    {
        while (true)
        {
            if (messageToDisplay != null)
            {
                txt_message.gameObject.SetActive(true);
                txt_message.text = messageToDisplay;
                yield return new WaitForSeconds(timerForMessageDisplay);
                txt_message.gameObject.SetActive(false);
                messageToDisplay = null;
            }
                yield return null;
        }
    }
    
    public void GameOver(GameObject obj)
    {
        Animator anim;
        obj.TryGetComponent<Animator>(out anim);

        if(anim!= null)
        {
            //activate the restart canvas
            anim.SetBool("isDead", true);
            ParticleSystem particle = Instantiate(isDeadParticle, obj.transform.position, isDeadParticle.transform.rotation);
            particle.Play();
            restartCanvas.SetActive(true);
            
        }
    }
    public bool PauseGame(bool gameIsPaused)
    {
        if (gameIsPaused)
        {
            this.gameIsPaused = true;
            Debug.Log("Game Paused");
           Cursor.lockState = CursorLockMode.None;

            AudioListener.pause = true;
            Time.timeScale = 0;
            return true;
        }
        else
        {
            this.gameIsPaused = false;
            Debug.Log("Game Resumed");
            Cursor.lockState = CursorLockMode.Locked;

            AudioListener.pause = false;
            Time.timeScale = 1;
            return false;
        }
    }
    public void MoneyManage()
    {
        this.money += 3000;
    }


    //Level Manager
    public IEnumerator LevelManager()
    {

        while (level!= 10)
        {
            if(levelCompleted)
            {
                yield return StartCoroutine(LevelComplete());
                level++;
                LevelIncrease(false);
                levelCompleted = false;
            }
            if(enemyKilled == enemyCount && !playerIsDead)
            {
                levelCompleted = true;
            }

            if(playerIsDead && !levelCompleted)
            {
                enemyList.Clear();
                restartCanvas.SetActive(true);
                CursorSetting(CursorLockMode.Confined, true);
            }

            yield return null;
        }
    }
    public IEnumerator LevelComplete()
    {
        txt_levelUp.text = "LEVEL UP!!";
        txt_levelUp.gameObject.SetActive(true);
        yield return StartCoroutine(WaitBeforeLevelChange(5));
        txt_levelUp.gameObject.SetActive(false);

        //Repositioning
        tempPlayer.transform.position = playerSpawnPoint.position;
        tempPlayer.GetComponent<PlayerHealth>().Health = 100;



        Debug.Log("Level Complete");
    }
    public void LevelIncrease(bool b)
    {
        if(b)
        {
            playerIsDead = false;
            tempPlayer = Instantiate(playerPrefab, playerSpawnPoint.transform.position, Quaternion.identity);
            playerWeaponSpawnPoint = tempPlayer.transform.Find("WeaponSpawnPoint").gameObject;
            shopCanvas.GetComponent<ShopCanvasScript>().player = tempPlayer;
            shopCanvas.GetComponent<ShopCanvasScript>().weaponSpawnPoint = playerWeaponSpawnPoint;
        }

        foreach (var item in healthPickupList)
        {
            Destroy(item);
        }

        enemyCount = level;
        enemyKilled = 0;
        enemyList = new List<GameObject>();
        Debug.Log("Level: " + level);


        for (int i = 0; i < level; i++)
        {
            float xPos = Random.Range(-48, -27);
            float zPos = Random.Range(0, 24);
            tempEnemy = Instantiate(enemyPrefab);
            tempEnemy.transform.SetPositionAndRotation(new Vector3(xPos, 1, zPos), Quaternion.identity);

            enemyList.Add(tempEnemy);
        }


        //Health Pickups
        for (int i = 0; i < healthPickupSpawnPoints.Length; i++)
        {
            GameObject tempHealthPick = Instantiate(healthPickup, healthPickupSpawnPoints[i].transform.position, Quaternion.identity);
            healthPickupList.Add(tempHealthPick);
        }
    }


    //Button Managers
    public void RestartGame()
    {
        Debug.Log("Restart game");
        restartCanvas.SetActive(false);

        foreach (var item in enemyList)
        {
            Destroy(item);
        }
        foreach (var item in healthPickupList)
        {
            Destroy(item);
        }

        Destroy(tempPlayer.gameObject);
        Destroy(tempWeapon);
        LevelIncrease(true);

    }    
    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
    public void OnResumeButtonClick()
    {
        optionCanvas.SetActive(false);
    }



    //Waiting Functions
    private IEnumerator WaitBeforeLevelChange(int timer)
    {
        yield return new WaitForSeconds(timer);
    }
    private IEnumerator CountDownTimer(int timer)
    {
        int count = timer;

        while (count >= 0)
        {
            timerTxt.text = $"Timer: {count}";
            yield return new WaitForSecondsRealtime(1);

            if(count==0)
            {
                PauseGame(false);
            }
            count--;
        }
    }
    public static void CursorSetting(CursorLockMode mode, bool visible)
    {
        Cursor.lockState = mode;
        Cursor.visible = visible;
    }
    public void CustomDestroy(GameObject obj, float timer)
    {
        if (obj.CompareTag("Enemy"))
        {
            if (enemyList.Contains(obj))
            {
                Debug.Log("Enemy Dies");
                enemyIsDead = true;
                MoneyManage();
                playerKills++;
                enemyKilled++;
                Destroy(obj, timer);
            }

        }
        else if(obj.CompareTag("Player"))
        {
            Debug.Log("Player Dies");
            playerIsDead = true;
            MoneyManage();
            Destroy(obj, timer);
            
        }
    }
}
