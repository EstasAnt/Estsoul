using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTesting : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float speed = 10, acceleration=1, reachDistance=2;
    Rigidbody2D rigid = new Rigidbody2D();
    public Vector2 disturbingForce;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.None;
    }

    private void FixedUpdate()
    {
        /*Vector2 
            distance = Vector2.zero,
            force = Vector2.zero;
        if (target != null)
        {
            distance = target.transform.position - transform.position;
            Vector2 projection = Vector3.Project(rigid.velocity, distance);
            force = distance + projection;
            force *= Mathf.Clamp(Mathf.Sqrt(distance.magnitude * Mathf.Clamp(acceleration,0,distance.magnitude*speed/Time.fixedDeltaTime)*speed*10) / rigid.mass - projection.magnitude, 0, speed) / force.magnitude * speed;
            if (Vector2.Distance(transform.position, target.transform.position) < 0.2f) target = null;
        }
        else
        {
            force *= (speed - rigid.velocity.magnitude);
        }
        print(rigid.velocity.magnitude);
        rigid.velocity = Vector2.Lerp();
        rigid.AddForce(force + new Vector2(0, clearUpForce));*/
        Vector2 differrence = (Vector2)target.transform.position- rigid.position, normzdDif = differrence.normalized;
        rigid.velocity = Vector2.Lerp(rigid.velocity, normzdDif * speed, Time.fixedDeltaTime*acceleration);
        float distance = differrence.magnitude;
        // disturbing:
        if (Input.GetKeyDown(KeyCode.W))
        {
            rigid.AddForce(disturbingForce);
        }
    }
}
