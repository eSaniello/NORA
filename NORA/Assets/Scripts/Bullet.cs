using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask groundLayer;
    public float destroyAfterSeconds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) //8 = ground layer
            gameObject.SetActive(false);
    }


    private void Update()
    {
        StartCoroutine(DestroyAfterTime(destroyAfterSeconds));
    }


    IEnumerator DestroyAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }
}
