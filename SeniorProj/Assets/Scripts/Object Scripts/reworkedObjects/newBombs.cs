using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newBombs : MonoBehaviour
{
    public bool isActive;
    public int timer;
    Objects _Objects;
    public AudioClip deathClip;
    public Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        isActive = false;
        timer = 0;
        _Objects = GetComponent<Objects>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _animator.SetBool("active", isActive);
        _Objects.isActive = isActive;
        detectThrow();
        if (timer >= 60)
        {
            isActive = true;
            Explosion();
        }
        if (timer >= 120)
        {
            Object.Destroy(gameObject);
        }
    }

    void detectThrow()
    {
        if (_Objects.wasThrown)
        {
            timer++;
        }


    }

    void Explosion()
    {
        Collider2D hit2D;

        hit2D = Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 1.0f, 1 << LayerMask.NameToLayer("Player"));
        if (hit2D != null)
        {
            Debug.Log(hit2D.transform.name);
            GameObject hitPlayers = GameObject.Find("FakeObject");
            hitPlayers = GameObject.Find(hit2D.transform.name);
            if (hitPlayers.GetComponent<playerCollision>().gotHit == false)
            {
                hitPlayers.GetComponent<playerCollision>().audioSource.PlayOneShot(deathClip, 1.0f);
            }
            hitPlayers.GetComponent<playerCollision>().gotHit = true;
           
        }
    }
}
