using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponJump : MonoBehaviour
{

    public Movement move;

    public float jumpUpShift;
    public float jumpDownShift;

    public float landDownShift;

    public float jumpMoveRate;
    public float landMoveRate;
    public float decayFactor;
    public float errorPadding;
    private float origJumpRate;
    private float origLandRate;

    private Vector3 origPos;
    private Vector3 newPos;

    private bool didJump;
    private bool didLand;
    private bool newAction;
    private bool jumpDownSwayComplete;
    private bool landDownSwayComplete;

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.localPosition;
        origJumpRate = jumpMoveRate;
        origLandRate = landMoveRate;
        newPos = origPos;
        didJump = false;
        didLand = false;
        newAction = false;
        jumpDownSwayComplete = false;
        landDownSwayComplete = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Decay move rate
        jumpMoveRate *= decayFactor;
        landMoveRate *= decayFactor;

        // Check for new action
        if (newAction){
            newAction = false;
            jumpMoveRate = origJumpRate;
            landMoveRate = origLandRate;
            jumpDownSwayComplete = false;
            landDownSwayComplete = false;
        }

        // Jumping check
        if (move.justJumped && !didJump){
            didLand = false;
            didJump = true;
            newAction = true;
        }

         // Landing check
        if (move.isGrounded && move.wasInAir && !didLand){
            didJump = false;
            didLand = true;
            newAction = true;
        }

        // Jump gun movement execution
        if (didJump){
            if (!jumpDownSwayComplete){
                if (transform.localPosition.y > origPos.y - jumpDownShift){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y - jumpMoveRate, transform.localPosition.z); 
                }
                else{
                    jumpDownSwayComplete = true;
                }
            }
            else{
                if (transform.localPosition.y < origPos.y + jumpUpShift){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y + jumpMoveRate, transform.localPosition.z);
                }
            }
        }

        // Land gun movement execution
        if (didLand){
            if (!landDownSwayComplete){
                if (transform.localPosition.y > origPos.y - landDownShift){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y - landMoveRate, transform.localPosition.z); 
                }
                else{
                    landDownSwayComplete = true;
                }
            }
            else{
                if (transform.localPosition.y < origPos.y + errorPadding){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y + landMoveRate, transform.localPosition.z);
                }
            }
        }

        // Set new position for frame
        transform.localPosition = newPos;
    }
}
