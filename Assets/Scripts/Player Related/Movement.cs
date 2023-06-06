using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform orientation;
    public LayerMask ground;
    public FOVVFX fovVFX;
    public GameObject playerCamera;

    [Header("Input")]
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode grapple;
    public int fb;
    public int lr;

    [Header("Ground Movement Variables")]
    public float moveSpeed;
    public float groundAccel;
    public float groundDecel;
    public float airAccel;
    public float airDecel;
    public float jumpHeight;
    public bool isGrounded;
    public bool wasInAir;

    [Header("Coyote Time Variables")]
    public bool justJumped;
    public bool canCoyote;
    public float coyoteWindow;
    public float coyoteJumpBoost;

    [Header("B Hop Variables")]
    public int bHopMax;
    public float bHopMultiplier;
    public float bHopWindow;
    public int bHopCount;

    [Header("Grapple Variables")]
    public float grappleRange;
    public float grappleCooldown;
    public float grapplePointSpeed;
    public float grapplePointDismountBoost;
    public bool  grappleEnabled;
    
    [Header ("Other Grapple Variables")]
    public Transform grappleStart;
    public LineRenderer lineRen;
    public bool  toggleControl;
    public bool  canGrapple;

    [Header("Grapple Start Variables")]
    public float grappleAccelerateTime;
    public float grappleAccelMaxSpeed;

    [Header("Grapple End Variables")]
    public float grappleDecelerateTime;
    public float grappleDecelMaxSpeed;
    public float grappleEndedTime;
    public float grappleEndedDecel;
    public float dismountBoost;

    [Header("Grapple UI Elements")]
    public  GameObject UICanvas;
    public  GameObject CooldownPrefab;
    public  GameObject CanGrapplePrefab;
    private GameObject CooldownUI;
    private GameObject CanGrappleUI;
    private NewCooldownUpdater CooldownUpdater;

    [Header("B-Hop UI Elements")]
    public GameObject bHopPrefab;
    private GameObject bHopUI;
    private TextMeshProUGUI bHopChainText;

    [Header("SFX Keys")]
    public string jumpSFXPath;
    public string bHopSFXPath;
    private string currentJumpSFX;

    void Start()
    {
        groundDecel = groundDecel < 1 ? 1 : groundDecel;                                                      // sets groundDecel to 1 if its less than 1, groundDecel value less than 1 causes bugs
        baseMoveSpeed = moveSpeed;
        SetUpControls();
        SetUpUI();
        currentJumpSFX = jumpSFXPath;
        
    }

    void Update()
    {
        GetInputs();
        isGrounded = Physics.Raycast(orientation.position, -orientation.up, 1.0001f, ground);                 // check if player is on the ground
        if(isGrounded  && wasInAir) StartBHopCoroutine();
        if(!isGrounded && wasInAir && !justJumped && !justGrappled && canCoyote) StartCoyoteTime();
        wasInAir = rb.velocity.y < 0;
        CapSpeed();
        if(Input.GetKeyDown(jump))    OnJumpPressed();
        if(Input.GetKeyDown(grapple)) OnGrapplePressed();
        if(Input.GetKeyUp(grapple) && !toggleControl) OnGrappleReleased();
        canGrapple = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, grappleRange);
        if(!grappleOnCooldown && grappleEnabled) CanGrappleUI.SetActive(canGrapple);
        if(grappleOnCooldown) CanGrappleUI.SetActive(false);

        // dev tools
        if(Input.GetKeyDown(KeyCode.Alpha0)) grappleCooldown = 0f; // no grapple cooldown
        if(Input.GetKeyDown(KeyCode.Equals))                       // add to bHop count
        {
            bHopOverride = true;
            bHopCount += bHopCount < bHopMax ? 1 : 0;
        }

        bHopChainText.text = bHopCount + "x b-Hop Chain";
    }

    void FixedUpdate()
    {
        if(grappling) DoActiveGrapple();
        else MovePlayer();
        CapSpeed();
        ApplyDeceleration();
    }
    
    public bool GetInputs()
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

    void ApplyDeceleration()
    {
        float decel = isGrounded ? groundDecel : airDecel;
        decel = grappleJustEnded ? grappleEndedDecel : decel;
        if(!GetInputs() && rb.velocity.magnitude > 0) rb.velocity -= rb.velocity / decel;
    }

    void OnJumpPressed()
    {
        if(isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
            justJumped = true;
            FMODUnity.RuntimeManager.PlayOneShot(currentJumpSFX);
        }
    }

    public void EnableGrapple()
    {
        grappleEnabled = true;
    }

    public void DisableGrapple()
    {
        grappleEnabled = false;
    }

    // OnGrapplePressed() executes functions relating to grapple functionality.
    bool grappling;
    bool grappleOnCooldown;
    bool hitGrapplePoint;
    bool justGrappled;
    Vector3 grapplePoint;
    GameObject hitGrappleObject;
    void OnGrapplePressed()
    {
        if(!grappleEnabled) return;
        if(!grappling && !grappleOnCooldown)
        {
            RaycastHit hit;
            if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grappleRange))
            {
                hitGrapplePoint = false;
                if(hit.transform.tag == "GrapplePoint")
                {
                    hitGrapplePoint = true;
                    hitGrappleObject = hit.transform.parent.parent.gameObject;
                }
                grapplePoint = hit.point;
                ToggleGrapple();
            }
        }
        else if(grappling && toggleControl) DoGrappleDismount();
    }

    // OnGrappleReleased() ends the player's grapple if toggleControl is set to false.
    void OnGrappleReleased()
    {
        if(grappling) DoGrappleDismount();
    }

    /* ToggleGrapple() turns the grappling boolean and the line renderer on
     * and off. It also resets the vertices of the line renderer if the player
     * is grappling.
     */
    void ToggleGrapple()
    {
        grappling = !grappling;
        lineRen.enabled = !lineRen.enabled;
        justGrappled = !justGrappled;
        if(grappling) // reset line renderer points, start FOV VFX, and lerp speed
        {
            lineRen.SetPosition(0, grapplePoint);
            lineRen.SetPosition(1, grapplePoint);
            fovVFX.GrappleStartVFX();
            LimitGrappleSpeed(grappleAccelMaxSpeed, baseMoveSpeed, grappleAccelerateTime);
        }
        else if(!grappling) // start FOV VFX, lerp speed, and start cooldown
        {
            fovVFX.GrappleEndVFX();
            LimitGrappleSpeed(baseMoveSpeed, grappleDecelMaxSpeed, grappleDecelerateTime);
            if(!hitGrapplePoint) StartCoroutine(StartGrappleCooldown());
            else hitGrappleObject.GetComponent<GrapplePointRespawn>().despawn = true;
            StartCoroutine(GrappleJustEnded());
            hitGrapplePoint = false;

        }
    }

    private void DoActiveGrapple()
    {
        float speed = baseMoveSpeed > moveSpeed ? baseMoveSpeed : moveSpeed;
        speed += hitGrapplePoint ? grapplePointSpeed : 0f;
        rb.velocity = GrappleSwingVector() * speed;                                    // set velocity towards grapple point with some speed multiplier
        lineRen.SetPosition(0, grappleStart.position);                                 // set vertex 0 of line renderer to player position
        float dist = hitGrapplePoint ? 5f : 1.2f;
        if(Vector3.Distance(grapplePoint, orientation.position) <= dist) StopGrapple();          // if player gets close to the grapple point disconnect them
        else
        {
            float angle = Vector3.Angle(VectorToGrapplePoint(), playerCamera.transform.forward); // if look vector of player and vector to the grapple point it
            if(angle < -89 || angle > 89) DoGrappleDismount();                                   // greater than 89 or -89 degress disconnect them
        }
    }

    /* VectorToGrapplePoint() returns the normalized vector from the player to
     * the grapplePoint.
     */
    private Vector3 VectorToGrapplePoint()
    {
        Vector3 dir = grapplePoint - orientation.position;
        return dir.normalized;
    }

    private Vector3 GrappleSwingVector()
    {
        Vector3 dirWithInput = VectorToGrapplePoint() + GetWishDirection();
        Vector3 dirWithLook  = Vector3.Slerp(dirWithInput, playerCamera.transform.forward.normalized, 0.5f);
        return dirWithLook.normalized;
    }

    private void DoGrappleDismount()
    {
        Vector3 boost = Vector3.up * dismountBoost;
        boost *= hitGrapplePoint ? grapplePointDismountBoost : 1f;
        rb.velocity = playerCamera.transform.forward * moveSpeed + boost;
        StopGrapple();
    }

    /* StopGrapple() will stop the grapple which applies a small
     * boost of speed as well. It also starts a coroutine that
     * decelerates the player's velocity.
     */
    public void StopGrapple()
    {
        if(grappling) ToggleGrapple();
    }

    bool grappleJustEnded;
    private IEnumerator GrappleJustEnded()
    {
        grappleJustEnded = true;
        yield return new WaitForSeconds(grappleEndedTime);
        grappleJustEnded = false;
    }

    private IEnumerator StartGrappleCooldown()
    {
        grappleOnCooldown = true;
        float timeLeft = grappleCooldown;
        CooldownUpdater.icon.SetActive(false);
        CooldownUpdater.transIcon.SetActive(true);
        while(timeLeft > 0f)
        {
            CooldownUpdater.UpdateCooldown(timeLeft, grappleCooldown);
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        grappleOnCooldown = false;
        CooldownUpdater.transIcon.SetActive(false);
        CooldownUpdater.icon.SetActive(true);
        CooldownUpdater.SetCooldownToReady();
    }

    /* LimitGrappleSpeed() keeps track if the grappleLerpCoroutine is running. If
     * so it stops it then, sets the player's movement speed back to its original value
     * then restarts the coroutine.
     */
    IEnumerator grappleLerpCoroutine;
    private void LimitGrappleSpeed(float start, float end, float timeFrame)
    {
        if(grappleLerpCoroutine != null) StopCoroutine(grappleLerpCoroutine);
        grappleLerpCoroutine = LerpGrappleSpeed(start, end, timeFrame);
        StartCoroutine(grappleLerpCoroutine);
    }

    /* LerpGrappleSpeed() will decelerate/accelerate the speed of the player
     * when the grapple is actve and when they dismount. Essentially linerally
     * interpolates up or down between values to limit the amount of 
     * speed the player retains when/after grappling.
     */
    float baseMoveSpeed;
    private IEnumerator LerpGrappleSpeed(float start, float end, float window)
    {
        float timeLeft = window;
        while(timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            moveSpeed = Mathf.Lerp(start, end, Mathf.Clamp(timeLeft / window, 0f, 1f));  // get new interpolated move speed
            yield return null;
        }
    }

    bool coyoteTimeRunning;
    IEnumerator coyoteTimeCoroutine;
    private void StartCoyoteTime()
    {
        if(coyoteTimeRunning || !canCoyote) return;
        coyoteTimeCoroutine = CoyoteTimeWindow();
        StartCoroutine(coyoteTimeCoroutine);
    }

    private IEnumerator CoyoteTimeWindow()
    {
        canCoyote = false;
        coyoteTimeRunning = true;
        float timer = coyoteWindow;
        while(timer > 0)
        {
            if(Input.GetKeyDown(jump))
            {
                rb.AddForce(transform.up * jumpHeight * coyoteJumpBoost, ForceMode.Impulse);
                coyoteTimeRunning = false;
                FMODUnity.RuntimeManager.PlayOneShot(bHopSFXPath);                             // play sfx
                bHopCount += bHopCount < bHopMax ? 1 : 0;
                yield break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        coyoteTimeRunning = false;
    }

    /* StartBHopCoroutine() will stop the bHopCoroutine is it's running
     * and start if it isn't.
     */
    IEnumerator bHopCoroutine;
    private void StartBHopCoroutine()
    {
        if(bHopCoroutine != null) StopCoroutine(bHopCoroutine);
        bHopCoroutine = CheckBHopWindow();
        StartCoroutine(bHopCoroutine);
    }

    /* CheckBHopWindow() determines if the player is inside the b hop window 
     * and is triggered when the player lands on the ground after jumping.
     * If the player jumps while inside the window, bHopCount is incremented
     * and gives the player a small speed increase based on the amount of successfully
     * chained b hops.
     */
    private bool bHopOverride;
    private IEnumerator CheckBHopWindow()
    {
        justJumped = false;
        canCoyote = true;
        float timer = bHopWindow;
        while(timer > 0)
        {
            if(Input.GetKeyUp(jump)){
                bHopCount += bHopCount < bHopMax ? 1 : 0;
                currentJumpSFX = bHopSFXPath;
                yield break;
            };
            timer -= Time.deltaTime;
            yield return null;
        }
        if(!bHopOverride) bHopCount = 0;
        currentJumpSFX = jumpSFXPath;
    }

    private void SetUpUI()
    {
        CooldownUI   = Instantiate(CooldownPrefab,   UICanvas.transform, false);
        CanGrappleUI = Instantiate(CanGrapplePrefab, UICanvas.transform, false);
        bHopUI       = Instantiate(bHopPrefab,       UICanvas.transform, false);
        CooldownUpdater = CooldownUI.GetComponent<NewCooldownUpdater>();
        CooldownUpdater.SetSliderAndNumber(grappleCooldown);
        CooldownUpdater.SetCooldownToReady();
        bHopChainText = bHopUI.GetComponent<TextMeshProUGUI>();
    }

    public void EnableGrappleUI()
    {
        CooldownUI.SetActive(true);
    }

    public void DisableGrappleUI()
    {
        CooldownUI.SetActive(false);
    }

    public void EnableBHopUI()
    {
        bHopUI.SetActive(true);
    }

    public void DisableBHopUI()
    {
        bHopUI.SetActive(false);
    }

    private void SetUpControls()
    {
        forward  = (KeyCode)PlayerPrefs.GetInt("Forward", 119);
        backward = (KeyCode)PlayerPrefs.GetInt("Backward", 115);
        left     = (KeyCode)PlayerPrefs.GetInt("Left", 97);
        right    = (KeyCode)PlayerPrefs.GetInt("Right", 100);
        jump     = (KeyCode)PlayerPrefs.GetInt("Jump", 32);
        grapple  = (KeyCode)PlayerPrefs.GetInt("Grapple", 324);
    }
}
