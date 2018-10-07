using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingDirection;
    public float firerate;
    public float bulletSpeed;

    private void Start()
    {
        InvokeRepeating("Shoot", firerate, firerate);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 dir = shootingDirection.position - transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
    }
}
