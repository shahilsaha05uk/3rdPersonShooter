using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 m_speed;

    private Transform target;
    float h;
    float v;

    [Space(20)][Header("New Camera")]
    public GameObject player;
    public GameObject cameraCenter;
    public float m_sensitivity;
    public Camera cam;
    public float collisionSensitivity;
    public Vector3 offset;

    private RaycastHit camHit;
    private Vector3 camDist;

    void Update()
    {
        #region TestCamera
         v += Input.GetAxis("Mouse Y");
         h += Input.GetAxis("Mouse X");



        cameraCenter.transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, player.transform.position.z+ offset.z);

        Quaternion rotation = Quaternion.Euler(cameraCenter.transform.rotation.x - v * m_sensitivity / 2,
                                               cameraCenter.transform.rotation.y + h * m_sensitivity,
                                               cameraCenter.transform.rotation.z);
        cameraCenter.transform.rotation = rotation;

        player.transform.rotation = Quaternion.Euler(cameraCenter.transform.eulerAngles);

        #endregion

    }

}
