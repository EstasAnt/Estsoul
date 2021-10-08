using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] float speed = 1;
    [SerializeField] float moveRadius = 2;

    Vector3 initPos;
    Vector3 target;
    Vector3 aim;

    void Start()
    {
        initPos = transform.position;
        GoToNextTarget();
    }

    private void GoToNextTarget()
    {
        target =  initPos + (Vector3)UnityEngine.Random.insideUnitCircle * moveRadius;
        aim = target;
    }

    void Update()
    {
        aim = Vector3.Lerp(aim, target, Time.deltaTime * speed);
        transform.position = Vector3.MoveTowards(transform.position, aim, Time.deltaTime * speed);
        if (Vector3.SqrMagnitude(transform.position - target) < 0.01f)
            GoToNextTarget();

        transform.localScale = new Vector3(aim.x < transform.position.x?-1:1, transform.localScale.y, transform.localScale.z);
    }
}
