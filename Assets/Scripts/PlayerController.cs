using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;
    public Transform grounded;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    float accelerationTimeAirbourne = .2f;
    float accelerationTimeGrounded = .1f;
    float velXSmoothing;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
    {
        isGrounded = Physics2D.OverlapCircle(grounded.position, .25f, groundLayer);
        
        float targetVelX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        rb.velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x , targetVelX , ref velXSmoothing , (isGrounded) ? accelerationTimeGrounded : accelerationTimeAirbourne) , rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        }
	}
}
