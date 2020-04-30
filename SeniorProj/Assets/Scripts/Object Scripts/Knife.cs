using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public bool isActive;
    public int timer;
    Objects _Objects;
    public Rigidbody2D _RB;
    public Quaternion startRotation;
    BoxCollider2D _bColl;
    
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        _RB = GetComponent<Rigidbody2D>();
        isActive = true;
        timer = 0;
        _Objects = GetComponent<Objects>();
        _bColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Objects.isActive = isActive;

        
        if (_Objects.held == false && Mathf.Abs(_RB.velocity.x) <= 0.5 )
        {
            //Debug.Log("inactive");
            isActive = false;
                _Objects.wasThrown = false;
           
        }
        else if (_Objects.wasThrown)
        {
            // Debug.Log(_Objects.isActive);
            isActive = true;
        } else if (_Objects.held)
        {
            isActive = true;
        }
        if (_Objects.held == true)
        {
          
            if (_Objects.nearbyPlayer != null && _Objects.nearbyPlayer.GetComponent<PlayerMovement>().facingRight)
            {
              GetComponent<SpriteRenderer>().flipX = true;
                transform.rotation = Quaternion.Euler(0, 0, 300);
            } else
            {
                GetComponent<SpriteRenderer>().flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 60);
            }
          
        } else if (_Objects._grounded == true && isActive == false && _RB.velocity.x <= 1.0f && _RB.velocity.y <= 1.0f)
        {

            transform.rotation = startRotation;
        }

     
    }
}
