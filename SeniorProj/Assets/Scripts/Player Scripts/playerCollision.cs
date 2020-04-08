﻿using System.Collections;
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
    gameManagerJuggernaut _juggernautGM;
    void Start()
    {
        spawnPosition = new Vector2(transform.position.x, transform.position.y);

        gotHit = false;
        _playerScript = GetComponent<PlayerMovement>();

        _juggernautGM = GameObject.Find("GameManager").GetComponent<gameManagerJuggernaut>();

    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffSet, radius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffSet, radius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + LeftOffSet, radius, groundLayer);


        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffSet, radius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + LeftOffSet, radius, groundLayer);
        onTopGround = Physics2D.OverlapCircle((Vector2)transform.position + TopOffSet, radius, groundLayer);
        //onGroundBelow = Physics2D.OverlapCircle((Vector2)transform.position + GBOffset, radius, groundLayer);
        wallSlide = onRightWall ? -1 : 1;

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player 2" || collision.gameObject.tag == "Player 3" || collision.gameObject.tag == "Player 4") { //Ignore player collisions
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        if (collision.gameObject.tag == "Orb") //Add player knockback when 
        {
            if (collision.gameObject.GetComponent<theOrb>().pulsing) {
                _playerScript.AddKnockBack(collision.gameObject);
                Debug.Log("A");
            }

        }
        if (collision.gameObject.layer == 13) { //player collides with throwable obj

         
            if (_playerScript.holding && _playerScript.heldObject.tag == "Orb" && collision.gameObject.GetComponent<Objects>().isActive) { //if they're currently holding the orb
                Debug.Log("juggernautDied");
                if (GetComponent<PlayerMovement>().holding = true && GetComponent<PlayerMovement>().heldObject.tag == "Orb")
                {
                    GetComponent<PlayerMovement>().heldObject.GetComponent<theOrb>().ownerDied = true;
                }
                gotHit = true;

                _juggernautGM.RespawnOrb();
                _juggernautGM.DecreasePlayerScore(gameObject);
                _playerScript.heldObject = null;
                _playerScript.holding = false;
                _playerScript.nearObject = false;
            }
            if (collision.gameObject.GetComponent<Objects>().isActive == true && _playerScript.enableHurt) {
                Debug.Log("Kill");
                audioSource.PlayOneShot(deathClip, 1.0f);
                gotHit = true;
            }
        }
    }

}
