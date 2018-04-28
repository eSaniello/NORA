using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public Transform grounded;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    Animator anim;
    private bool isGrounded = false;
    public float accelerationTimeAirbourne = .2f;
    public float accelerationTimeGrounded = .1f;
    float velXSmoothing = 0.0f;
    public GameObject bullet;
    public Transform gunTip;
    public float bulletSpeed;
    bool facingRight = true;
    public Transform shootingDirection;
    public Transform gun;
    Vector3 dir;
    public float fireRate = 0;
    float timeToFire = 0;
    public ObjectPool bulletPool;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //moving the player
        isGrounded = Physics2D.OverlapCircle(grounded.position, .25f, groundLayer);

        float targetVelX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, targetVelX, ref velXSmoothing, (isGrounded) ? accelerationTimeGrounded : accelerationTimeAirbourne), rb.velocity.y);

        //grounding the player
        if(Input.GetKey(KeyCode.C) && isGrounded)
        {
            rb.velocity = Vector2.zero;
        }

        //jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y > 0)
        {
            anim.SetBool("Jump", true);
        }
        else if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }

        if (!isGrounded)
        {
            anim.SetFloat("yVel", Mathf.Clamp(rb.velocity.y, -1.0f, 1.0f));
        }

        //shooting
        if (fireRate == 0)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.X) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    private void Update()
    {
        //gun pointing functionality
        if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow) && facingRight)
        {
            Debug.Log("right");
            gun.localRotation = Quaternion.Euler(0, 0, 45f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && !facingRight)
        {
            Debug.Log("left");
            gun.localRotation = Quaternion.Euler(0, 0, 45f);
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            gun.localRotation = Quaternion.Euler(0, 0, 90f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            gun.localRotation = Quaternion.Euler(0, 0, -90f);
        }
        else
        {
            gun.localRotation = Quaternion.Euler(0, 0, 0);
        }


        //flipping the character
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Shoot()
    {
        dir = shootingDirection.position - gun.transform.position;
        GameObject projectile = bulletPool.GetBullet();
        projectile.SetActive(true);
        projectile.transform.position = gunTip.position;
        projectile.transform.rotation = gunTip.rotation;
        projectile.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed * Time.deltaTime;
    }
}
