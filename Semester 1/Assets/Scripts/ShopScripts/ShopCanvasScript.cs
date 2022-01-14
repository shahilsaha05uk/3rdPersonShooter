using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCanvasScript : MonoBehaviour
{
    [SerializeField] private Transform m_ScrollContent;
    [SerializeField] private ShopItemButtonScript buttonPrefab;

    [SerializeField] private Scriptable_Objects_Script[] scriptableObjects;
    private ShopItemButtonScript[] shopItemsButtons;

    public Image imageCanvas;

    private Graphic _graphics;
    public Color color;
    [SerializeField] private GameManager manager;
    public GameObject player;
    public GameObject weaponSpawnPoint;
    static public bool itemBought;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _graphics = imageCanvas.GetComponent<Graphic>();
    }

    private void Update()
    {
        ImageColorChanger();
    }
    private void OnEnable()
    {

        shopItemsButtons = new ShopItemButtonScript[scriptableObjects.Length];
        GameManager.canvasOpened = true;



        for (int i = 0; i < shopItemsButtons.Length; i++)
        {
            shopItemsButtons[i] = Instantiate(buttonPrefab, m_ScrollContent);
            shopItemsButtons[i].Ref_ImageCanvas(imageCanvas);
            shopItemsButtons[i].Init(scriptableObjects[i]);

            if(shopItemsButtons[i].itemType == ItemType.ARMOUR)
            {
                shopItemsButtons[i].items += ArmourBuy;
            }
            else if(shopItemsButtons[i].itemType == ItemType.AMMO)
            {
                shopItemsButtons[i].items += AmmoBuy;
            }
            else
            {
                shopItemsButtons[i].items += ShopButtonPressed;
            }
        }

    }
    private void OnDisable()
    {
        for (int i = shopItemsButtons.Length - 1; i >= 0; i--)
        {
            shopItemsButtons[i].items -= ShopButtonPressed;
            shopItemsButtons[i].items -= ArmourBuy;
            shopItemsButtons[i].items -= AmmoBuy;
            Destroy(shopItemsButtons[i].gameObject);
        }
    }
    public Transform[] playerChildObj;
    public GameObject gunObj = null;



    public void ArmourBuy(Scriptable_Objects_Script obj)
    {
        itemBought = true;

        if (obj.m_Type == ItemType.ARMOUR && obj.m_Cost< manager.money)
        {
            PlayerHealth pHealth = player.GetComponent<PlayerHealth>();
            pHealth.armourBought = true;

            manager.money -= obj.m_Cost;
            manager.PauseGame(false);
            this.gameObject.SetActive(false);
            GameManager.canvasOpened = false;
        }
        else
        {
            manager.messageToDisplay = "This item is not available at this moment check back later";
            manager.timerForMessageDisplay = 3f;
        }

    }
    public void AmmoBuy(Scriptable_Objects_Script obj)
    {
        itemBought = true;

        if (obj.m_Type == ItemType.AMMO)
        {
            playerChildObj = player.GetComponentsInChildren<Transform>();
            gunObj= null;

            foreach (var item in playerChildObj)
            {
                if (item.GetComponent<getCategoryType>() != null)
                {
                    ItemType type = item.GetComponent<getCategoryType>().returnCategory();

                    if (type == ItemType.GUN && manager.money > obj.m_Cost)
                    {
                        gunObj = item.gameObject;
                        break;
                    }
                    else if (type == ItemType.GUN && manager.money < obj.m_Cost)
                    {
                        manager.messageToDisplay = "You do not have enough sufficient funds to buy this item!! kill more enemies to collect more money";
                        manager.timerForMessageDisplay = 3f;

                        manager.PauseGame(false);
                        this.gameObject.SetActive(false);
                        GameManager.canvasOpened = false;

                    }
                }
            }


            if (player.GetComponent<PlayerScript>().raycastWeapon.totalRefills < 5)
            {

                foreach (var sc_Obj in scriptableObjects)
                {
                    if (gunObj == null)
                    {
                        manager.messageToDisplay = "You need to buy a gun first";
                        manager.timerForMessageDisplay = 3f;
                        manager.PauseGame(false);
                        this.gameObject.SetActive(false);
                        GameManager.canvasOpened = false;
                        break;
                    }
                    else if (sc_Obj.returnItemType() == gunObj.GetComponent<getCategoryType>().returnCategory())
                    {
                        player.GetComponent<PlayerScript>().raycastWeapon.totalRefills++;
                        manager.money -= obj.m_Cost;
                        manager.PauseGame(false);
                        this.gameObject.SetActive(false);
                        GameManager.canvasOpened = false;
                    }
                }
            }
            else
            {
                Debug.Log("This item is not available at this moment check back later");
                manager.messageToDisplay = "This item is not available at this moment check back later";
                manager.timerForMessageDisplay = 3f;

                manager.PauseGame(false);
                this.gameObject.SetActive(false);
                GameManager.canvasOpened = false;

            }
        }
    }
    private void ShopButtonPressed(Scriptable_Objects_Script obj)
    {   
        if(obj.item== null)
        {
            manager.PauseGame(false);
            Debug.Log("This item is not available at this moment check back later");
            manager.messageToDisplay = "This item is not available at this moment check back later";
            manager.timerForMessageDisplay = 3f;

            this.gameObject.SetActive(false);
            GameManager.canvasOpened = false;
        }


        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        if (obj.m_Cost > manager.money)
        {
            Debug.Log("You do not have enough sufficient funds to buy this item!! kill more enemies to collect more money");
            manager.messageToDisplay = "You do not have enough sufficient funds to buy this item!! kill more enemies to collect more money";
            manager.timerForMessageDisplay = 3f;

            manager.PauseGame(false);
            this.gameObject.SetActive(false);
            GameManager.canvasOpened = false;
        }
        else
        {
            if(player.GetComponent<PlayerScript>().raycastWeapon!= null)
            {
                Destroy(player.GetComponent<PlayerScript>().raycastWeapon.gameObject);
            }

            itemBought = true;

            manager.tempWeapon = Instantiate(obj.item, manager.playerWeaponSpawnPoint.transform.position, manager.playerWeaponSpawnPoint.transform.rotation);
            manager.tempWeapon.transform.SetParent(player.transform);
            playerScript.weapon = manager.tempWeapon;
            playerScript.raycastWeapon = manager.tempWeapon.GetComponent<RaycastWeapon>();
            SetHandPositions(manager.tempWeapon);

            manager.money -= obj.m_Cost;
            manager.PauseGame(false);
            this.gameObject.SetActive(false);
            GameManager.canvasOpened = false;
        }
    }


    private void ImageColorChanger()
    {
        if(imageCanvas.sprite!= null)
        {
            color = new Color(1, 1, 1, 1);
        }
        else
        {
            color = new Color(1, 1, 1, 0);
        }
        _graphics.color = color;
    }
    private void SetHandPositions(GameObject itemObject)
    {
        Transform[] item = itemObject.GetComponentsInChildren<Transform>();
        PlayerIKScript testIKScript = player.GetComponent<PlayerIKScript>();
        
        testIKScript.rightHandObj = null;
        testIKScript.leftHandObj = null;

        foreach (Transform t in item)
        {
            if (t.gameObject.CompareTag("LeftHandPos"))
            {
                Debug.Log($"ChildObjects name: " + t.gameObject.name);

                testIKScript.leftHandObj = t;
            }
            else if (t.gameObject.CompareTag("RightHandPos"))
            {
                Debug.Log($"ChildObjects name: " + t.gameObject.name);

                testIKScript.rightHandObj = t;
            }
        }

    }

}
