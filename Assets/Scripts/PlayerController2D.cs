using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class PlayerController2D : MonoBehaviour
{
    // movement config
    public bool canDoubleJump;
    public bool canWallJump;
    public bool canDash;
    public float runSpeed = 12f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float maxJumpHeight = 4f;
    public float minJumpHeight = 2f;
    public float timeToJumpApex = .45f;
    public float wallSlidingSpeed = .75f;
    public float wallStickTime = .25f;
    public Vector2 wallCrawl = new Vector2(12, 15);
    public Vector2 wallJump = new Vector2(18, 17);
    public float dashSpeed = 75;
    public float startTimeBtwMeleeAttack;
    public Transform meleeAttackRangePos;
    public float meleeAttackRangeX;
    public float meleeAttackRangeY;
    public LayerMask enemieMask;
    public WeaponSwitcher weaponSwitcher;
    public MeleeWeapon basicSword;
    public MeleeWeapon powerSword;
    public MeleeWeapon ultraPowerSword;

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
    private float timeBtwMeleeAttack;
    


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
		}
		else if( input < 0 )
		{
			normalizedHorizontalSpeed = -1;

            if (facingRight)
                Flip();
		}
		else
		{
			normalizedHorizontalSpeed = 0;
        }

        //playing the animations
        if (_controller.isGrounded == true)
            _animator.SetInteger("Run", input);


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
        

        // we can only jump whilst grounded
        if (Input.GetKeyDown( KeyCode.Space ) )
		{
            if (_controller.isGrounded)
            {
                _velocity.y = maxJumpVelocity;
                doubleJump = true;
                // _animator.SetBool("Jump", true);
            }
            else
            {
                if (doubleJump && canDoubleJump && !(_controller.collisionState.left || _controller.collisionState.right))
                {
                    doubleJump = false;
                    _velocity.y = maxJumpVelocity;
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

        if (!_controller.isGrounded && _velocity.y > 0)
            _animator.SetBool("Jump", true);

        if (!_controller.isGrounded && !isWallSliding && _velocity.y < 0)
            _animator.SetBool("Jump", false);

        _animator.SetBool("Grounded", _controller.isGrounded);

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

        //Melee code
        if (weaponSwitcher.selectedWeapon == 0)
            startTimeBtwMeleeAttack = basicSword.attackRate;
        else if (weaponSwitcher.selectedWeapon == 1)
            startTimeBtwMeleeAttack = powerSword.attackRate;
        else if (weaponSwitcher.selectedWeapon == 2)
            startTimeBtwMeleeAttack = ultraPowerSword.attackRate;

        if (timeBtwMeleeAttack <= 0)
        {
            if (Input.GetKey (KeyCode.X))
            {
                if (weaponSwitcher.selectedWeapon == 0)
                    _animator.SetTrigger("Basic_sword_attack");
                else if (weaponSwitcher.selectedWeapon == 1)
                    _animator.SetTrigger("Power_sword_attack");
                else if (weaponSwitcher.selectedWeapon == 2)
                    _animator.SetTrigger("Ultra_power_sword_attack");

                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(meleeAttackRangePos.position, new Vector2(meleeAttackRangeX, meleeAttackRangeY), 0, enemieMask);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //Deal damage to enemy
                    if (weaponSwitcher.selectedWeapon == 0)
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(basicSword.damage);
                    else if (weaponSwitcher.selectedWeapon == 1)
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(powerSword.damage);
                    else if (weaponSwitcher.selectedWeapon == 2)
                        enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(ultraPowerSword.damage);
                }


                timeBtwMeleeAttack = startTimeBtwMeleeAttack;
            }
        }
        else
        {
            timeBtwMeleeAttack -= Time.deltaTime;
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

    //editor scripting
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(meleeAttackRangePos.position, new Vector3(meleeAttackRangeX, meleeAttackRangeY, 1));
    }
}
