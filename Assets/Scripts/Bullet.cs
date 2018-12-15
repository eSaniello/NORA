using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeTime = 10;
    public float detectionRayDistance;

    private Rigidbody2D rb;
    private PlayerController2D controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed * controller.transform.localScale.x * Time.deltaTime;
    }

    private void Update()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
