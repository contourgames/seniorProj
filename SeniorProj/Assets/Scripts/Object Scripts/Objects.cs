using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    [Space]
    [Header("Bools")]
    public bool isActive;
    public bool held;
    public bool nearPlayer;
    public bool beingThrown;
    public bool wasThrown;
    public bool _grounded;
    bool _ignoreColl;
    [Space]
    [Header("Floats & Ints")]
    public float offset;
    public int throwTimer;
    [Space]
    public GameObject owner;
    public GameObject nearbyPlayer;
    public Collider2D myCollider;
    public Collider2D ownerCollider;
    Bomb _bombScript;
    Rigidbody2D _rb;
    Vector2 forceAdded;
    public AudioSource audioSource;
    public AudioClip pickUpClip;

   

    LayerMask _layerMask;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        held = false;
        nearPlayer = false;
        owner = GameObject.Find("FakeObject");
        nearbyPlayer = GameObject.Find("FakeObject");
        _rb = GetComponent<Rigidbody2D>();
        _bombScript = GetComponent<Bomb>();
        wasThrown = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
      //Debug.Log(owner.transform.name);
        DetectPlayer();
       // Debug.Log("X vel: " + _rb.velocity.x + " Y Vel " + _rb.velocity.y);
        if (_rb.velocity.x == 0 && _rb.velocity.y == 0 && _grounded)
        {
            _ignoreColl = true;
        }
        else {
            _ignoreColl = false;
        }
        if (held)
        {
            if (nearbyPlayer.layer == 9 && nearbyPlayer != null && nearbyPlayer.GetComponent<PlayerMovement>().facingRight)
            {
                offset = .5f;
            }
            else if(nearbyPlayer.layer == 9 && nearbyPlayer != null && nearbyPlayer.GetComponent<PlayerMovement>().facingRight == false) {
                offset = -.5f;
            }
            gameObject.transform.position = new Vector2(owner.transform.position.x + offset, owner.transform.position.y);
            Physics2D.IgnoreCollision(myCollider, ownerCollider, true);
            
        }
        else
        {
            gameObject.SetActive(true);
           
        }

        if (beingThrown)
        {
            throwTimer++;
            wasThrown = true;
            if (nearbyPlayer.layer == 9 && nearbyPlayer != null && nearbyPlayer.GetComponent<PlayerMovement>().facingRight)
            {
                 //forceAdded = new Vector2(owner.GetComponent<PlayerMovement>().throwX, owner.GetComponent<PlayerMovement>().throwY);
                 _rb.velocity = new Vector2(owner.GetComponent<PlayerMovement>().throwX, owner.GetComponent<PlayerMovement>().throwY);
                //_rb.AddForce(forceAdded);


            }
            else if (nearbyPlayer.layer == 9 && nearbyPlayer != null && nearbyPlayer.GetComponent<PlayerMovement>().facingRight == false)
            {
                //forceAdded = new Vector2(-owner.GetComponent<PlayerMovement>().throwX, owner.GetComponent<PlayerMovement>().throwY);

                _rb.velocity = new Vector2(-owner.GetComponent<PlayerMovement>().throwX, owner.GetComponent<PlayerMovement>().throwY);
                //_rb.AddForce(forceAdded);

            }
        }
       
        if (throwTimer > 1)
        {
            if (gameObject.tag == "knife")
            {
               
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), nearbyPlayer.GetComponent<BoxCollider2D>(), false);
            }
            else
            {
              //  Debug.Log("Nearby Player");
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), nearbyPlayer.GetComponent<BoxCollider2D>(), false);
            }
            throwTimer = 0;
            beingThrown = false;
            held = false;
            owner.GetComponent<PlayerMovement>().throwX = 5;
            owner.GetComponent<PlayerMovement>().throwY = 20;
           
        }

       
    }


    
    public void DetectPlayer()
    {
        Collider2D hit2D;

        hit2D = Physics2D.OverlapCircle(new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y), 1.0f, 1 << LayerMask.NameToLayer("Player"));
        if (hit2D!= null)
        {
           nearbyPlayer = GameObject.Find(hit2D.transform.name);
         
            nearPlayer = true;
            // Debug.Log(nearbyPlayer.transform.name);

            if (nearbyPlayer.GetComponent<PlayerMovement>().searching == true && nearbyPlayer.GetComponent<PlayerMovement>().nearObject == false && held == false)
            {
                nearbyPlayer.GetComponent<PlayerMovement>().heldObject = gameObject;
                nearbyPlayer.GetComponent<PlayerMovement>().nearObject = true;
                nearbyPlayer.GetComponent<PlayerMovement>().holding = true;
                held = true;
                owner = nearbyPlayer;
                ownerCollider = nearbyPlayer.GetComponent<Collider2D>();
                audioSource.PlayOneShot(pickUpClip, 1.0f);
            }
        } else 
        {

            nearbyPlayer = GameObject.Find("FakeObject");
            nearPlayer = false;
        }
      
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _grounded = true;

        }
        else
        {
            _grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

      
  
        if (!isActive && _grounded == true && collision.gameObject.layer == 9)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            
        }

        if (collision.gameObject.tag == "Orb" && collision.gameObject.GetComponent<theOrb>().pulsing)
        {
            Debug.Log("deflected");
            GetComponent<Rigidbody2D>().velocity = new Vector2(15 * (transform.position.x - collision.gameObject.transform.position.x), 15 * (transform.position.y - collision.gameObject.transform.position.y));
            // GetComponent<Rigidbody2D>().velocity = new Vector2(20,20);
        }
    }

    public IEnumerator isActiveTimer() {
        //Debug.Log("A");
        yield return new WaitForSeconds(.1f);
        _bombScript.GetComponent<Animator>().SetBool("Active", true);
        //yield return new WaitForSeconds(1.5f);
        isActive = true;

    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        
       
    }
}
