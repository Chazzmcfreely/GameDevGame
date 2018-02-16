using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpHop;
    public Vector2 wallJumpLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .1f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + "Jump Velocity: " + maxJumpVelocity);
    }

    void Update() 
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;

        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }


        }



        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpHop.x;
                    velocity.y = wallJumpHop.y;
                }
                else 
                {
                    velocity.x = -wallDirX * wallJumpLeap.x;
                    velocity.y = wallJumpLeap.y;
                }
            }

            if(controller.collisions.below) 
            {
                velocity.y = maxJumpVelocity;
            }

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }

        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

    }
}
/*
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

        if (Input.GetKeyDown(rightKey))
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(leftKey))
        {
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
        if (Input.GetKeyDown(jumpKey))//&& groundDetection.onGround)
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
        if (jump)
        {
            rb.AddForce(Vector2.up * jumpVelocity * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void AddValueToScore(int value)
    {
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
 */
