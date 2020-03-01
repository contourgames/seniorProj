using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{

    //variables for dashing
    public bool facingRight;
    public int dashVelocity;
    public bool dashing;
    public int dashTimer;
    public bool canDash;
    //variables for holding
    public bool nearObject;
    public bool holding;
    public GameObject heldObject;
    
    /// <summary>
    ///    Reference for Jumping mechanics: https://www.youtube.com/watch?v=7KiK0Aqtmzc
    /// </summary>
    private Player player;
    private Rigidbody2D _rb;
    private playerCollision _collScript;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    private float joyStickX;
    private float joyStickY;
    int whichSide;
    float xSpeed;


    [Space]
    [Header("Bools")]
    [SerializeField] private bool _playerGrounded = false;
    public bool isWallSliding = false;
    public bool canWallJump;
    public bool canMove;
    public bool canSlide;
    [Space]
    [Header("Floats")]
    public float maxVel;
    public float acceleration;
    public float airDrag = 0.95f;
    public float wallSlidingSpeed;
    public float wallJumpVelocity;
    public float jumpVelocity = 15f;
    float prevYVel = 0;
    float currVel = 0;
    [Space]
    [Header("Ints")]
    [SerializeField]
    private int numWallJumps = 1;



    // Start is called before the first frame update
    void Start()
    {
        //dashing variables set do default values
        facingRight = true;
        dashing = false;
        dashTimer = 0;
        canDash = true;

        //holding object variables set to default values
        holding = false;
        heldObject = GameObject.Find("FakeObject");
        
        _rb = GetComponent<Rigidbody2D>();
        _collScript = GetComponent<playerCollision>();
        maxVel = 9;
        acceleration = 1.1f;
        canWallJump = true;

        transform.GetComponent<TrailRenderer>().enabled = false;

        //Assigns each player to a different controller by object name
        if (this.gameObject.name == "Player")
        {
            player = ReInput.players.GetPlayer(0);
        }
        else if (this.gameObject.name == "Player 2")
        {
            player = ReInput.players.GetPlayer(1);

        }
        else if (this.gameObject.name == "Player 3")
        {
            player = ReInput.players.GetPlayer(2);
        }
        else if (this.gameObject.name == "Player 4")
        {
            player = ReInput.players.GetPlayer(3);

        }
    }

    void Update()
    {
        
        Dash();
        Throw();
        if (_collScript.onWall && !_playerGrounded && player.GetButtonDown("A/X"))
        {
            WallJump();

        }

        if(dashing == true)
        {
            transform.GetComponent<TrailRenderer>().enabled = true;
        } else
        {
            transform.GetComponent<TrailRenderer>().enabled = false;

        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Setting player input
        joyStickX = player.GetAxis("Horizontal");
        joyStickY = player.GetAxis("Vertical");
        Vector2 walkDir = new Vector2(joyStickX, joyStickY);
        if (!isWallSliding && canMove)
        {

            Move(walkDir);
        }
        prevYVel = currVel;
        currVel = _rb.velocity.y;

        #region Dashing
        //Dash
        //detects player's direction
        if (walkDir.x > 0)
        {
            facingRight = true;
        }
        if (walkDir.x < 0)
        {
            facingRight = false;
        }
        //sets dash to be left or right based on player direction
        if (facingRight)
        {
            //speed of dash
            dashVelocity = 15;
        }
        else
        {
            dashVelocity = -15;
        }

        //pressing the dash button activates this booleon, which causes a timer to go up
        if (dashing && canDash)
        {
            dashTimer++;
            //this time is the duration of the dash
            if (dashTimer <= 10)
            {
                _rb.velocity = new Vector2(dashVelocity, 0);
            }
            else
            {
                _rb.velocity = new Vector2(_rb.velocity.x * .7f, 0);
            }
            //this time is how long the cooldown is between dashes
            if (dashTimer >= 20)
            {
                dashTimer = 0;
                dashing = false;
                canDash = false;
            }
        }
        #endregion

     //Jump
        if (player.GetButton("A/X"))
        {
            if (_playerGrounded == true)
            {
                Jump(Vector2.up);
            }


        }



        //High jump vs lowJump gravity multiplier
        if (!isWallSliding)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1.5f) * Time.deltaTime;
            }
            else
            if (_rb.velocity.y > 0 && !player.GetButton("A/X"))
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

            }
        }
        CheckForWallSlide();

        //Set the wallsliding force
        if (isWallSliding)
        {
            
            if (joyStickX == 0 && _rb.velocity.y <= 0)
            {
                if (_rb.velocity.y < -wallSlidingSpeed)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed);
                }
            }
            else if (joyStickX != 0 && _rb.velocity.y <= 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed * .095f);
            }

        }

        #region Drew Old WASD Controls
        //moving left and right
        //	if (Input.GetKey(KeyCode.D))
        //	{
        //		if (Mathf.Abs(RB.velocity.x) <= 5)
        //		{
        //			RB.velocity = new Vector2(maxVel, RB.velocity.y);
        //		}
        //	}



        //	if (Input.GetKey(KeyCode.A))
        //	{	
        //			RB.velocity = new Vector2(maxVel * -1, RB.velocity.y);

        //	}
        //	//stopping
        //	if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && RB.velocity.x != 0)
        //	{
        //		RB.velocity = new Vector2(RB.velocity.x - RB.velocity.x / 10, RB.velocity.y);
        //	}


        //	//Detecting Ground
        //	RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y - 1), new Vector2(1, 0), 1.5f, 1 << LayerMask.NameToLayer("Ground"));
        //	if (hit.collider != null)
        //	{
        //		Debug.Log("Detecting Ground");
        //		//Jumping
        //		if (Input.GetKey(KeyCode.W))
        //		{

        //			RB.velocity = new Vector2(RB.velocity.x, 25);
        //		}
        //		else
        //		{
        //			RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y);
        //		}
        //	}


        //}
        #endregion

    }
    private void Move(Vector2 walkDir)
    {
        _rb.velocity = new Vector2(walkDir.x * maxVel, _rb.velocity.y);

        //Slows horizontal movement when in the air
        if (!_playerGrounded && !isWallSliding && joyStickX != 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x * airDrag, _rb.velocity.y);
        }

        //Stopping
        if (joyStickX == 0 && _rb.velocity.x != 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x - _rb.velocity.x / 10, _rb.velocity.y);
        }

    }
    void CheckForWallSlide()
    {
        if (_collScript.onWall && !_playerGrounded)
        {
            Debug.Log("Prev: " + prevYVel + " Curr: " + currVel);

            if (_rb.velocity.y <= 0)
            {
                Debug.Log("slide");
                isWallSliding = true;

            }
            else {
                isWallSliding = false;
            }
        }
        else
        {
            
            isWallSliding = false;


        }

        if (_collScript.onLeftWall)
        {
            whichSide = -1;
        }
        else if (_collScript.onRightWall)
        {
            whichSide = 1;
        }
    }
    //Dynamic Jumping
    void Jump(Vector2 dir)
    {

        _rb.AddForce(Vector3.up * jumpVelocity);

    }

    void WallJump()
    {
        Vector2 wallDirection;

        if (joyStickX == 0)
        {
           // Debug.Log("Wall Jump: No Input");
            wallDirection = _collScript.onRightWall ? Vector2.left : Vector2.right; //Determines which direction player will jump off the wall
            _rb.AddForce(wallDirection * wallJumpVelocity);
        }
        else {

            wallDirection = _collScript.onRightWall ? Vector2.left : Vector2.right; //Determines which direction player will jump off the wall

            _rb.AddForce(Vector3.up * wallJumpVelocity); // Add force upwards

            if (joyStickX == wallDirection.x)
            {
                _rb.AddForce(wallDirection * wallJumpVelocity); //Add force to the side
                
               // Debug.Log("Joystick against the wall");
            }
            else if (joyStickX != wallDirection.x) 
            {
                _rb.AddForce(wallDirection * (wallJumpVelocity * .5f)); //Add force to the side
             //   Debug.Log("Joystick with the wall");
            }

            StartCoroutine("WSMoveDelay");
            canMove = false;
        }

    }

    void Dash()
    {
        if (player.GetButtonDown("LT/L2") || player.GetButtonDown("RT/R2"))
        {
           //Debug.Log("Presse");
            dashing = true;
        }
    }

    void Throw() {
        if (nearObject) {
            if (player.GetButtonDown("X/Square"))
            {
              //  Debug.Log(holding);
                if (!holding)
                {
                    holding = true;
                  
                }
                else
                {
                    
                    holding = false;
                    heldObject.GetComponent<Objects>().beingThrown = true;

                }

            }
        } else
        {
            holding = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_collScript.onGround && collision.gameObject.tag == "Ground")
        {
            _playerGrounded = true;
            canDash = true;
            canMove = true;
           // Debug.Log("Player grounded");
        }
        else
        {
            _playerGrounded = false;

        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _playerGrounded = false;
        }
    }
    IEnumerator canMoveDelay() {
        canMove = false;
        yield return new WaitForSeconds(.5f);
        canMove = true;
    }

    IEnumerator WSMoveDelay()
    {
        canWallJump = false;
        yield return new WaitForSeconds(.4f);
        canWallJump = true;
    }

}
