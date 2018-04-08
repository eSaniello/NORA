using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public Transform grounded;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    Animator anim;
    private bool isGrounded = false;
    float accelerationTimeAirbourne = .2f;
    float accelerationTimeGrounded = .1f;
    float velXSmoothing;
    public GameObject bullet;
    public Transform gunTip;
    public float bulletSpeed;


    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	void FixedUpdate ()
    {
        isGrounded = Physics2D.OverlapCircle(grounded.position, .25f, groundLayer);
        
        float targetVelX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x , targetVelX , ref velXSmoothing , (isGrounded) ? accelerationTimeGrounded : accelerationTimeAirbourne) , rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.C) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.C) && rb.velocity.y > 0)
        {
            anim.SetBool("Jump", true);
        }
        else if (isGrounded)
        {
            anim.SetBool("Jump", false);
        }

        if (!isGrounded)
        {
            anim.SetFloat("yVel", Mathf.Clamp(rb.velocity.y , -1.0f , 1.0f));
        }
	}

    private void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            GameObject projectile = Instantiate(bullet, gunTip.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * Time.deltaTime, 0);
        }
    }
}
