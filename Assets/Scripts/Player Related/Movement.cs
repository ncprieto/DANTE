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
    public float acceleration;
    public float deceleration;            
    public float groundDrag;
    public float jumpHeight;
    private bool justJumped;
    private bool apexReached;

    [Header("B Hop Variables")]
    public float bHopMax;
    public float currentBHop;
    public float bHopStep;
    public float bHopWindow;

    void Start()
    {
        // sets deceleration to 1 if its less than 1, deceleration value less than 1 causes bugs
        deceleration = deceleration < 1 ? 1 : deceleration;
    }

    bool isGrounded;
    void Update()
    {
        // check if player is on the ground
        RaycastHit hit;
        isGrounded = Physics.SphereCast(orientation.position, 0.001f, -orientation.up, out hit, 1f, ground);
        if(isGrounded && justJumped && apexReached)
        {
            StartCoroutine(CheckBHopWindow());
            justJumped = false;
            apexReached = false;
        }
        if(Input.GetKeyDown(jump)){ OnJumpPressed(); }
    }

    void FixedUpdate()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
            if(GetInputs())
            {
                Vector3 dir = GetWishDirection();
                rb.velocity += dir * acceleration;
            }
            else
            {
                // slow player down if they aren't inputting anything if
                // their currently velocty is > 0 ie player was just moving
                if(rb.velocity.magnitude > 0)
                {
                    rb.velocity -=  rb.velocity / deceleration;
                }
            }
        }
        else if(!isGrounded)
        {
            rb.drag = 0;
            if(!apexReached && rb.velocity.y < 0){ apexReached = true; }
        }
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

    void OnJumpPressed()
    {
        if(isGrounded)
        {
            if(GetInputs())
            {
                rb.velocity -= rb.velocity;
                rb.velocity = GetWishDirection() * currentBHop + transform.up * jumpHeight;
            }
            else
            {
                // bugged
                rb.AddForce(rb.velocity * currentBHop + transform.up * jumpHeight, ForceMode.Impulse);
            }
            justJumped = true;
            apexReached = false;
        }
    }

    private IEnumerator CheckBHopWindow()
    {
        float timer = bHopWindow;
        float original = currentBHop;
        // while player is inside of the b hop window
        while(timer > 0)
        {
            // if player pressed jump while in the b hop window then up
            // a multiplier that affects their horizontal distance
            if(Input.GetKeyDown(jump)){
                if(currentBHop < bHopMax){ currentBHop += bHopStep; }
                yield break; // completely breaks out of the coroutine
            };
            timer -= Time.deltaTime;
            yield return null;
        }
        currentBHop = 15f;
        yield break;
    }
}
