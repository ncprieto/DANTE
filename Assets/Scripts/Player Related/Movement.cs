using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform orientation;
    public LayerMask ground;

    [Header("Input")]
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode grapple;

    [Header("Ground Movement Variables")]
    public float moveSpeed;
    public float groundAccel;
    public float groundDecel;
    public float airAccel;
    public float jumpHeight;

    private bool justJumped;
    private bool apexReached;

    [Header("B Hop Variables")]
    public int bHopMax;
    public float bHopMultiplier;
    public float bHopWindow;
    private int bHopCount;

    [Header("Grapple Variables")]
    public float grappleSpeed;
    public float grappleVerticalBoost;
    public float grappleHorizontalBoost;
    public float grappleDecelerateTime;
    public float grappleDecelMaxSpeed;
    public float grappleAccelerateTime;
    public float grappleAccelMaxSpeed;
    public Transform playerCamera;
    public LineRenderer lineRen;

    void Start()
    {
        groundDecel = groundDecel < 1 ? 1 : groundDecel;                                                      // sets groundDecel to 1 if its less than 1, groundDecel value less than 1 causes bugs
        baseMoveSpeed = moveSpeed;
    }

    bool isGrounded;
    void Update()
    {
        GetInputs();
        
        isGrounded = Physics.Raycast(orientation.position, -orientation.up, 1.0001f, ground);                 // check if player is on the ground
        if(isGrounded && justJumped)
        {
            StartCoroutine(CheckBHopWindow());
            justJumped = false;
        }
        CapSpeed();
        if(Input.GetKeyDown(jump)){ OnJumpPressed(); }
        if(Input.GetKeyDown(grapple)) { OnGrapplePressed(); }
    }

    void FixedUpdate()
    {
        MovePlayer();
        if(grappling)
        {
            float speed = baseMoveSpeed > moveSpeed ? baseMoveSpeed : moveSpeed;
            rb.velocity = GetGrappleVector() * speed;                                                         // set velocity towards grapple point with some speed multiplier
            lineRen.SetPosition(0, orientation.position);                                                     // set vertex 0 of line renderer to player position
            if(Vector3.Distance(grapplePoint, orientation.position) < 1.1f) { DoGrappleWallJump(); }          // check if less than a distance of 1.1 from the grapplePoint
        }
        if(isGrounded)
        {
            if(!GetInputs() && rb.velocity.magnitude > 0){ rb.velocity -= rb.velocity / groundDecel; }        // slow player down if they were just moving
        }
        CapSpeed();
    }
    
    int fb;
    int lr;
    bool GetInputs()
    {
        // converts inputs into 0, 1, or -1 used for multiplying direction vectors
        fb =  Input.GetKey(forward)  ? 1 : 0;
        fb += Input.GetKey(backward) ? -1 : 0;

        lr =  Input.GetKey(right) ? 1 : 0;
        lr += Input.GetKey(left)  ? -1 : 0;

        // return a boolean to see if the player is even inputing anything
        return fb != 0 || lr != 0 ? true : false;
    }

    /* GetWishDirection() returns a Vector3 that points in the direction the
     * player is wanting to move in based on their inputs and their transform.
     * It returns a normalized direction vector.
     */
    Vector3 GetWishDirection()
    {
        Vector3 dir = orientation.forward * fb + orientation.right * lr;
        return dir.normalized;
    }

    void MovePlayer()
    {
        float acceleration = isGrounded ? groundAccel : airAccel; // controls how fast they player
        // essentially quake 3 movement phyics
        Vector3 wishDir = GetWishDirection();
        float current = Vector3.Dot(rb.velocity, wishDir);
        float addSpeed = moveSpeed - current;
        addSpeed = Mathf.Max(Mathf.Min(addSpeed, acceleration * Time.deltaTime), 0);
        // get bHop multiplier
        float bHop = bHopCount > 0 ? bHopMultiplier * bHopCount : 1;
        rb.velocity += wishDir * addSpeed * bHop;
    }

    void CapSpeed()
    {
        // if(grappling) return;

        Vector3 velo = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(velo.magnitude > moveSpeed + (bHopCount * bHopMultiplier))
        {
            velo = velo.normalized * (moveSpeed + (bHopCount * bHopMultiplier));
            rb.velocity = new Vector3(velo.x, rb.velocity.y, velo.z);
        }
    }

    void OnJumpPressed()
    {
        if(isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            justJumped = true;
        }
    }

    /* OnGrapplePressed() executes functions relating to grapple functionality.
     */
    bool grappling;
    Vector3 grapplePoint;
    Vector3 grappleReflect;
    void OnGrapplePressed()
    {
        if(!grappling)
        {
            RaycastHit hit;
            if(Physics.Raycast(orientation.position, playerCamera.forward, out hit, Mathf.Infinity))
            {
                grapplePoint  = hit.point;
                ToggleGrapple();
                CalculateReflectVector(hit.normal);
                StartGrappleCoroutine(grappleAccelerateTime, 0f, "accelerate", grappleAccelMaxSpeed);
            }
        }
        else if(grappling) { DoGrappleDismount(); }
    }

    /* ToggleGrapple() turns the grappling boolean and the line renderer on
     * and off. It also resets the vertices of the line renderer if the player
     * is grappling.
     */
    void ToggleGrapple()
    {
        grappling = !grappling;
        lineRen.enabled = !lineRen.enabled;
        if(grappling)
        {
            // reset line renderer points
            lineRen.SetPosition(0, grapplePoint);
            lineRen.SetPosition(1, grapplePoint);
        }
    }

    /* GetGrappleVector() returns the normalized vector from the player to
     * the grapplePoint.
     */
    Vector3 GetGrappleVector()
    {
        Vector3 dir = grapplePoint - orientation.position;
        return dir.normalized;
    }

    /* CalculateReflectVector() calculates the a vector reflected across the parameter
     * normal. It create a point that has the same y value as the grapple point and an
     * x and z component as the player's x and z position. It then creates the vector from
     * this point to the grapplePoint and then reflects it across the normal. The reflected
     * vector is stored in grappleReflect.
     */
    Vector3 grappleAligned;
    void CalculateReflectVector(Vector3 normal)
    {
        // vector that has same y as the grapple point and x/z of the player
        // is essentially on the same elevation of the grapple point and keeps y component 0
        Vector3 aligned = new Vector3(orientation.position.x, grapplePoint.y, orientation.position.z);                                                      
        grappleAligned = grapplePoint - aligned;                                                      // vector from the aligned point to the grapple point
        grappleReflect = Vector3.Reflect(grappleAligned, normal);                                     // the reflected aligned vector

        // debug stuff for visualizing where the player is going, what reflected direction
        // they are traveling in if they grapple jump, and the normal of the surface grappled to
        Debug.DrawLine(aligned,   aligned + grappleAligned,       Color.green, 1f);                   // aligned vector from player to grapple point
        Debug.DrawLine(grapplePoint, grapplePoint + normal,     Color.black, 1f);                     // surface normal at grapple point
    }

    /* DoGrappleWallJump() calculates a vector in the direction the player will
     * wall jump off of a surface and applies it to the player's rigidbody.
     */
    void DoGrappleWallJump()
    {
        ToggleGrapple();                                                                              // disconnect player from grapple once they come close enough to the point

        rb.velocity = Vector3.zero;
        Vector3 grappleJumpDir = grappleReflect.normalized * grappleHorizontalBoost + transform.up * grappleVerticalBoost; // exit vector of the grapple wall jump
        rb.velocity = grappleJumpDir;
        StartGrappleCoroutine(grappleDecelerateTime, 0f, "decelerate", grappleDecelMaxSpeed);

        Debug.DrawLine(grapplePoint, grapplePoint + grappleJumpDir, Color.red, 1f);                   // debug vector from grapple point to the exit direction of player
    }

    /* DoGrappleDismount() sets the player velocity to the direction in which they
     * grappled and applies a small vertical boost. Recreates the effect of jumping
     * off of the grapple when dismounting.
     */
    void DoGrappleDismount()
    {
        ToggleGrapple();

        Vector3 dismountDir = grappleAligned.normalized * grappleHorizontalBoost * 2 + transform.up * grappleVerticalBoost;
        rb.velocity = dismountDir;
        StartGrappleCoroutine(grappleDecelerateTime, 0f, "decelerate", grappleDecelMaxSpeed);

        Debug.DrawLine(grapplePoint, grapplePoint + dismountDir, Color.cyan, 1f);                     // debug vector that player get launched to
    }

    /* StartGrappleCoroutine() keeps track if the grappleDecelCoroutine is running. If
     * so it stops it then, sets the player's movement speed back to its original value
     * then restarts the coroutine.
     */
    bool grappleCoroutineRunning;
    IEnumerator grappleDecelCoroutine;
    IEnumerator grappleInterpolateCoroutine;
    void StartGrappleCoroutine(float startTime, float lerpStart, string mode, float max)
    {
        if(grappleCoroutineRunning)
        {
            StopCoroutine(grappleInterpolateCoroutine);
            grappleCoroutineRunning = false;
            if(mode == "decelerate") { moveSpeed = baseMoveSpeed; }
        }

        grappleInterpolateCoroutine = InterpolateGrappleSpeed(startTime, lerpStart, mode, max);
        StartCoroutine(grappleInterpolateCoroutine);
    }

    /* DecelerateAfterGrapple() will slowly limit the speed the player gains form 
     * grappling after they either grapple wall jump or dismount. Essentially linerally
     * interpolates from a max speed value back down to the original move speed value in
     * an amount of time that is determined by grappleSlowTimer.
     */
    float baseMoveSpeed;
    // private IEnumerator DecelerateAfterGrapple()
    // {
    //     grappleCoroutineRunning = true;
    //     float timeLeft = grappleSlowTimer;
    //     float t = 1.0f;

    //     while(timeLeft > 0)
    //     {
    //         timeLeft -= Time.deltaTime;
    //         t = timeLeft / grappleSlowTimer;                                      // get interpolation value from timer
    //         float newMoveSpeed = Mathf.Lerp(baseMoveSpeed, grappleMaxSpeed, t);   // get new interpolated move speed
    //         moveSpeed = newMoveSpeed;
    //         yield return null;
    //     }

    //     moveSpeed = baseMoveSpeed;
    // }

    // float baseMoveSpeed;
    private IEnumerator InterpolateGrappleSpeed(float window, float lerpStart, string mode, float maxSpeed)
    {
        grappleCoroutineRunning = true;
        float timeLeft = window;
        float t = lerpStart;
        Debug.Log("TIMER " + window);
        Debug.Log("STARTING INTERPOLATION VALUE " + lerpStart);
        Debug.Log("MODE " + mode);
        Debug.Log("MAX SPEED " + maxSpeed);

        while(timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            t = mode == "decelerate" ? timeLeft / window : 1 - (timeLeft / window);
            Debug.Log("T " + t);
            float newMoveSpeed = Mathf.Lerp(baseMoveSpeed, maxSpeed, t);   // get new interpolated move speed
            moveSpeed = newMoveSpeed;
            Debug.Log("NEW MOVE SPEED " + moveSpeed);
            yield return null;
        }

        // moveSpeed = baseMoveSpeed;
        grappleCoroutineRunning = false;
        moveSpeed = mode == "decelerate" ?  baseMoveSpeed  : moveSpeed;
    }
    
    private bool CheckWallJump()
    {
        return false;
    }

    /* CheckBHopWindow() determines if the player is inside the b hop window 
     * and is triggered when the player lands on the ground after jumping.
     * If the player jumps while inside the window, bHopCount is incremented
     * and gives the player a small speed increase based on the amount of successfully
     * chained b hops.
     */
    private IEnumerator CheckBHopWindow()
    {
        float timer = bHopWindow;

        while(timer > 0)
        {
            if(Input.GetKeyDown(jump)){
                bHopCount += bHopCount < bHopMax ? 1 : 0;
                yield break; // completely breaks out of the coroutine
            };

            timer -= Time.deltaTime;
            yield return null;
        }

        bHopCount = 0;
        yield break;
    }
}
