using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidesAndStairs : MonoBehaviour
{
    public float stepHeight = 0.3f;
    public float stepSmooth = 0.01f;
    public GameObject upperStepRay;
    public GameObject lowerStepRay;


    private Rigidbody rb;

    private void Start()
    {
        upperStepRay.transform.position = new Vector3(upperStepRay.transform.position.x, stepHeight, upperStepRay.transform.position.z);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        StepClimb();
    }

    //This function checks if the steps are colliding with the upper ray and if it does than we cannot step on it and if we dont
    //then we increase the position
    public void StepClimb()
    {
        //1. If the steps collide with the lower ray but NOT the upper ray
        RaycastHit hitLower;
        Ray lowRay = new Ray(lowerStepRay.transform.position, transform.TransformDirection(Vector3.forward));
        if(Physics.Raycast(lowRay, out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            Ray upperRay = new Ray(upperStepRay.transform.position, transform.TransformDirection(Vector3.forward));
            if(!Physics.Raycast(upperRay, out hitUpper, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
            else
            {
                Debug.Log("Long steps");
            }
        }

        // Now to check within a 45 degree +-
        RaycastHit hitLower45;
        RaycastHit hitUpper45;
        if (Physics.Raycast(lowerStepRay.transform.position, transform.TransformDirection(1.5f,0,1f), out hitLower45, 0.1f))
        {
            if (!Physics.Raycast(upperStepRay.transform.position, transform.TransformDirection(1.5f, 0, 1f), out hitUpper45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
            else
            {
                Debug.Log("Long steps");
            }
        }

        RaycastHit hitLowerMinus45;
        RaycastHit hitUpperMinus45;
        if (Physics.Raycast(lowerStepRay.transform.position, transform.TransformDirection(-1.5f, 0, 1f), out hitLowerMinus45, 0.1f))
        {
            if (!Physics.Raycast(upperStepRay.transform.position, transform.TransformDirection(-1.5f, 0, 1f), out hitUpperMinus45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
            else
            {
                Debug.Log("Long steps");
            }
        }

    }

}

