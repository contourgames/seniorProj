using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Explode() {
        
        yield return new WaitForSeconds(2f);
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }
    public IEnumerator CollisionExplode() {
        _anim.Play("Explosion");
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);

    }
}
