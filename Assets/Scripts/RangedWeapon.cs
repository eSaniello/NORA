using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public Transform firepoint;
    public Bullet bulletPrefab;
    public float fireRate;
    public float damage;

    private float timeBtwFire;

    private void Update()
    {
        if(timeBtwFire <= 0)
        {
            if (Input.GetKey(KeyCode.C))
            {
                Bullet bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.identity);
                bullet.damage = damage;

                timeBtwFire = fireRate;
            }
        }
        else
        {
            timeBtwFire -= Time.deltaTime;
        }
    }

}
