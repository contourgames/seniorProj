using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{

    /// <summary>
    ///    Reference for Jumping mechanics: https://www.youtube.com/watch?v=7KiK0Aqtmzc
    /// </summary>
    private Player player;
    private Rigidbody2D _rb;
    private playerCollision _collScript;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private bool _playerGrounded = false;

    public float maxVel;
    public float acceleration;
    public float joyStickX;
    public float joyStickY;

    public bool isWallSliding = false;
    public float wallSlidingSpeed;
    public float jumpVelocity = 5f;
    int whichSide;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collScript = GetComponent<playerCollision>();
        maxVel = 9;
        acceleration = 1.1f;
        

        //Assigns each player to a different controller by object name
        if (this.gameObject.name == "Player")
        {
            player = ReInput.players.GetPlayer(0);
        }
        else if (this.gameObject.name == "Player2") {
            player = ReInput.players.GetPlayer(1);

        }
        else if (this.gameObject.name == "Player3")
        {
            player = ReInput.players.GetPlayer(2);
        }
        else if (this.gameObject.name == "Player4")
        {
            player = ReInput.players.GetPlayer(3);

        }
    }

    // Update is called once per frame
  	// Update is called once per frame
	void FixedUpdate()
	{

		joyStickX = player.GetAxis("Horizontal");
		joyStickY = player.GetAxis("Vertical");
		float xSpeed = maxVel * joyStickX;

        _rb.velocity = new Vector2(xSpeed, _rb.velocity.y);

		//Jump
		if (player.GetButtonDown("A/X") && (_playerGrounded || isWallSliding)) {
            Jump();
		}

        CheckForWallSlide();
        //Set the wallsliding force
        if (isWallSliding)
        {
            if (_rb.velocity.y < -wallSlidingSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -wallSlidingSpeed);
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
    void CheckForWallSlide()
    {

        if (_collScript.onWall && !_playerGrounded)
        {
            isWallSliding = true;
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
    void Jump() {

        //High jump vs lowJump gravity multiplier
        if (!isWallSliding)
        {
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
            }
            else if (_rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

            }
        }

        if (!isWallSliding && _playerGrounded)
        {
            _rb.velocity = Vector2.up * jumpVelocity;
            _playerGrounded = false;
        }
        //wall jumping
        else if (isWallSliding)
        {
            if (whichSide == 1)
            {
                // _rb.velocity = Vector2.left * jumpVelocity;
                Vector2 wallJumpForce = new Vector2(jumpVelocity * -whichSide, jumpVelocity);
                _rb.AddForce(wallJumpForce);
                _playerGrounded = false;
            }
            else if (whichSide == -1)
            {

                _playerGrounded = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _playerGrounded = true;
        }


    }
}
