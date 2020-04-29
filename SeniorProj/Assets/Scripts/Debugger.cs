using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    Objects _objects;
    // Start is called before the first frame update
    void Start()
    {
        _objects= GetComponent<Objects>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(new Vector2(_objects.owner.transform.position.x + _objects.offsetx, _objects.owner.transform.position.y + 0.5f));
    }
}
