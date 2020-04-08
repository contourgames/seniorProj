using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theOrb : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public int player1Score;
    public int player2Score;
    public int player3Score;
    public int player4Score;

    //being held variables
    public bool held;
    public bool nearPlayer;
    public GameObject owner;
    public GameObject nearbyPlayer;
    public Collider2D myCollider;
    public Collider2D ownerCollider;

    public bool _grounded;
    Rigidbody2D _rb;
    LayerMask _layerMask;

    bool _ignoreColl;

    //pulse variables
    public bool pulsing;
    public int pulseTimer;
    public GameObject pulseAttack;
    public GameObject currPulse;

    //respawn variables
    public Vector2 startPos;
    public bool ownerDied;
    public int ownerDeathTimer;

    public AudioSource audioSource;
    public AudioClip pickUpClip;
    // Start is called before the first frame update
    void Start()
    {
        ownerDied = false;
        ownerDeathTimer = 0;
        startPos = transform.position;
        player1Score = 0;
        player2Score = 0;
        player3Score = 0;
        player4Score = 0;

        player1 = GameObject.Find("Player");

        
        myCollider = GetComponent<Collider2D>();
        held = false;
        nearPlayer = false;
        owner = GameObject.Find("FakeObject");
        nearbyPlayer = GameObject.Find("FakeObject");
        _rb = GetComponent<Rigidbody2D>();
        currPulse = GameObject.Find("FakeObject");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("pulsing: " + pulsing);
        DetectPlayer();
        Pulse();
       
        if (_rb.velocity.x == 0 && _rb.velocity.y == 0 && _grounded)
        {
            _ignoreColl = true;
        }
        else
        {
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
        //checks if player died
        
       if (ownerDied)
        {
            //Debug.Log(ownerDeathTimer);
            ownerDeathTimer++;
        }
        if (ownerDeathTimer == 1)
        {
      
           // Debug.Log(owner.transform.name);
            held = false;
           
            owner.GetComponent<PlayerMovement>().holding = false;
            owner.GetComponent<PlayerMovement>().heldObject = GameObject.Find("FakeObject");
            transform.position = new Vector3(0, 0, 0);
            owner.GetComponent<PlayerMovement>().nearObject = false;
            owner = GameObject.Find("FakeObject");
            myCollider.enabled = true;
        }
        if (ownerDeathTimer >= 2)
        {
            ownerDied = false;
        }
        if (!ownerDied)
        {
            ownerDeathTimer = 0;
        }
    }

    public void Pulse()
    {
        if (pulsing)
        {
            pulseTimer++;
        }
        if (pulsing && pulseTimer <= 30)
        {
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<Animator>().SetBool("Play", true);
            //    if (pulseTimer == 1)//creates pulse prefab
            //{
            //    GameObject newPulse = Instantiate(pulseAttack);
            //    newPulse.transform.position = gameObject.transform.position;
            //    currPulse = newPulse; //assigning it to currPulse allows you to manipualte the newPulse outside of this one frame
            //}
            //currPulse.transform.position = gameObject.transform.position;   
        }
        if (pulseTimer == 30)
        {
            GetComponent<CircleCollider2D>().enabled = false;

            GetComponent<Animator>().SetBool("Play", false);

            //Destroy(currPulse);
            //currPulse = GameObject.Find("FakeObject");
        }
        if (pulseTimer >= 100)
        {
            pulseTimer = 0;
            pulsing = false;
        }
    }

   
    public void DetectPlayer()
    {
        RaycastHit2D hit2D;

        //Debug.Log("nearPlayer: " + nearPlayer);
        hit2D = Physics2D.BoxCast(gameObject.transform.position, new Vector2(1, 1), 50, new Vector2(1, 0), 1.0f, 1 << LayerMask.NameToLayer("Player"));
        if (hit2D.collider != null)
        {
            nearbyPlayer = GameObject.Find(hit2D.transform.name);

            nearPlayer = true;
            // Debug.Log(nearbyPlayer.transform.name);

            if (nearbyPlayer.GetComponent<PlayerMovement>().searching == true && nearbyPlayer.GetComponent<PlayerMovement>().nearObject == false && held == false)
            
            {
                audioSource.PlayOneShot(pickUpClip, 1.0f);
                ownerDied = false;
                nearbyPlayer.GetComponent<PlayerMovement>().heldObject = gameObject;
                nearbyPlayer.GetComponent<PlayerMovement>().nearObject = true;
                nearbyPlayer.GetComponent<PlayerMovement>().holding = true;
                held = true;
                owner = nearbyPlayer;
                ownerCollider = nearbyPlayer.GetComponent<Collider2D>();
                //myCollider.enabled = false;

            }
        }
        else
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


        if (_grounded == true && !held && collision.gameObject.layer == 9)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());

        }
    }
}
