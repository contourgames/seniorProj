using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    public gameManagerJuggernaut _juggernautGM;
    [Space]
    [Header("Dashing")]
    //variables for dashing
    public bool facingRight;
    public int dashVelocity;
    public bool dashing;
    public int dashTimer;
    public bool canDash;
    [Space]
    [Header("Object Holding")]
    //variables for holding
    public bool nearObject;
    public bool searching;
    public int searchTime;
    public bool holding;
    public GameObject heldObject;
    public float throwX;
    public float throwY;

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

    private Vector2 spawnPosition;
    private Vector2 forcePerFrame = Vector2.zero;

    [Space]
    [Header("Bools")]
    [SerializeField] private bool _playerGrounded;
    public bool isWallSliding = false;
    public bool canWallJump;
    public bool canMove;
    public bool canSlide;
    public bool canJump;

    [Space]
    [Header("GameModes")]
    public float score;
    public bool enableHurt;

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
        score = 0;
        #region Starting values

        #region dashing vals
        //dashing variables set do default values
        facingRight = true;
        dashing = false;
        dashTimer = 0;
        canDash = true;
        canJump = true;

        #endregion

        #region throwing vals
        //holding object variables set to default values
        holding = false;
        heldObject = GameObject.Find("FakeObject");
        throwX = 5.0f;
        throwY = 7.0f;
        searching = false;
        searchTime = 0;
        nearObject = false;
        enableHurt = true;
        #endregion

        _rb = GetComponent<Rigidbody2D>();
        _collScript = GetComponent<playerCollision>();
        maxVel = 9;
        acceleration = 1.1f;
        canWallJump = true;

        transform.GetComponent<TrailRenderer>().enabled = false;
        #endregion

        #region Reinput assignment
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
        #endregion

        spawnPosition = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {

       // Debug.Log(holding);
        Dash();
        Throw();

        if (heldObject.tag == "Orb") { //Increase player score as long as they are holding orb

            _juggernautGM.IncreasePlayerScore(gameObject.GetComponent<PlayerMovement>());
            int temp = Mathf.RoundToInt(score);
            Debug.Log(gameObject.transform.name + " Score: " + temp);
        }

        if (_collScript.onWall && !_playerGrounded && player.GetButtonUp("A/X"))
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
        #region Player jumping anf player grounded
        //Jump
        if (player.GetButton("A/X") && canJump)
        {
            if (_playerGrounded == true)
            {
                Jump(Vector2.up);
            }
        }

        if (player.GetButtonUp("A/X") && !canJump || _playerGrounded && !player.GetButton("A/X"))
        {
            canJump = true;
        }


        if (_collScript.onGround && _rb.velocity.y == 0)
        {
            _playerGrounded = true;
        }
        else
        {
            _playerGrounded = false;
        }
        #endregion

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

        #region wallsliding & Gravity multiplier
        //High jump vs lowJump gravity multiplier
        if (!isWallSliding)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1.5f) * Time.deltaTime;
                canJump = false;
            }
            else
            if (_rb.velocity.y > 0 && !player.GetButton("A/X"))
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                canJump = false;

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
        #endregion

        _rb.AddForce(forcePerFrame);
        forcePerFrame = Vector2.zero;

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
        if (_collScript.onWall && !_playerGrounded && player.GetButton("A/X"))
        {
          //  Debug.Log("Prev: " + prevYVel + " Curr: " + currVel);

            if (_rb.velocity.y <= 0) //If player is starting to fall they can wall slide
            {
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

        if (_collScript.onLeftWall) // Determines which side player is on
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

        forcePerFrame = Vector2.up * jumpVelocity;

    }

    void WallJump()
    {
        Vector2 wallDirection;

        if (joyStickX == 0) //If there's no input, player detach from the wall
        {
           // Debug.Log("Wall Jump: No Input");
            wallDirection = _collScript.onRightWall ? Vector2.left : Vector2.right; //Determines which direction player will jump off the wall
            forcePerFrame += wallDirection * wallJumpVelocity;
        }
        else {

            wallDirection = _collScript.onRightWall ? Vector2.left : Vector2.right; //Determines which direction player will jump off the wall

            forcePerFrame +=  Vector2.up * wallJumpVelocity; // Add force upwards

            if (joyStickX == wallDirection.x) //If joystick is against the wall jump farther
            {
                forcePerFrame +=  wallDirection * wallJumpVelocity; //Add force to the side
                
               // Debug.Log("Joystick against the wall");
            }
            else if (joyStickX != wallDirection.x) //If joystick is with the wall jump shorter
            {
                forcePerFrame += wallDirection * (wallJumpVelocity * .5f); //Add force to the side
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
   //  Debug.Log(heldObject.transform.name);
        if (searching)
        {
            searchTime++;
        }
        if (searchTime >= 5)
        {
            searching = false;

        }
        if (holding == false)
        {
            
                if (player.GetButtonUp("X/Square"))
                {
                    searching = true;
                       
                }
            
        } else if (holding == true && heldObject.transform.name != "Orb")
        {
            
            if (player.GetButton("X/Square"))   //longer the player holds button, shot will transition from lob to fastball
            {
                            //maximum throw power
                if (throwX <= 20.0f) { throwX += 0.4f; }
                            //minimum throw height
                if (throwY >= 1.0f) { throwY -= 0.2f; }
                
                
            }
            if (player.GetButtonUp("X/Square"))
            {
             //  Debug.Log("Y: " + throwY);
             //  Debug.Log("X: " + throwX);
                holding = false;
                nearObject = false;
                heldObject.GetComponent<Objects>().beingThrown = true;
              

            }
        } else
        {
           
            if (player.GetButtonUp("X/Square"))
            {
                heldObject.GetComponent<theOrb>().pulsing = true;


            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Death") //Reset player position
        {
            transform.position = spawnPosition;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_collScript.onGround && collision.gameObject.tag == "Ground") //Tell if player's grounded
        {
            _playerGrounded = true;
            canDash = true;
            canMove = true;
           // Debug.Log("Player grounded");
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
