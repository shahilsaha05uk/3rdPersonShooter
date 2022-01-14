using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Cinemachine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    public Text ScreenView_text;
    public GameObject crosshair;
    public GameObject cube;
    public Camera cam;
    Vector3 screenToViewPortPoint;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //screenToViewPortPoint = cam.ScreenToViewportPoint(crosshair.transform.position);

        ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //cube.transform.position = hit.point;
        }


        ScreenView_text.text = "Ray Origin: " + ray.origin + "Ray direction: "+ ray.direction;

    }

}
