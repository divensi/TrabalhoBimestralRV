using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    private float damage = 10.0f;
    private float range = 100.0f;
    private float fireRate = 15.0f;
    private float nextTimeToFire = 0.0f;
    private GameObject impactEffect;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffectMeet;
    public GameObject impactEffectWood;
    public GameObject impactEffectStel;
    public GameObject impactEffectStone;
    public GameObject zombie;
    private void Start()
    {
        impactEffect = impactEffectStone;
    }

    void Update () {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1.0f / fireRate;
            Shoot();
        }
	}
    private void Shoot()
    {
        muzzleFlash.Play();
        AudioSource som = GetComponent<AudioSource>();
        som.Play();
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            ImpactType(hit);
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            GameObject ImpactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactGO, 2.0f);
        }
    }
    private void ImpactType(RaycastHit hit)
    {
        switch (hit.transform.tag)
        {
            case "Zombie":
                {
                    impactEffect = impactEffectMeet;
                     
                };break;
            case "Wood":
                {
                    impactEffect = impactEffectWood;
                };break;
            case "Steel":
                {
                    impactEffect = impactEffectStel;
                };break;
            case "Stone":
                {
                    impactEffect = impactEffectStone;
                };break;
            default: break;
        }
    }
}
