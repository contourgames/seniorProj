using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator _anim;
    public bool hasDetonated = false;
    public bool hasSpawned;
    private Objects _objScript;
    public ExplosionParticles _ParticleMgr;
    private SpriteRenderer _spr;
    CircleCollider2D _cColl;
    public bool hasBeenPickedUp;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
       
        StartCoroutine(bombSpawnDel());

        _ParticleMgr = GameObject.Find("GameManager").GetComponent<ExplosionParticles>();

        _objScript = GetComponent<Objects>();

        _spr = GetComponent<SpriteRenderer>();
        _spr.enabled = true;
        _cColl = GetComponent <CircleCollider2D> ();

        hasBeenPickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_objScript.wasThrown) {
            StartCoroutine(Explode());
            _objScript.wasThrown = false;        
        }

        if (_objScript.held)
        {
            hasBeenPickedUp = true;
            _cColl.enabled = false;
        }
        else {
            _cColl.enabled = true;
        }
        if (!hasBeenPickedUp)
        {
            _cColl.enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        } else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (_objScript.isActive && collision.gameObject.layer == 9)
        { //If bomb explodes close enough to another bomb
            StartCoroutine("Explode");
        }
    }
    public IEnumerator Explode() {
        _anim.SetBool("Active", true);

        yield return new WaitForSeconds(1.9f);
        _objScript.isActive = true;
        yield return new WaitForSeconds(.1f);
        

        hasDetonated = true; 

        yield return new WaitUntil(() => hasSpawned == true);
        //when bomb spawns, destroy bomb;
        _anim.Play("Explosion");
        _spr.enabled = false;
        _ParticleMgr.animPos = new Vector2(transform.position.x, transform.position.y);
        StartCoroutine(_ParticleMgr.ActivatePrt());
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    public IEnumerator CollisionExplode() {
        _anim.SetBool("Active", true);

        yield return new WaitForSeconds(.1f);
        hasDetonated = true;
        yield return new WaitUntil(() => hasSpawned == true);
        _spr.enabled = false;

        _ParticleMgr.animPos = new Vector2(transform.position.x, transform.position.y);
        StartCoroutine(_ParticleMgr.ActivatePrt());
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }

    public IEnumerator bombSpawnDel()
    {
        yield return new WaitForSeconds(0.1f);
        hasSpawned = false;
    }

    public void Detonate()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
