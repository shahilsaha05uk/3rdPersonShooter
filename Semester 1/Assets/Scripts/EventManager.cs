using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttonOBJ;
    [SerializeField] private GameObject inGameUI;
/*    private EquipWeapons equipWeapons;

    private void Start()
    {
        equipWeapons = inGameUI.GetComponent<EquipWeapons>();
        foreach (var obj in buttonOBJ)
        {
            AddEventByScript(obj);
        }
    }

    private void AddEventByScript(GameObject obj)
    {
        if (obj.GetComponent<EventTrigger>() == null)
        {
            obj.AddComponent<EventTrigger>();
        }

        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter;

        entry.callback.AddListener((func1) => { equipWeapons.ImageUpdater(obj); });
        trigger.triggers.Add(entry);
    }
*/}
