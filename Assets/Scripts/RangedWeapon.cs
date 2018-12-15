using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public Transform firepoint;
    public Bullet bulletPrefab;
    public float fireRate;

    private float timeBtwFire;

    private void Update()
    {
        if(timeBtwFire <= 0)
        {
            if (Input.GetKey(KeyCode.C))
            {
                Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);

                timeBtwFire = fireRate;
            }
        }
        else
        {
            timeBtwFire -= Time.deltaTime;
        }
    }

}
