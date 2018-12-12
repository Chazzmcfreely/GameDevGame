using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    Rigidbody2D rb;
    bool onWall = false;
    float xprev;
    float yprev;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xprev = rb.transform.position.x;
        yprev = rb.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (onWall)
        {
            transform.position = new Vector3(xprev, yprev, 0);
        }

        xprev = rb.transform.position.x;
        yprev = rb.transform.position.y;
    }

    void OnCollisionEnter2D(Collision2D collisioninfo)
    {
        // rb.transform.position()
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        if (collisioninfo.transform.gameObject.tag == "obstacle")
        {
            onWall = true;
        }
    }

}

       