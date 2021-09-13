using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class ParallaxEffect : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float Remoteness = 1;
    [SerializeField] bool OnlyX = true;

    Vector3 initPos;
    Vector3 initCamPos;
    Camera camera;

    void Start()
    {
        initPos = transform.position;
        camera = Camera.main;
        initCamPos = camera.transform.position;
    }

    void LateUpdate()
    {
        var delta = camera.transform.position - initCamPos;
        if (OnlyX)
            delta.y = delta.z = 0;
        transform.position = initPos + delta * Remoteness;
    }
}
