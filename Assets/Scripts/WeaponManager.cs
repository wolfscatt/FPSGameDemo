using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int minDamage, maxDamage;
    public Camera playerCamera;
    public float range = 300f;
    
    [Header("Particle Objects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private EnemyManager enemyManager;
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SingleShoot();
        }
    }
    private void SingleShoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            enemyManager = hit.transform.GetComponent<EnemyManager>();
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            if(enemyManager != null)
            {
                int damage = Random.Range(minDamage, maxDamage);
                enemyManager.EnemyTakeDamage(damage);
            }
            Destroy(impact, 2f);
        }
    }

}
