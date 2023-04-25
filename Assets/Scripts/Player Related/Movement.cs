using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform orientation;
    public LayerMask ground;
    public FOVVFX fovVFX;
    public Camera playerCamera;

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
    public int bHopCount;

    [Header("Grapple Variables")]
    public float grappleSpeed;
    public float grappleRange;
    public float grappleCooldown;
    public float grapplePointVerticalBoost;
    public bool  toggleControl;
    
    public LineRenderer lineRen;

    [Header("Grapple Start Variables")]
    public float grappleAccelerateTime;
    public float grappleAccelMaxSpeed;

    [Header("Grapple End Variables")]
    public float grappleVerticalBoost;
    public float grappleHorizontalBoost;
    public float grappleDecelerateTime;
    public float grappleDecelMaxSpeed;

    void Start()
    {
        groundDecel = groundDecel < 1 ? 1 : groundDecel;                                                      // sets groundDecel to 1 if its less than 1, groundDecel value less than 1 causes bugs
        baseMoveSpeed = moveSpeed;
    }

    bool isGrounded;
    bool wasInAir;
    void Update()
    {
        GetInputs();
        isGrounded = Physics.Raycast(orientation.position, -orientation.up, 1.0001f, ground);                 // check if player is on the ground
        if(isGrounded && wasInAir) StartBHopCoroutine();
        wasInAir = rb.velocity.y < 0;
        CapSpeed();
        if(Input.GetKeyDown(jump)) OnJumpPressed();
        if(Input.GetKeyDown(grapple))  OnGrapplePressed();
        if(Input.GetKeyUp(grapple) && !toggleControl) OnGrappleReleased();
    }

    void FixedUpdate()
    {
        MovePlayer();
        if(grappling)
        {
            float speed = baseMoveSpeed > moveSpeed ? baseMoveSpeed : moveSpeed;
            rb.velocity = VectorToGrapplePoint() * speed;                                                     // set velocity towards grapple point with some speed multiplier
            lineRen.SetPosition(0, orientation.position);                                           // set vertex 0 of line renderer to player position
            float dist = hitGrapplePoint ? 5f : 1.2f;
            if(Vector3.Distance(grapplePoint, orientation.position) <= dist)
            {
                Vector3 horizontal = grappleReflect * grappleHorizontalBoost;
                Vector3 vertical = transform.up * grappleVerticalBoost;
                if(hitGrapplePoint)
                {
                    horizontal = Vector3.zero;
                    vertical *= 1.5f;
                    hitGrapplePoint = false;
                }
                DoGrappleEnd(horizontal, vertical);
            }          // check if less than a distance of 1.1 from the grapplePoint
        }
        if(isGrounded)
        {
            if(!GetInputs() && rb.velocity.magnitude > 0) rb.velocity -= rb.velocity / groundDecel;         // slow player down if they were just moving
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
        if(grappling) return;
        float acceleration = isGrounded ? groundAccel : airAccel; // controls how fast the player accelerates

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
        Vector3 velo = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(velo.magnitude > moveSpeed + (bHopCount * bHopMultiplier))
        {
            velo = velo.normalized * (moveSpeed + (bHopCount * bHopMultiplier));
            rb.velocity = new Vector3(velo.x, rb.velocity.y, velo.z);
        }
    }

    void OnJumpPressed()
    {
        if(isGrounded) rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }

    // OnGrapplePressed() executes functions relating to grapple functionality.
    bool grappling;
    bool grappleOnCooldown;
    bool hitGrapplePoint;
    Vector3 grapplePoint;
    void OnGrapplePressed()
    {
        if(!grappling && !grappleOnCooldown)
        {
            RaycastHit hit;
            if(Physics.Raycast(orientation.position, playerCamera.transform.forward, out hit, grappleRange))
            {
                hitGrapplePoint = hit.transform.name == "GrapplePoint" || hit.transform.parent.name == "GrapplePoint" ? true : false;
                grapplePoint = hit.point;
                ToggleGrapple();
                CalculateReflectVector(hit.normal);
                int factor = hitGrapplePoint ? 10 : 1;
                LimitGrappleSpeed(grappleAccelMaxSpeed * factor, baseMoveSpeed, grappleAccelerateTime);
                fovVFX.GrappleStartVFX();
            }
        }
        else if(grappling && toggleControl) DoGrappleEnd(grappleAligned * grappleHorizontalBoost * 2, transform.up * grappleVerticalBoost);
    }

    // OnGrappleReleased() ends the player's grapple if toggleControl is set to false.
    void OnGrappleReleased()
    {
        if(grappling) DoGrappleEnd(grappleAligned * grappleHorizontalBoost * 2, transform.up * grappleVerticalBoost);
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

    /* VectorToGrapplePoint() returns the normalized vector from the player to
     * the grapplePoint.
     */
    Vector3 VectorToGrapplePoint()
    {
        Vector3 dir = grapplePoint - orientation.position;
        return dir.normalized;
    }

    /* CalculateReflectVector() calculates a vector reflected across the parameter
     * normal. It create a point that has the same y value as the grapple point and an
     * x and z component as the player's x and z position. It then creates the vector from
     * this point to the grapplePoint and then reflects it across the normal. The reflected
     * vector is stored in grappleReflect.
     */
    Vector3 grappleAligned;
    Vector3 grappleReflect;
    void CalculateReflectVector(Vector3 normal)
    {
        // vector that has same y as the grapple point and x/z of the player
        // is essentially on the same elevation of the grapple point and keeps y component 0
        Vector3 aligned = new Vector3(orientation.position.x, grapplePoint.y, orientation.position.z);                                                      
        grappleAligned = grapplePoint - aligned;                                                  // vector from the aligned point to the grapple point
        grappleReflect = Vector3.Reflect(grappleAligned, normal);                                 // the reflected aligned vector
        grappleReflect.Normalize();
        grappleAligned.Normalize();
    }

    
    /* DoGrappleEnd() launches the player is the direction provided by the
     * parameters horizontal and vertical. It also starts a coroutine that
     * decelerates the player's velocity.
     */
    void DoGrappleEnd(Vector3 horizontal, Vector3 vertical)
    {
        ToggleGrapple();
        rb.velocity = Vector3.zero;
        rb.velocity = horizontal + vertical;
        LimitGrappleSpeed(baseMoveSpeed, grappleDecelMaxSpeed, grappleDecelerateTime);
        fovVFX.GrappleEndVFX();
        StartCoroutine(StartGrappleCooldown());

        Debug.DrawLine(grapplePoint, grapplePoint + horizontal + vertical, Color.cyan, 1f);                     // debug vector that player get launched to
    }

    private IEnumerator StartGrappleCooldown()
    {
        grappleOnCooldown = true;
        yield return new WaitForSeconds(grappleCooldown);
        grappleOnCooldown = false;
    }

    /* LimitGrappleSpeed() keeps track if the grappleInterpolate is running. If
     * so it stops it then, sets the player's movement speed back to its original value
     * then restarts the coroutine.
     */
    bool grappleCoroutineRunning;
    IEnumerator grappleLerpCoroutine;
    void LimitGrappleSpeed(float start, float end, float timeFrame)
    {
        if(grappleCoroutineRunning) StopCoroutine(grappleLerpCoroutine);
        grappleLerpCoroutine = LerpGrappleSpeed(start, end, timeFrame);
        StartCoroutine(grappleLerpCoroutine);
    }

    /* InterpolatedGrappleSpeed() will decelerate/accelerate the speed of the player
     * when the grapple is actve and when they dismount or wall jump. Essentially linerally
     * interpolates up or down between values to limit the amount of speed the player retains
     * when/after grappling.
     */
    float baseMoveSpeed;
    private IEnumerator LerpGrappleSpeed(float start, float end, float window)
    {
        grappleCoroutineRunning = true;
        float timeLeft = window;

        while(timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            moveSpeed = Mathf.Lerp(start, end, Mathf.Clamp(timeLeft / window, 0f, 1f));  // get new interpolated move speed
            yield return null;
        }

        grappleCoroutineRunning = false;
    }

    /* StartBHopCoroutine() will stop the bHopCoroutine is it's running
     * and start if it isn't.
     */
    bool bHopCoroutineRunning;
    IEnumerator bHopCoroutine;
    void StartBHopCoroutine()
    {
        if(bHopCoroutineRunning) StopCoroutine(bHopCoroutine);
        bHopCoroutine = CheckBHopWindow();
        StartCoroutine(bHopCoroutine);
    }

    /* CheckBHopWindow() determines if the player is inside the b hop window 
     * and is triggered when the player lands on the ground after jumping.
     * If the player jumps while inside the window, bHopCount is incremented
     * and gives the player a small speed increase based on the amount of successfully
     * chained b hops.
     */
    private IEnumerator CheckBHopWindow()
    {
        bHopCoroutineRunning = true;
        float timer = bHopWindow;
        while(timer > 0)
        {
            if(Input.GetKeyUp(jump)){
                bHopCount += bHopCount < bHopMax ? 1 : 0;
                yield break;
            };
            timer -= Time.deltaTime;
            yield return null;
        }
        bHopCount = 0;
        bHopCoroutineRunning = false;
        yield break;
    }
}
