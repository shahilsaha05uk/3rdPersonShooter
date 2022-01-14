using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shake : MonoBehaviour
{
    CinemachineImpulseSource source;

    private void Start()
    {
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void Impulse(Vector3 intensityVec)
    {
        source.GenerateImpulse(intensityVec);
    }

}
