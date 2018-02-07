using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode jumpKey;
    public GameObject TeleporterPrefab;

    //bool hasJump = true;
    public float jumpVelocity;
    public float moveSpeed;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    public float TeleporterSpeed;
    public GroundDetection groundDetection;

    Vector2 moveDirection = Vector2.zero;
    bool jump = false;
    Rigidbody2D rb;

    GameManager gameManager;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       // gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Vector2.zero;

        if (Input.GetKeyDown(rightKey)) {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(leftKey)) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetKey(rightKey))
        {
            moveDirection += Vector2.right;

        }

        if (Input.GetKey(leftKey))
        {
            moveDirection += Vector2.left;

            //transform.localRotation = Quaternion.Euler(0, 0, 0);

        }

        jump = false;
        if (Input.GetKey(jumpKey) && groundDetection.onGround)
        {
            Debug.Log("trying to jump");
            jump = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 MouseCords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 TeleporterRotation = new Vector3(MouseCords.x, MouseCords.y, 0f);
            Vector2 DirectionToMouse = MouseCords - transform.position;



            DirectionToMouse.Normalize();
            float angleToMouse = Mathf.Rad2Deg * Mathf.Atan2(DirectionToMouse.y, DirectionToMouse.x) - 90;


            GameObject newTeleporter = Instantiate(TeleporterPrefab);
            newTeleporter.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            newTeleporter.transform.eulerAngles = new Vector3(0, 0, angleToMouse);

            newTeleporter.GetComponent<Rigidbody2D>().velocity = DirectionToMouse * TeleporterSpeed; ;
        }







    }

    void FixedUpdate()
    {
        //Vector2 position = (Vector2)transform.position + (moveDirection * speed * Time.fixedDeltaTime);
        //rb.MovePosition(position);
        Debug.Log("move direction: " + moveDirection);
        Debug.Log("can jump: " + jump);
        Debug.Log("move force: " + (moveDirection * moveSpeed * Time.fixedDeltaTime));
        rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime);
        if (jump) {
            rb.AddForce(Vector2.up * jumpVelocity * Time.fixedDeltaTime, ForceMode2D.Impulse);   
        }
    }

    void AddValueToScore(int value) {
        gameManager.score += value;
        Debug.Log("Current Score: " + gameManager.score);
        
    }

    //void OnCollisionEnter2D(Collision2D CollisionInfo)
    //{
    //    if (CollisionInfo.gameObject.tag == "Floor")
    //    {
    //        //Physics2D.OverlapArea();
    //        hasJump = true;
    //        print("contact");
    //    }
    //    else { hasJump = false; }

    //}
}
