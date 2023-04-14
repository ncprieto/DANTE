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

    void Start()
    {
        // sets groundDecel to 1 if its less than 1, groundDecel value less than 1 causes bugs
        groundDecel = groundDecel < 1 ? 1 : groundDecel;
    }

    bool isGrounded;
    void Update()
    {
        GetInputs();
        // check if player is on the ground
        isGrounded = Physics.Raycast(orientation.position, -orientation.up, 1f);
        if(isGrounded && justJumped && apexReached)
        {
            StartCoroutine(CheckBHopWindow());
            justJumped = false;
            apexReached = false;
        }
        CapSpeed();
        if(Input.GetKeyDown(jump)){ OnJumpPressed(); }
    }

    void FixedUpdate()
    {
        MovePlayer();
        if(isGrounded)
        {
            if(!GetInputs() && rb.velocity.magnitude > 0)
            {
                rb.velocity -= rb.velocity / groundDecel;
            }
        }
        else if(!isGrounded)
        {
            if(justJumped && rb.velocity.y < 0)
            {
                apexReached = true;
            }
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
        Vector3 velo = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(velo.magnitude > moveSpeed + (bHopCount * bHopMultiplier))
        {
            velo = velo.normalized * (moveSpeed + (bHopCount * bHopMultiplier));
            rb.velocity = new Vector3(velo.x, rb.velocity.y, velo.z);
        }
    }

    private Vector3 direction;
    void OnJumpPressed()
    {
        if(isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            justJumped = true;
            apexReached = false;
        }
    }

    private IEnumerator CheckBHopWindow()
    {
        float timer = bHopWindow;
        // while player is inside of the b hop window
        while(timer > 0)
        {
            // add to counter if they jump in the window
            if(Input.GetKeyDown(jump)){
                bHopCount += bHopCount < bHopMax ? 1 : 0;
                Debug.Log("COUNT " + bHopCount);
                yield break; // completely breaks out of the coroutine
            };
            timer -= Time.deltaTime;
            yield return null;
        }
        bHopCount = 0;
        yield break;
    }
}
