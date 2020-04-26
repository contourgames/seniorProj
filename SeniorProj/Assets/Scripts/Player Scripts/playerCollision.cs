using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollision : MonoBehaviour
{
    /// <summary>
    /// Reference for code: https://www.youtube.com/watch?v=STyY26a_dPY
    /// </summary>
    // Start is called before the first frame update
    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Space]
    public Vector2 spawnPosition;


    public bool isJuggernaut = false;
    public bool gotHit;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onTopGround;
    public bool onGroundBelow;
    public int wallSlide;

    public AudioSource audioSource;
    public AudioClip deathClip;

    public float radius = 0.25f;
    public Vector2 bottomOffSet, rightOffSet, LeftOffSet, TopOffSet, GBOffset;
    private Color debugColor = Color.red;

    PlayerMovement _playerScript;
    gameManagerJuggernaut _GM;
    //respawn variables
    public Vector2 startPos;
    public int spawnTimer;
    //public Animator _animator;

    Animator _moveAnim;
    public Camera _mainCam;

    public bool isSliding = false;
    public bool isFlipped = false;
    void Start()
    {
        spawnPosition = new Vector2(transform.position.x, transform.position.y);
        _GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameManagerJuggernaut>();
        gotHit = false;
        _playerScript = GetComponent<PlayerMovement>();
        //_animator = GetComponent<Animator>();
        startPos = transform.position;

        _moveAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Respawn animation
        //_animator.SetBool("dead", gotHit);
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffSet, radius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffSet, radius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + LeftOffSet, radius, groundLayer);


        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffSet, radius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + LeftOffSet, radius, groundLayer);
        onTopGround = Physics2D.OverlapCircle((Vector2)transform.position + TopOffSet, radius, groundLayer);
        //onGroundBelow = Physics2D.OverlapCircle((Vector2)transform.position + GBOffset, radius, groundLayer);
        wallSlide = onRightWall ? -1 : 1;

        if (gotHit == true)
        {
            _GM.StartCoroutine(_GM.Respawn(gameObject, startPos));
            _GM.DecreasePlayerScore(gameObject);
            if (_playerScript.holding )
            { //if they're currently holding the orb
                if (_playerScript.heldObject.tag == "Orb")
                {
                    if (GetComponent<PlayerMovement>().holding == true && GetComponent<PlayerMovement>().heldObject.tag == "Orb")
                    {
                        GetComponent<PlayerMovement>().heldObject.GetComponent<theOrb>().ownerDied = true;
                    }

                    _playerScript.heldObject = null;
                    _playerScript.holding = false;
                    _playerScript.nearObject = false;
                } else
                {
                    GameObject tempObject = GetComponent<PlayerMovement>().heldObject;
                    GetComponent<PlayerMovement>().heldObject = GameObject.Find("FakeObject");
                    Object.Destroy(tempObject);
                    _playerScript.nearObject = false;
                    GetComponent<PlayerMovement>().holding = false;
                    
                }
            }
        }

    }

    public void Update()
    {
     
      
        if (onGround)
        {
            _moveAnim.SetTrigger("Grounded");
        }

        if (isSliding)
        {
            _moveAnim.SetBool("Wallsliding", true);

            if (onRightWall)
            {
                //transform.localScale = new Vector2(-0.05f, transform.localScale.y);

            } else if (onLeftWall)
            {
                transform.localScale = new Vector2(0.05f, transform.localScale.y);
            }

            //if (isFlipped == false)
            //{
            //    if(transform.localScale.x == 0.05f)
            //    {
            //    }
            //    else
            //    {

            //    }
            //    //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            //    isFlipped = true;
            //}
            
        } else
        {
            //isFlipped = false;
            _moveAnim.SetBool("Wallsliding", false);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        var positions = new Vector2[] { bottomOffSet, rightOffSet, LeftOffSet, TopOffSet };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffSet, radius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffSet, radius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftOffSet, radius);
        Gizmos.DrawWireSphere((Vector2)transform.position + TopOffSet, radius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + GBOffset, radius);


    }

    public void OnCollisionStay2D(Collision2D collision)
    {
       
       
        if (collision.gameObject.layer == 13) { //player collides with throwable obj

           // Debug.Log("Colliding");
           
            if (collision.gameObject.GetComponent<Objects>().isActive == true) {
              //  Debug.Log("Kill");
                audioSource.PlayOneShot(deathClip, 1.0f);
                gotHit = true;
                _mainCam.GetComponent<cameraShake>().StartCameraShake();

            }
        }

    }

    public void OnCollisionExit2D(Collision2D collision)
    {

    }


}
