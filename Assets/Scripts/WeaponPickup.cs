using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponPickup : MonoBehaviour
{
    public Transform weaponToPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Transform weaponsToUnlockHolder = collision.transform.GetChild(collision.transform.childCount - 1);

            if (weaponsToUnlockHolder.Find(weaponToPickup.name))
            {
                Transform weaponToUnlock = weaponsToUnlockHolder.Find(weaponToPickup.name);
                weaponToUnlock.SetParent(collision.transform.Find("Melee Weapons Holder"));
                Destroy(gameObject);
            }

        }
    }
}
