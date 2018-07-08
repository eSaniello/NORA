using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject bullet;
    public int objectPoolCount;
    List<GameObject> objectPool;

    private void Awake()
    {
        objectPool = new List<GameObject>();

        for(int i = 0; i < objectPoolCount; i++)
        {
            GameObject projectile = Instantiate<GameObject>(bullet);
            projectile.gameObject.SetActive(false);
            objectPool.Add(projectile);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null;
    }
}
