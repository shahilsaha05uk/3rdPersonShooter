using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RaycastWeapon : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public GameObject Muzzle;
    public Vector3 intensityVec;

    public Transform rayCastOrigin;
    public Vector3 rayCastDestination;
    public Camera cam;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletSpawnPoint;

    public int totalBullets;
    public int remainingBullets;
    public int totalRefills;
    

    List<AudioSource> fireSound = new List<AudioSource>();
    public float fireSoundRunTime;
    public bool startBulletSound;
    public int testIncrease = 0;


    private void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip.LoadAudioData();
        fireSound.Add(source);

        remainingBullets = totalBullets;
        rayCastDestination = Vector3.zero;
    }
    public void StartFiring()
    {
        muzzleFlash.Play();
        if (!fireSound[0].isPlaying)
        {
            fireSound[0].Play();
            remainingBullets--;
        }
        Muzzle.GetComponent<Shake>().Impulse(intensityVec);
    }

    public void MagazinesOnBuy(int magCount)
    {
        testIncrease += magCount;
    }

    public void StopFiring()
    {
        muzzleFlash.Stop();
    }

}
