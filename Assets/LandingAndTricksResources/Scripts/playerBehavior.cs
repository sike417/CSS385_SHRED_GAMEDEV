using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerBehavior : MonoBehaviour
{
    enum State
    {
        Live, Die,Crash,Win
    };
    #region global variablesF
    // player variables
    public float maxSpeed = 140f;
    public const float jumpHeight = 600f;
    public const float rotationSpeed = 200f;
    public const float grindSpeed = 50;
    public float velocity = 0;
    private float speedBoostTime = 2f; // the duration the speed boost is applied for    
    private float speedMultiplier;
    private float AddedSpeed = 100.2f;
    private bool jumped = true;
    private bool boost = false;
    private bool isAboveRail = false;
    public bool attachedToRail = false;
    private bool trickComplete = false;
    private float initRotation;
    private Vector3 initPos;
    private float stateTimer = 0;
    private GameObject gm;
    private GlobalBehavior gb;
    private raycastUp rayCastLeft, rayCastRight;
    private State HeroState;

    // ground checker variables 
    private bool previousGround = false;    
    public LayerMask groundLayer;
    public Transform groundChecker;
    public bool isOnGround = false;
    private Rigidbody2D mRB;
    private float groundCheckerRadius = 1f;

    // head checker variables
    public Transform headChecker;
    private float headCheckerRadius = 0.8f;

    #endregion

    // Use this for initialization
    void Start()
    {
        initPos = transform.position;
        mRB = GetComponent<Rigidbody2D>();
        HeroState = State.Live;
        gm = GameObject.Find("Game Manager");

        gb = gm.GetComponent<GlobalBehavior>();

        rayCastLeft = GameObject.Find("ray_cast_left").GetComponent<raycastUp>();
        rayCastRight = GameObject.Find("ray_cast_right").GetComponent<raycastUp>();

        speedMultiplier = 1;
    }

    void Update()
    {
        velocity = mRB.velocity.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HeroState == State.Live)
        {
            // checks if the player is contacting the ground
            isOnGround = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);

            if (isOnGround)
            {
                previousGround = isOnGround;
            }


            if (isOnGround && Input.GetAxis("Jump") > 0)
            {
                initRotation = mRB.rotation;
                isOnGround = false;
                previousGround = false;
                jumped = true;
                mRB.AddForce(new Vector2(0, jumpHeight));
                gb.UpdateLandingText("Landing: In Air");
            }
            else if (!isOnGround)
            {
                if (previousGround)
                {
                    initRotation = mRB.rotation;
                    previousGround = false;
                }
                jumped = true;
                gb.UpdateLandingText("Landing: In Air");
            }
            // checks if player's head has hit the ground
            if (Physics2D.OverlapCircle(headChecker.position, headCheckerRadius, groundLayer))
            {
                HeroState = State.Crash;
                LocalDestroy();
            }

            //checks if player is above a grindable object
            if (rayCastLeft.grindable)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    attachedToRail = true;
                    isAboveRail = true;
                }
            }
            else
            {
                isAboveRail = false;
                attachedToRail = false;
            }

            if (attachedToRail)
            {
                PlayerAttachToRail();
            }

            // prevents character from move faster than the max speed;
            if (mRB.velocity.magnitude >= maxSpeed)
            {
                mRB.velocity = mRB.velocity.normalized * maxSpeed;
            }

            // player can only rotate when in the air
            if (!isOnGround && !isAboveRail)
            {
                mRB.MoveRotation(mRB.rotation - Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime);
                // mRB.AddTorque(Input.GetAxis("Horizontal") * -rotationSpeed);  different rotation method feels different
            }

            // makes the players stay parallel to the ground
            if (isOnGround && !jumped)
            {
                float turnSpeed = 2f;
                Vector3 targetVector = (rayCastRight.point - rayCastLeft.point).normalized;
                float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.fixedDeltaTime);
            }

            // apply speed boost for x amount of seconds
            if (boost && trickComplete)
            {
                speedBoostTime -= Time.deltaTime;
                if (speedBoostTime > 0f)
                {
                    mRB.AddForce(Vector3.Normalize(transform.right) * 20f * speedMultiplier);
                    gb.updateTimer(2f, speedBoostTime);
                }
                else
                {
                    boost = false;
                    trickComplete = false;
                    speedMultiplier = 1f;
                    speedBoostTime = 2f; // reset timer
                }
            }

            // update text
            if (speedMultiplier == 1f)
                gb.UpdateSpeedMulText("");
            else
                gb.UpdateSpeedMulText("Boost X " + speedMultiplier);

            if (jumped)
            {
                CheckForTricks();
            }
        }

        if (HeroState == State.Crash)//Player Crash
        {
            gb.UpdateLandingText("Landing: CRASH!");
            stateTimer += 1 * Time.smoothDeltaTime;

            if(stateTimer>1.5)
            {
                HeroState = State.Die;
            }
        }

        if (HeroState == State.Die)//Player Die
        {
            gb.PlayerDie();
            stateTimer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        #region ground trigger
        if (other.gameObject.CompareTag("GroundCollider") && jumped)
        {
            float angle = Vector2.Angle(this.transform.right, rayCastRight.point - rayCastLeft.point);

            if (angle <= 15f)
            {
                int reward = 20;

                jumped = false;

                if (trickComplete)
                    BoostPlayer(2f);

                gb.UpdateLandingText("Landing: Perfect! +" + reward);
                gb.UpdateScore(reward);
            }
            else if (angle <= 30f)
            {
                int reward = 15;

                if (trickComplete)
                    BoostPlayer(1.8f);

                jumped = false;

                gb.UpdateLandingText("Landing: Great! +" + reward);
                gb.UpdateScore(reward);
            }
            else if (angle <= 45f)
            {
                int reward = 10;

                if (trickComplete)
                    BoostPlayer(1.6f);

                jumped = false;

                gb.UpdateLandingText("Landing: Good! +" + reward);
                gb.UpdateScore(reward);

            }
            else if (angle <= 60f)
            {
                int reward = 5;

                if (trickComplete)
                    BoostPlayer(1.2f);

                jumped = false;

                gb.UpdateLandingText("Landing: Alright +" + reward);
                gb.UpdateScore(reward);
            }
            else if (angle <= 90f)
            {
                int reward = 2;

                if (trickComplete)
                    BoostPlayer(0.5f);

                jumped = false;

                gb.UpdateLandingText("Landing: Poor +" + reward);
                gb.UpdateScore(reward);
            }
            else
            {
                gb.UpdateLandingText("Landing: CRASH!");
                LocalDestroy();
                HeroState = State.Crash;
            }

            if (angle > 1)
            {
                mRB.AddForce((rayCastRight.point - rayCastLeft.point) * AddedSpeed * (1 / angle)); //added 1 / angle
            }
            else
            {
                mRB.AddForce((rayCastRight.point - rayCastLeft.point) * AddedSpeed);
            }
        }
        #endregion
    }

    void PlayerJump()
    {
        initRotation = mRB.rotation;
        isOnGround = false;
        jumped = true;
        mRB.AddForce(new Vector2(0, jumpHeight));
        gm.GetComponent<GlobalBehavior>().UpdateLandingText("Landing: In Air");
    }

    void PlayerAttachToRail()
    {
        gb.UpdateScore(2);
        float turnSpeed = 10f;
        float newX = transform.position.x + 0.5f;
        Vector3 targetVector = rayCastLeft.gTransform.right;
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.deltaTime);
        Vector3 destination = new Vector3(newX, rayCastLeft.railY + (gameObject.GetComponent<Collider2D>().bounds.center.y - gameObject.GetComponent<Collider2D>().bounds.min.y)/2, 0f);
        transform.position = Vector3.Lerp(transform.position, destination, 100f * Time.deltaTime);
        if (Input.GetAxis("Jump") > 1)
        {
            attachedToRail = false;
            PlayerJump();
        }
    }
    public void winPlayer()
    {
        HeroState = State.Win;
    }
    void BoostPlayer(float boostMul)
    {
        speedMultiplier = boostMul;
        boost = true;
    }
    public string getState()
    {
        return HeroState.ToString();
    }
    public void CrashPlayer()
    {
        HeroState = State.Crash;
       
    }
    void CheckForTricks()
    {
        int reward = 50;
        float trickRotation = mRB.rotation;
        float deltaAngle = 0;
        float absouluteAngle = 0;
        int tricksInARow = 1;

        deltaAngle += initRotation - trickRotation;
        absouluteAngle += Mathf.Abs(initRotation - trickRotation);

        if (deltaAngle > 330f)
            gb.UpdateTrickText("Trick: Frontflip");

        else if (deltaAngle < -330f)
            gb.UpdateTrickText("Trick: Backflip");

        tricksInARow = (int)(absouluteAngle / 330f);

        if (isOnGround && tricksInARow > 1)
        {
            trickComplete = true;
            gb.UpdateScore(reward * tricksInARow);
            gb.UpdateTrickText("Trick: Combo +" + reward + " X " + tricksInARow);
        }
        else
        {
            // front flip detection
            if (isOnGround && deltaAngle > 330f)
            {
                trickComplete = true;
                gb.UpdateScore(reward);
                gb.UpdateTrickText("Trick: Backflip +" + reward);
            }

            // back flip detection
            if (isOnGround && deltaAngle < -330f)
            {
                trickComplete = true;
                gb.UpdateScore(reward);
                gb.UpdateTrickText("Trick: Backflip +" + reward);
            }
        }
    }
    public void Retry()
    {
        transform.position = initPos;
        mRB.velocity = new Vector2(0, 0);
        HeroState = State.Live;
        mRB.rotation = 0f;
    }
    void LocalDestroy()
    {
        speedMultiplier = 0f;
        boost = false;
        jumped = false;
        //gb.DestroyMe();
    }
}
