using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour {

    public bool onGround;

    void OnTriggerEnter2D (Collider2D c) {
        if (c.gameObject.tag == "Floor") {
            onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Floor") {
            onGround = false;
        }
    }
}
