using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    //Reference: https://medium.com/@mattThousand/basic-2d-screen-shake-in-unity-9c27b56b516
    private Transform _transform;
    private float shakeDuration = 0f;

    private float shakeMagnitude = 0.7f;
    private float dampingSpeed = 1.0f;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Awake()
    {
        if (transform == null) {
            _transform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    void OnEnable()
    {
        startingPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shakeDuration > 0) {
            transform.localPosition = startingPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else {
            shakeDuration = 0f;
            transform.localPosition = startingPos;
        }
    }

    public void StartCameraShake() {
        shakeDuration = .5f;
    }
}
