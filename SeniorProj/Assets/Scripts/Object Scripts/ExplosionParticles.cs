using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{

    public Vector2 animPos;
    public GameObject ParticlesPref;
    public AudioSource ExplSound;
    public bool soundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ActivatePrt()
    {
        GameObject Explosion = Instantiate(ParticlesPref);
        Explosion.transform.position = animPos;

        ExplSound = Explosion.GetComponent<AudioSource>();

        soundPlayed = false;
            if(soundPlayed == false)
        {
            ExplSound.Play();
            soundPlayed = true;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(Explosion);
        //StartCoroutine(DestroyParticles(Explosion));
    }

    //public IEnumerator DestroyParticles(GameObject Explosion)
    //{
    //    Debug.Log("DestroyExpl");
    //    yield return new WaitForSeconds(0f);
    //}

}
