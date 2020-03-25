using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator _anim;
    public bool hasDetonated = false;
    public bool hasSpawned;
   
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
       
        StartCoroutine(bombSpawnDel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Explode() {
       
        yield return new WaitForSeconds(2f);
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        hasDetonated = true; Destroy(gameObject);

        yield return new WaitUntil(() => hasSpawned == true);
        
    }
    public IEnumerator CollisionExplode() {
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        hasDetonated = true;
        yield return new WaitUntil(() =>hasSpawned == true);
        Destroy(gameObject);

    }

    public IEnumerator bombSpawnDel()
    {
        yield return new WaitForSeconds(0.05f);
        hasSpawned = false;
    }
}
