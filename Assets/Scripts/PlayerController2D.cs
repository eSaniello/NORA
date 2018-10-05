using UnityEngine;
using System.Collections;
using Prime31;


public class PlayerController2D : MonoBehaviour
{
    // movement config
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float maxJumpHeight = 4f;
    public float minJumpHeight = 2f;
    public float timeToJumpApex = .4f;
    public float wallSlidingSpeed;
    public float wallStickTime = .25f;
    public Vector2 wallCrawl;
    public Vector2 wallJump;
    public float dashSpeed;


    [HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float gravity;
    private bool doubleJump = false;
    private bool isWallSliding = false;
    private float timeToWallUnstick;
    private bool facingRight = true;
    private int input;
    private bool isDashing;
    private bool dash;

    [HideInInspector]
    public bool isSwinging;


    void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}

    private void Start()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }


    #region Event Listeners

    void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
        input = (int)Input.GetAxisRaw("Horizontal");
        int wallDirX = (_controller.collisionState.left) ? -1 : 1;

		if( _controller.isGrounded )
			_velocity.y = 0;

		if( input > 0 ) 
		{
			normalizedHorizontalSpeed = 1;

            if (!facingRight)
                Flip();

			//if( _controller.isGrounded )
			//	_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( input < 0 )
		{
			normalizedHorizontalSpeed = -1;

            if (facingRight)
                Flip();

			//if( _controller.isGrounded )
			//	_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			//if( _controller.isGrounded )
			//	_animator.Play( Animator.StringToHash( "Idle" ) );
		}

        //wall sliding
        if (!_controller.isGrounded && (_controller.collisionState.left || _controller.collisionState.right) && _velocity.y < 0)
        {
            isWallSliding = true;
            _velocity.y *= wallSlidingSpeed;

            if(timeToWallUnstick > 0)
            {
                _velocity.x = 0;

                if(input != wallDirX && input != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
        else
        {
            isWallSliding = false;
        }

        Debug.Log("Is wall sliding: " + isWallSliding);
        Debug.Log("faceDir: " + _controller.collisionState.faceDir);
        Debug.Log("Collision state left: " + _controller.collisionState.left);
        Debug.Log("Collision state right: " + _controller.collisionState.right);
        Debug.Log("X Velocity" + _velocity.x);

        // we can only jump whilst grounded
        if (Input.GetKeyDown( KeyCode.Space ) )
		{
            if (_controller.isGrounded)
            {
                _velocity.y = maxJumpVelocity;
               //_animator.Play( Animator.StringToHash( "Jump" ) );
                doubleJump = true;
            }
            else
            {
                if (doubleJump && canDoubleJump && !(_controller.collisionState.left || _controller.collisionState.right))
                {
                    doubleJump = false;
                    _velocity.y = maxJumpVelocity;
                    //_animator.Play(Animator.StringToHash("Jump"));
                }
            }

            if (isWallSliding & canWallJump)
            {
                if (wallDirX == input)
                {
                    _velocity.x = -wallDirX * wallCrawl.x;
                    _velocity.y = wallCrawl.y;
                }
                else
                {
                    _velocity.x = -wallDirX * wallJump.x;
                    _velocity.y = wallJump.y;
                }
            }
		}

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_velocity.y > minJumpVelocity)
                _velocity.y = minJumpVelocity;
        }

        //dashing code
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            isDashing = true;

            if (dash)
            {
                float _dashSpeed;

                if (facingRight)
                    _dashSpeed = dashSpeed;
                else
                    _dashSpeed = -dashSpeed;

                if (_controller.isGrounded)
                {
                    _velocity.x += _dashSpeed;
                    dash = false;
                }
                else
                {
                    _velocity.x += (_dashSpeed / 2.5f);
                    dash = false;
                }
            }
        }

        if ((_controller.isGrounded || isWallSliding) && isDashing)
        {
            dash = true;
            isDashing = false;
        }
        

        //apply horizontal speed smoothing it.dont really do this with Lerp.Use SmoothDamp or something that provides more control
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        //if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
        //{
        //	_velocity.y *= 3f;
        //	_controller.ignoreOneWayPlatformsThisFrame = true;
        //}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
