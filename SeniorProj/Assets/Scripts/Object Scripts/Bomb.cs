using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator _anim;
    public bool hasDetonated = false;
    public ObjectSpawner _objSpScript;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _objSpScript = GameObject.Find("GameManager").GetComponent<ObjectSpawner>();
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
        hasDetonated = true;
        yield return new WaitUntil(() => _objSpScript.hasSpawned == true);
        Destroy(gameObject);
    }
    public IEnumerator CollisionExplode() {
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        hasDetonated = true;
        yield return new WaitUntil(() =>_objSpScript.hasSpawned == true);
        Destroy(gameObject);

    }

    public IEnumerator bombSpawnDel()
    {
        yield return new WaitForSeconds(0.05f);
        _objSpScript.hasSpawned = false;
    }
}
