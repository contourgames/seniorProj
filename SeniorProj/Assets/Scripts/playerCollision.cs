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
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onTopGround;
    public bool onGroundBelow;
    public int wallSlide;


    public float radius = 0.25f;
    public Vector2 bottomOffSet, rightOffSet, LeftOffSet, TopOffSet, GBOffset;
    private Color debugColor = Color.red;
    void Start()
    {

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
        onGroundBelow = Physics2D.OverlapCircle((Vector2)transform.position + GBOffset, radius, groundLayer);
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
        Gizmos.DrawWireSphere((Vector2)transform.position + GBOffset, radius);


    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player 2" || collision.gameObject.tag == "Player 3" || collision.gameObject.tag == "Player 4") {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }
}
