using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestScriptableObjects", menuName = "TestScriptableObjects", order = 2)]
public class TestSC : ScriptableObject
{
    public int health;
    public GameObject obj;

}
