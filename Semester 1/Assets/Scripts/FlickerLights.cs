using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLights : MonoBehaviour
{
    float timeRange;
    public float min, max;
    public Light lightobj;
    private void Start()
    {
        lightobj = GetComponent<Light>();
        timeRange = Random.Range(min, max);
    }


    void Update()
    {
        timeRange = Random.Range(min, max);

        if(timeRange>0)
        {
            timeRange-= Time.deltaTime;
        }

        if(timeRange<=0)
        {
            lightobj.enabled = !lightobj.enabled; 
            timeRange = Random.Range(min, max);
        }
    }
}
