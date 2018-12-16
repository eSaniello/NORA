using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponPickup : MonoBehaviour
{
    public bool basicSword = false;
    public bool powerSword = false;
    public bool ultraPowerSword = false;
    public bool asaultRifle = false;
    public bool smg = false;

    private Transform weaponToPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Transform weaponsToUnlockHolder = collision.transform.Find("Weapons To Unlock");

            if (basicSword)
            {
                weaponToPickup = weaponsToUnlockHolder.Find("Basic Sword");

                PickUpWeapon(weaponsToUnlockHolder, weaponToPickup, collision);
            }
            else if (powerSword)
            {
                weaponToPickup = weaponsToUnlockHolder.Find("Power Sword");

                PickUpWeapon(weaponsToUnlockHolder, weaponToPickup, collision);
            }
            else if (ultraPowerSword)
            {
                weaponToPickup = weaponsToUnlockHolder.Find("Ultra Power Sword");

                PickUpWeapon(weaponsToUnlockHolder, weaponToPickup, collision);
            }
            else if (asaultRifle)
            {
                weaponToPickup = weaponsToUnlockHolder.Find("Asault Rifle");

                PickUpWeapon(weaponsToUnlockHolder, weaponToPickup, collision);
            }
            else if (smg)
            {
                weaponToPickup = weaponsToUnlockHolder.Find("SMG");

                PickUpWeapon(weaponsToUnlockHolder, weaponToPickup, collision);
            }
        }
    }

    private void PickUpWeapon(Transform weaponsToUnlockHolder, Transform weaponToPickup, Collider2D collision)
    {
        if (weaponsToUnlockHolder.Find(weaponToPickup.name))
        {
            Transform weaponToUnlock = weaponsToUnlockHolder.Find(weaponToPickup.name);
            weaponToUnlock.SetParent(collision.transform.Find("Melee Weapons Holder"));
            if (collision.transform.Find("Melee Weapons Holder").childCount == 1)
                weaponToUnlock.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
