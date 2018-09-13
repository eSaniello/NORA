using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_V2 : MonoBehaviour
{
    private new Renderer renderer;
    Rigidbody2D rb;

    public float maxSpeed = 10;
    public float acceleration = 35;
    public float jumpSpeed = 8;
    public float jumpDuration;

    public bool enableDoubleJump = true;
    public bool wallHitDoubbleJumpOverride = true;


    bool canDoublejump = true;
    float jumpduration;
    bool jumpKeyDown = false;
    bool canVariableJump = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if(horizontal < -0.1f)
        {
            if(rb.velocity.x > -maxSpeed)
            {
                rb.AddForce(new Vector2(-acceleration, 0.0f));
            }
            else
            {
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            }
        }
        else if(horizontal > 0.1f)
        {
            if (rb.velocity.x < maxSpeed)
            {
                rb.AddForce(new Vector2(acceleration, 0.0f));
            }
            else
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }
        }

        bool onTheGround = isOnGround();

        float vertical = Input.GetAxis("Vertical");

        if (onTheGround)
        {
            canDoublejump = true;
        }

        if(vertical > 0.1f)
        {
            if (!jumpKeyDown) //1st frame
            {
                jumpKeyDown = true;

                if(onTheGround || (canDoublejump && enableDoubleJump) || wallHitDoubbleJumpOverride)
                {
                    bool wallHit = false;
                    int wallHitDir = 0;

                    bool leftWallHit = isOnWallLeft();
                    bool rightWallHit = isOnWallRight();

                    if(horizontal != 0)
                    {
                        if (leftWallHit)
                        {
                            wallHit = true;
                            wallHitDir = 1;
                        }
                        else if (rightWallHit)
                        {
                            wallHit = true;
                            wallHitDir = -1;
                        }
                    }

                    if (!wallHit)
                    {
                        if(onTheGround || (canDoublejump && enableDoubleJump))
                        {
                            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

                            jumpduration = 0.0f;
                            canVariableJump = true;
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(jumpSpeed * wallHitDir, jumpSpeed);

                        jumpduration = 0.0f;
                        canVariableJump = true;
                    }

                    if(!onTheGround && !wallHit)
                    {
                        canDoublejump = false;
                    }
                }
            }
            else if(canVariableJump) //2nd frame
            {
                jumpduration += Time.deltaTime;

                if(jumpduration < jumpDuration / 1000)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
            }
        }
        else
        {
            jumpKeyDown = false;
            canVariableJump = false;
        }
    }


    private bool isOnGround()
    {
        float lenghtToSearch = 0.1f;
        float colliderThreshold = 0.001f;

        Vector2 lineStart = new Vector2(transform.position.x, transform.position.y - renderer.bounds.extents.y - colliderThreshold);

        Vector2 vectorToSearch = new Vector2(transform.position.x, lineStart.y - lenghtToSearch);

        RaycastHit2D hit = Physics2D.Linecast(lineStart, vectorToSearch);

        return hit;
    }

    private bool isOnWallLeft()
    {
        bool retVal = false;

        float lenghtToSearch = 0.1f;
        float colliderThreshold = 0.01f;

        Vector2 lineStart = new Vector2(transform.position.x - renderer.bounds.extents.x - colliderThreshold, transform.position.y);

        Vector2 vectorToSearch = new Vector2(transform.position.x - lenghtToSearch, transform.position.y);

        RaycastHit2D hitLeft = Physics2D.Linecast(lineStart, vectorToSearch);

        retVal = hitLeft;

        if (retVal)
        {
            if (hitLeft.collider.GetComponent<NoSlideJump>())
            {
                retVal = false;
            }
        }

        return retVal;
    }

    private bool isOnWallRight()
    {
        bool retVal = false;

        float lenghtToSearch = 0.1f;
        float colliderThreshold = 0.01f;

        Vector2 lineStart = new Vector2(transform.position.x + renderer.bounds.extents.x + colliderThreshold, transform.position.y);

        Vector2 vectorToSearch = new Vector2(transform.position.x + lenghtToSearch, transform.position.y);

        RaycastHit2D hitRight = Physics2D.Linecast(lineStart, vectorToSearch);

        retVal = hitRight;

        if (retVal)
        {
            if (hitRight.collider.GetComponent<NoSlideJump>())
            {
                retVal = false;
            }
        }

        return retVal;
    }
}
