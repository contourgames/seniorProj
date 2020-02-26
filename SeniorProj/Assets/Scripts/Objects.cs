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
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        held = false;
        nearPlayer = false;
        nearbyPlayer = GameObject.Find("FakeObject");
      
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(held);
        DetectPlayer();

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
            if (nearbyPlayer.GetComponent<PlayerMovement>().facingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(10, 5);
            } else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 5);
            }
        }
       
        if (throwTimer > 10)
        {
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), nearbyPlayer.GetComponent<BoxCollider2D>(), false);
            throwTimer = 0;
            beingThrown = false;
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
            nearbyPlayer.GetComponent<PlayerMovement>().nearObject = true;
            nearPlayer = true;
           // Debug.Log(nearbyPlayer.transform.name);

            if (nearbyPlayer.GetComponent<PlayerMovement>().holding == true)
            {
                held = true;
                owner = nearbyPlayer;
                ownerCollider = nearbyPlayer.GetComponent<Collider2D>();
                nearbyPlayer.GetComponent<PlayerMovement>().heldObject = gameObject;
            } else
            {
                nearbyPlayer.GetComponent<PlayerMovement>().heldObject = GameObject.Find("FakeObject");
                held = false;
            }
        } else
        {
            nearPlayer = false;
        }
      
        
    }
}
