﻿using System.Collections;
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
        Debug.Log(_objects.owner.transform.name);
        Debug.Log("old: " + _objects.oldOwner.transform.name);
    }
}
