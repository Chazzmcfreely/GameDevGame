﻿using System.Collections;
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
    public float moveSpeed = 8;
    public GameObject TeleporterPrefab;

    public enum PlayerNum
    {
        Player1,
        Player2
    }
    public bool roundOver = false;

    public PlayerNum playerNum;

    public LayerMask enemyMask;

    public RoundEnd roundEnd;
    public Animator anim;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpHop;
    public Vector2 wallJumpLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .1f;
    float timeToWallUnstick;
    public float TeleporterSpeed;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    public float currentSpeed;
    public float dashSpeed = 40;
    float dashTimer = 0;
    Vector3 destination;
    Vector3 dashStart;

    public AudioSource source;
    public AudioClip Jump;
    public AudioClip runOne; 
    public AudioClip runTwo;


    int dashCount = 3;
    float[] lastDashTimes;
    float dashCoolDownDuration = 15f;

    public float lives = 3;

    bool isTeleporter = false;
    GameObject teleporter;

    Controller2D controller;

    private GameObject[] players = new GameObject[2];
    private List<BoxCollider2D> playerColliders = new List<BoxCollider2D>();

    string horizontalMove;
    string verticalMove;
    string jump;
    string self;
    string enemy;
    string color;
    string rightHorizontal;
    string rightVertical;
    string dash;
    string teleport;

    void Start()
    {
        source = GetComponent<AudioSource>();
        controller = GetComponent<Controller2D>();
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + "Jump Velocity: " + maxJumpVelocity);
        currentSpeed = moveSpeed;
        teleporter = null;

        lastDashTimes = new float[dashCount];

        if(playerNum == PlayerNum.Player1){
            horizontalMove = "Horizontal";
            verticalMove = "Vertical";
            jump = "Jump";
            self = "Player1";
            enemy = "Player2";
            color = "Red_";
            rightHorizontal = "RightHorizontal";
            rightVertical = "RightVertical";
            dash = "Dash";
            teleport = "Teleport";


        }else if(playerNum == PlayerNum.Player2){
            horizontalMove = "P2Horizontal";
            verticalMove = "P2Vertical";
            jump = "P2Jump"; //needs to be different
            self = "Player2";
            enemy = "Player1";
            color = "Blue_";
            rightHorizontal = "P2RightHorizontal";
            rightVertical = "P2RightVertical";
            dash = "P2Dash";
            teleport = "P2Teleport";


        }

        for (int i = 0; i < players.Length; i++)
        {
            playerColliders.Add(players[i].GetComponent<BoxCollider2D>());
            //Debug.Log(players[i].name);
        }
    }

    void Update() 
    {
        roundOver = RoundEnd.roundOver;

        Vector2 input = new Vector2(Input.GetAxisRaw(horizontalMove), Input.GetAxisRaw(verticalMove));
        float targetVelocityX = input.x * currentSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        //Debug.Log(velocity.x);
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
        HandleSpriteDirection();
       // if (roundOver){
         //   return; 
        //}
        if (dashTimer > 0) {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0) {
                dashTimer = 0;
            }
        }
        //currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, 0.2f);

        int wallDirX = (controller.collisions.left) ? -1 : 1;


        //increase drag force


        bool wallSliding = false;
       
        //////////////////////////////////////
        float angle = Mathf.Atan2(Input.GetAxis(rightHorizontal), Input.GetAxis(rightVertical)) * Mathf.Rad2Deg;
        //Debug.Log(angle + "right horizontal: " + Input.GetAxis(rightHorizontal));
        ///////////////////////////////////////


        //if(controller.collisions.below) 
        //{
        //    anim.SetBool("Ground", true); 
        //}
        //else {
        //    anim.SetBool("Ground", false);
        //}

        if (anim.GetBool("Ground") != controller.collisions.below) {
            anim.SetBool("Ground", (controller.collisions.below));
            //anim.SetTrigger("Jump");
        }


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
        else {
            wallSliding = false;
        }

        if (anim.GetBool("WallSliding") != wallSliding) {
            anim.SetBool("WallSliding", wallSliding);   
        }

        if (Input.GetButtonDown(teleport))
        {
            //make it only able to teleport when it has stoppped moving

            if (teleporter == null)
            {
                Vector3 MouseCords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 TeleporterRotation = new Vector3(MouseCords.x, MouseCords.y, 0f);
                Vector2 DirectionToMouse = MouseCords - transform.position;
                Vector2 dashDir = new Vector2(-1 * (Input.GetAxis(rightHorizontal)), Input.GetAxis(rightVertical)).normalized;



                DirectionToMouse.Normalize();
                float angleToMouse = Mathf.Rad2Deg * Mathf.Atan2(DirectionToMouse.y, DirectionToMouse.x) - 90;


                teleporter = Instantiate(TeleporterPrefab);
                //Physics2D.IgnoreCollision(teleporter.GetComponent<CircleCollider2D>(), playerCollider);

                for (int i = 0; i < playerColliders.Count; i++)
                {
                    Physics2D.IgnoreCollision(teleporter.GetComponent<CircleCollider2D>(), playerColliders[i]);
                }

                teleporter.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                teleporter.transform.eulerAngles = new Vector3(0, 0, angle);

                teleporter.GetComponent<Rigidbody2D>().velocity = dashDir * TeleporterSpeed; ;

                isTeleporter = true;
            }
            else
            {
                transform.position = teleporter.transform.position;

                // use it by teleporting


                Destroy(teleporter);
                teleporter = null;

            }

        } 


        if (Input.GetButtonDown(jump)) 
        {
            source.PlayOneShot(Jump, 0.5f);
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
        if (Input.GetButtonUp(jump))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }

        }

    
        velocity.y += gravity * Time.deltaTime;

        CheckDash();

        if (dashTimer > 0)
        {
            Vector3 lineToDestination = destination - transform.position;
            lineToDestination.Normalize();
            velocity = lineToDestination * dashSpeed;

            // raycast check for damage
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lineToDestination.normalized, 2, enemyMask);
            if (hit.collider != null) {
                Debug.Log("hit: " + hit.collider.gameObject.name + ", tag: " + hit.collider.gameObject.tag + ", this player's enemy is: " + enemy);
                //if (hit.collider.tag == enemy) {
                    //if (hit.distance < 2) {
                        Debug.Log("CHIPS");
                        ScoreControl.liveCount -= 1;
                        RoundEnd.EndRound();
                    //slow down time after hit
                //communicate to a different script that a player won, then turn off input
                // particle effects
                //then reload the 
                //}
                //}
            }
        }

        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

    }

    void CheckDash () {

        if (dashCount < 3) {
            Debug.Log("dash cool down check. dash count: " + dashCount);
            Debug.Log("dash time: " + (Time.time - lastDashTimes[dashCount]));
            if (Time.time - lastDashTimes[dashCount] > dashCoolDownDuration)
            {
                Debug.Log("increasing dashCount");
                dashCount++;
            }    
        }

        Debug.Log("dash count: " + dashCount);

        if (dashCount > 0) {
            if (Input.GetButtonDown(dash))
            {
                Vector2 dirToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                dirToMouse *= dashSpeed;
                velocity.x += dirToMouse.x;
                velocity.y += dirToMouse.y;
                dashTimer = 0.25f;

                dashStart = transform.position;
                destination = transform.position + (Vector3)dirToMouse;

                //Set the velocities to just equals
                // make everymovement a coditional, if you dash you need a global timer to delay movement until the timer runs out
                //
                lastDashTimes[dashCount - 1] = Time.time;
                dashCount--;
            }


            //Game pad Dash/////////////////////////////////////////////////////////////////////////
            if (Input.GetButtonDown(dash))
            {
                Debug.Log("trying to dash");
                //Vector2 dirToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                Vector2 dashDir = new Vector2(-1 * (Input.GetAxis(rightHorizontal)), Input.GetAxis(rightVertical)).normalized;
                dashDir *= dashSpeed;
                velocity.x += Input.GetAxis(rightHorizontal);
                velocity.y += Input.GetAxis(rightVertical);
                dashTimer = 0.25f;

                dashStart = transform.position;
                destination = transform.position + (Vector3)dashDir;

                //Set the velocities to just equals
                // make everymovement a coditional, if you dash you need a global timer to delay movement until the timer runs out
                //
                lastDashTimes[dashCount - 1] = Time.time;
                dashCount--;
            }
            //Game pad Dash/////////////////////////////////////////////////////////////////////////

        }

    }

  



    void HandleSpriteDirection()
    {
        // this code will flip the sprite direction by assigning the x component of the transform scale to be either positive or negative
        if (velocity.x > 0)
        {
            // moving right
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (velocity.x < 0)
        {
            // moving left
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }


}
