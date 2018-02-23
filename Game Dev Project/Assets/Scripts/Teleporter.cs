using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    Rigidbody2D rb;
    bool onWall = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	if (onWall)
        {
           // rb.transform.position
        }


	}

    void OnCollisionEnter2D(Collision2D collisioninfo)
    {
        // rb.transform.position()
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }
}
