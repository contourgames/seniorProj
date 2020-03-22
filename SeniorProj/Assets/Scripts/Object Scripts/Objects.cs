using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public bool held;
    public bool nearPlayer;
    public GameObject owner;
    public bool beingThrown;
    public int throwTimer;
    public GameObject nearbyPlayer;
    public Collider2D myCollider;
    public Collider2D ownerCollider;
    public bool _grounded;
    bool _ignoreColl;
    Rigidbody2D _rb;
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
    }

    // Update is called once per frame
    void Update()
    {
      
      //Debug.Log(held);
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
           
            gameObject.transform.position = owner.transform.position;
            Physics2D.IgnoreCollision(myCollider, ownerCollider, true);
            
        }
        else
        {
            gameObject.SetActive(true);
           
        }

        if (beingThrown)
        {
            throwTimer++;
            if (nearbyPlayer != null && nearbyPlayer.GetComponent<PlayerMovement>().facingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(owner.GetComponent<PlayerMovement>().throwX, owner.GetComponent<PlayerMovement>().throwY);
            } else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-owner.GetComponent<PlayerMovement>().throwX, owner.GetComponent<PlayerMovement>().throwY);
            }
        }
       
        if (throwTimer > 1)
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), nearbyPlayer.GetComponent<BoxCollider2D>(), false);
            throwTimer = 0;
            beingThrown = false;
            held = false;
            owner.GetComponent<PlayerMovement>().throwX = 5;
            owner.GetComponent<PlayerMovement>().throwY = 20;
           
        }
    }

    
    public void DetectPlayer()
    {
        RaycastHit2D hit2D;
      
        //Debug.Log("nearPlayer: " + nearPlayer);
        hit2D = Physics2D.BoxCast(gameObject.transform.position, new Vector2 (1,1), 50, new Vector2(1,0), 1.0f, 1 << LayerMask.NameToLayer("Player"));
        if (hit2D.collider != null)
        {
           nearbyPlayer = GameObject.Find(hit2D.transform.name);
         
            nearPlayer = true;
            // Debug.Log(nearbyPlayer.transform.name);

            if (nearbyPlayer.GetComponent<PlayerMovement>().searching == true && nearbyPlayer.GetComponent<PlayerMovement>().nearObject == false)
            {
                nearbyPlayer.GetComponent<PlayerMovement>().heldObject = gameObject;
                nearbyPlayer.GetComponent<PlayerMovement>().nearObject = true;
                nearbyPlayer.GetComponent<PlayerMovement>().holding = true;
                    held = true;
                owner = nearbyPlayer;
                ownerCollider = nearbyPlayer.GetComponent<Collider2D>();
                
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
      
  
        if ( _grounded == true && collision.gameObject.layer == 9)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
            
        }
    }
}
