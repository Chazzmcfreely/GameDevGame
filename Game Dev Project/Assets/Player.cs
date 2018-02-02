using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public KeyCode rightKey;
    public KeyCode leftKey;

    public float speed;

    Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection *= 0.8f;

        if (Input.GetKey(rightKey))
        {
            moveDirection += Vector2.right;
        }

        if (Input.GetKey(leftKey))
        {
            moveDirection += Vector2.left;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)transform.position + (moveDirection * speed * Time.fixedDeltaTime);
        rb.MovePosition(position);
    }
}
