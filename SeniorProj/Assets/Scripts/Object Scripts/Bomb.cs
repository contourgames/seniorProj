using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator _anim;
    public bool hasDetonated = false;
    public bool hasSpawned;

    public ExplosionParticles _ParticleMgr;
   
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
       
        StartCoroutine(bombSpawnDel());

        _ParticleMgr = GameObject.Find("GameManager").GetComponent<ExplosionParticles>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Explode() {
       
        yield return new WaitForSeconds(2f);
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        hasDetonated = true; 

        yield return new WaitUntil(() => hasSpawned == true);
        //when bomb spawns, destroy bomb;
        _ParticleMgr.animPos = new Vector2(transform.position.x, transform.position.y);
        StartCoroutine(_ParticleMgr.ActivatePrt());
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public IEnumerator CollisionExplode() {
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        hasDetonated = true;
        yield return new WaitUntil(() => hasSpawned == true);
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
