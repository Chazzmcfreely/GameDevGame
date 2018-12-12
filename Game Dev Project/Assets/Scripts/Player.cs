using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public Transform arrowPivot;

    public enum PlayerNum
    {
        Player1,
        Player2
    }
    public bool roundOver = false;

    public PlayerNum playerNum;
    float lastAngle;
    public LayerMask enemyMask;

    public RoundEnd roundEnd;
    public Animator anim;

    public ParticleSystem walkingParticles;
    public ParticleSystem teleporterBurst;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpHop;
    public Vector2 wallJumpLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .1f;
    float timeToWallUnstick;
    public float TeleporterSpeed;

    public Image DashCoolDownOne;
    public Image DashCoolDownTwo;
    public Image DashCoolDownThree;
    public Image TeleporterCoolDownOne;
    public Image TeleporterCoolDownTwo;
    public Image[] scoreNums = new Image[6];

    public Vector2 dashDir;
    public bool isDashing = false;
    public float hitTimer = 0.1f;
    float maxHitTimer = 0.1f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
   public Vector3 velocity;
    float velocityXSmoothing;
    public float currentSpeed;
    public float dashSpeed = 40;
    float dashDuration = 0;
    Vector3 destination;
    Vector3 dashStart;
    Vector2 lastDashDir;
    public AudioSource source;
    public AudioClip Jump;
    public AudioClip runOne; 
    public AudioClip runTwo;
    public AudioClip Dash;
    public AudioClip Teleport;
    public AudioClip Death;
    public bool ifMovedOnce = false;
    public float hitPauseTimer = .05f;
    float hitPauseOriginal = 0f;
    public bool hitPauseNow = false;
    public bool hitPauseDone = false;

    public float lives = 3;

   // bool isTeleporter = false;
    GameObject teleporter;

    Controller2D controller;

    private GameObject[] players = new GameObject[2];
    private List<BoxCollider2D> playerColliders = new List<BoxCollider2D>();

    string horizontalMove;
    string verticalMove;
    string jump;
    //string self;
    string enemy;
    string color;
    string rightHorizontal;
    string rightVertical;
    string dash;
    string teleport;
    int score;

    float[] dashTimers = new float[3] {
        0, 0, 0
    };

    float[] teleporterTimers = new float[2] {
        0, 0
    };


    int dashesAvailable = 3;
    int teleportersAvailable = 2;

    float dashCoolDownDuration = 5f;
    float teleporterCoolDownDuration = 10f;

   
  //Hit_pause hit_Pause;

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
       // hit_Pause = GetComponent<Hit_pause>();

        if(playerNum == PlayerNum.Player1){
            horizontalMove = "Horizontal";
            verticalMove = "Vertical";
            jump = "Jump";
           // self = "Player1";
            enemy = "Player2";
            color = "Red";
            rightHorizontal = "RightHorizontal";
            rightVertical = "RightVertical";
            dash = "Dash";
            teleport = "Teleport";


        }else if(playerNum == PlayerNum.Player2){
            horizontalMove = "P2Horizontal";
            verticalMove = "P2Vertical";
            jump = "P2Jump"; //needs to be different
            //self = "Player2";
            enemy = "Player1";
            color = "Blue";
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

        hitPauseOriginal = hitPauseTimer;
        lastAngle = 90f;
    }

    void Update() 
    {
        if(playerNum == PlayerNum.Player1){
            score = ScoreManager.RedScore;
            if(RoundEnd.Player2Win){
                return;
            }
        }else if(playerNum == PlayerNum.Player2){
            score = ScoreManager.BlueScore;
            if (RoundEnd.Player1Win)
            {
                return;
            }
        }

        roundOver = RoundEnd.roundOver;
        arrowPivot.position = transform.position;
        if (ifMovedOnce == true)
        {
            arrowPivot.gameObject.SetActive(true);
        }
        else{
            arrowPivot.gameObject.SetActive(false);
        }
        Vector2 input = new Vector2(Input.GetAxisRaw(horizontalMove), Input.GetAxisRaw(verticalMove));
        float targetVelocityX = input.x * currentSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        //Debug.Log(velocity.x);
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
        HandleSpriteDirection();
       // if (roundOver){
         //   return; 
        //}
        if (dashDuration > 0) {
            dashDuration -= Time.deltaTime;
            if (dashDuration < 0) {
                dashDuration = 0;
            }
        }
        //currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, 0.2f);

        int wallDirX = (controller.collisions.left) ? -1 : 1;


        //increase drag force


        bool wallSliding = false;
       
        //////////////////////////////////////
        float angle = Mathf.Atan2(Input.GetAxis(rightHorizontal), Input.GetAxis(rightVertical)) * Mathf.Rad2Deg;
        if (Input.GetAxis(rightHorizontal) != 0f || Input.GetAxis(rightVertical) != 0f){
            arrowPivot.gameObject.SetActive(true);
            arrowPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            lastAngle = angle;
           
        }
        else
        {
            arrowPivot.rotation = Quaternion.Euler(new Vector3(0, 0, lastAngle));
           
        }
        Debug.Log("last angle is:" + lastAngle);




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


        ////// <summary>
        /// TELEPORTERS
        /// </summary>
        /// <returns><c>true</c>, if available was teleportered, <c>false</c> otherwise.</returns>
       // CheckTeleporter();

            
            if (Input.GetButtonDown(teleport))
            {
            Debug.Log("Trying to teleport");

            if (teleporterAvailable() && teleporter == null)
            {
                int teleporterTimerIndex = 9999;
                for (int i = 0; i < teleporterTimers.Length; i++)
                {
                    if (teleporterTimers[i] <= 0)
                    {
                        teleporterTimerIndex = i;
                        break;
                    }
                }

                teleporterTimers[teleporterTimerIndex] = teleporterCoolDownDuration;
                //make it only able to teleport when it has stoppped moving


                Vector3 MouseCords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               // Vector3 TeleporterRotation = new Vector3(MouseCords.x, MouseCords.y, 0f);
                Vector2 DirectionToMouse = MouseCords - transform.position;
                Vector2 dashDir = new Vector2(-1 * (Input.GetAxis(rightHorizontal)), Input.GetAxis(rightVertical)).normalized;



                DirectionToMouse.Normalize();
             //   float angleToMouse = Mathf.Rad2Deg * Mathf.Atan2(DirectionToMouse.y, DirectionToMouse.x) - 90;


                teleporter = Instantiate(TeleporterPrefab);
                //Physics2D.IgnoreCollision(teleporter.GetComponent<CircleCollider2D>(), playerCollider);

                for (int i = 0; i < playerColliders.Count; i++)
                {
                    Physics2D.IgnoreCollision(teleporter.GetComponent<CircleCollider2D>(), playerColliders[i]);
                }

                teleporter.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                teleporter.transform.eulerAngles = new Vector3(0, 0, angle);

                teleporter.GetComponent<Rigidbody2D>().velocity = dashDir * TeleporterSpeed; ;
                teleporterBurst.Emit(100);

                //isTeleporter = true;

            }
            else
            {
                if (teleporter != null)
                {
                    source.PlayOneShot(Teleport, 0.5f);

                    transform.position = teleporter.transform.position;
                    //teleporterBurst.Emit(100);
                    // use it by teleporting


                    Destroy(teleporter);
                    teleporter = null;

                }
            }

            }

        //////////////////////////Teleporters /////////////////////////////

        //////score handling//////
        for (int i = 0; i < scoreNums.Length; i++){
            if (i == score) {
                scoreNums[i].gameObject.SetActive(true);
            }
            else {
                scoreNums[i].gameObject.SetActive(false);
            }

        }


      

        /////////////////////// Jumping ////////////////////
        if (Input.GetButtonDown(jump)) 
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                    source.PlayOneShot(Jump, 0.5f);

                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpHop.x;
                    velocity.y = wallJumpHop.y;
                    source.PlayOneShot(Jump, 0.5f);

                }
                else 
                {
                    velocity.x = -wallDirX * wallJumpLeap.x;
                    velocity.y = wallJumpLeap.y;
                    source.PlayOneShot(Jump, 0.5f);

                }
            }

            if(controller.collisions.below) 
            {
                velocity.y = maxJumpVelocity;
                source.PlayOneShot(Jump, 0.5f);

            }

        }
        if (Input.GetButtonUp(jump))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }

        }
        ////////////////////////////////////////////// Jumping //////////////////////////



    
        velocity.y += gravity * Time.deltaTime;

        dashDir = new Vector2(-1 * (Input.GetAxis(rightHorizontal)), Input.GetAxis(rightVertical)).normalized;
        if (isAiming() == true)
        {
            lastDashDir = dashDir;
        }

        if (ifMovedOnce == true)
        {
            CheckDash();
        }

        if (dashDuration > 0)
        {
            Vector3 lineToDestination = destination - transform.position;
            lineToDestination.Normalize();
            velocity = lineToDestination * dashSpeed;

            // raycast check for damage
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lineToDestination.normalized, 2, enemyMask);
            Debug.DrawRay(transform.position, lineToDestination.normalized, Color.red);
          

            if (hit.collider != null) {
                Debug.Log("hit: " + hit.collider.gameObject.name + ", tag: " + hit.collider.gameObject.tag + ", this player's enemy is: " + enemy);
                if (hit.collider.gameObject.tag == enemy)
                {
                    isDashing = true;

                    source.PlayOneShot(Death, 1f);

                    RoundEnd.EndRound(color);
                }
            }
        }



        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        ///Hit Pause Section
        if (hitPauseNow == true)
        {
            hitPauseTimer -= Time.deltaTime;
            if (hitPauseTimer <= 0)
            {
                DashNow();
            }
        }

    }
    //update ends here


    bool teleporterAvailable () {
        for (int i = 0; i < teleporterTimers.Length; i++) {
            if (teleporterTimers[i] <= 0) {
                return true;
            }
        }

        return false;
    }

    bool DashAvailable () {
        
        for (int i = 0; i < dashTimers.Length; i++) {
            if (dashTimers[i] <= 0) {
                return true;
            }
        }

        return false;
    }

    bool isAiming()
    {
        if (Input.GetAxis(rightHorizontal) != 0 || Input.GetAxis(rightVertical) != 0) {
            ifMovedOnce = true;
            return true;
           
        }

        return false;
    }

    void CheckDash()
    {

        //Game pad Dash/////////////////////////////////////////////////////////////////////////
        if (Input.GetButtonDown(dash))
        {
            hitPauseNow = true;
        }
            dashesAvailable = 0;
        for (int i = 0; i < dashTimers.Length; i++)
        {
            dashTimers[i] -= Time.deltaTime;
            if (dashTimers[i] <= 0)
            {
                dashesAvailable++;
            }
            //Debug.Log("dash index: " + i + ". dash timer: " + dashTimers[i]);

            //yes its bad coding but it works for now 
            if (dashesAvailable == 1)
            {
                DashCoolDownOne.gameObject.SetActive(true);
                DashCoolDownTwo.gameObject.SetActive(false);
                DashCoolDownThree.gameObject.SetActive(false);

            }
            else if (dashesAvailable == 2)
            {
                DashCoolDownOne.gameObject.SetActive(true);
                DashCoolDownTwo.gameObject.SetActive(true);
                DashCoolDownThree.gameObject.SetActive(false);
            }
            else if (dashesAvailable == 3)
            {
                DashCoolDownOne.gameObject.SetActive(true);
                DashCoolDownTwo.gameObject.SetActive(true);
                DashCoolDownThree.gameObject.SetActive(true);

            }
            else
            {
                DashCoolDownOne.gameObject.SetActive(false);
                DashCoolDownTwo.gameObject.SetActive(false);
                DashCoolDownThree.gameObject.SetActive(false);
            }
        }

        teleportersAvailable = 0;
        for (int i = 0; i < teleporterTimers.Length; i++)
        {
            teleporterTimers[i] -= Time.deltaTime;
            if (teleporterTimers[i] <= 0)
            {
                teleportersAvailable++;
            }
            //Debug.Log("dash index: " + i + ". dash timer: " + dashTimers[i]);
            if (teleportersAvailable == 1)
            {
                TeleporterCoolDownOne.gameObject.SetActive(true);
                TeleporterCoolDownTwo.gameObject.SetActive(false);
            }
            else if (teleportersAvailable == 2)
            {
                TeleporterCoolDownOne.gameObject.SetActive(true);
                TeleporterCoolDownTwo.gameObject.SetActive(true);
            }
            else
            {
                TeleporterCoolDownOne.gameObject.SetActive(false);
                TeleporterCoolDownTwo.gameObject.SetActive(false);
            }
        }

        // send info to ui
        //create public array with references to ui images that were all ready created, 
        // turn on how many dashes avaible and turn off those that aren't

     }
        //everything else going in a seperate method

        //Game pad Dash/////////////////////////////////////////////////////////////////////////
    void DashNow()
    {
     Debug.Log("trying to dash");
           // if (isAiming() == true)
          //  {

                if (DashAvailable())
                {
                    source.PlayOneShot(Dash, 0.8f);
                    screenShake shaker = GameObject.Find("Main Camera").GetComponent<screenShake>();
                    shaker.Screenshake(10f, 10f);

                    int dashTimerIndex = 9999;
                    for (int i = 0; i < dashTimers.Length; i++)
                    {
                        if (dashTimers[i] <= 0)
                        {
                            dashTimerIndex = i;
                            break;
                        }
                    }


                    //f hit_Pause.Freeze(); 


                   // if (hitPauseTimer <= 0)
                   // {
                        dashTimers[dashTimerIndex] = dashCoolDownDuration;


                        //Vector2 dirToMouse = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                     
                        
                        lastDashDir *= dashSpeed;
                        velocity.x += Input.GetAxis(rightHorizontal);
                        velocity.y += Input.GetAxis(rightVertical);
                        dashDuration = 0.25f;
                        //Michael says
                        //We make it so that dash direction doesnt get a value until a timer is done
                        //This means we have to make a timer float that counts down.
                        //This should fix the hit pause problem

                        dashStart = transform.position;
                        destination = transform.position + (Vector3)lastDashDir;
                  // }
                    //Set the velocities to just equals
                    // make everymovement a coditional, if you dash you need a global timer to delay movement until the timer runs out
                    //
                }
            //}
            hitPauseNow = false;
            hitPauseDone = false;
            hitPauseTimer = hitPauseOriginal;

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
    /*
    void HandleRedScore() {
        string currentScore;
        for (int i = 0; i <= 5; i++) {
            if (i == ScoreManager.redScore) {
                currentScore = "redNum" + ScoreManager.redScore.ToString();
                currentScore.gameObject.SetActive(true);
            }
        }
    }
*/


}
