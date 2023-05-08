using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShiftingNew : MonoBehaviour
{

    public Movement move;

    public float forwardShift;
    public float backwardShift;
    public float leftShift;
    public float rightShift;

    public float moveRate;
    public float decayFactor;
    public float errorPadding;
    private float origRate;

    private Vector3 origPos;
    private Vector3 newPos;

    private int prevLR;
    private int prevFB;

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.localPosition;
        origRate = moveRate;
        newPos = origPos;
        prevLR = 0;
        prevFB = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Decay move rate
        moveRate *= decayFactor;

        // Check for new inputs
        if (prevLR != move.lr || prevFB != move.fb){
            moveRate = origRate;
        }

        // Shift center
        if (move.lr == 0 && move.fb == 0){
            // Right to center
            if (transform.localPosition.x > origPos.x + errorPadding){
                // Forward-right to center
                if (transform.localPosition.z > origPos.z + errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                // Backward-right to center
                else if (transform.localPosition.z < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
                // Just right
                else{
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z);
                }
            }
            // Left to center
            else if (transform.localPosition.x < origPos.x - errorPadding){
                // Forward-left to center
                if (transform.localPosition.z > origPos.z + errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                // Backward-left to center
                else if (transform.localPosition.z < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
                // Just left
                else{
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z);
                }
            }
            // Forward to center
            else if (transform.localPosition.z > origPos.z + errorPadding){
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveRate);
            }
            // Backwards to center
            else if (transform.localPosition.z < origPos.z - errorPadding){
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveRate);
            }
        }

        // Shift right
        else if (move.lr == 1 && move.fb == 0){
            if (transform.localPosition.x < origPos.x + rightShift){
                // Correct to center if needed
                if (transform.localPosition.z > origPos.z + errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                else if (transform.localPosition.z < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
                else{
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z);
                }
            }
            else{
                // Correct to center if needed
                if (transform.localPosition.z > origPos.z + errorPadding){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                else if (transform.localPosition.z < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
            }
        }
        // Shift left
        else if (move.lr == -1 && move.fb == 0){
            if (transform.localPosition.x > origPos.x - leftShift){
                // Correct to center if needed
                if (transform.localPosition.z > origPos.z + errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                else if (transform.localPosition.z < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
                else{
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z);
                }
            }
            else{
                // Correct to center if needed
                if (transform.localPosition.z > origPos.z + errorPadding){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                else if (transform.localPosition.z < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
            }
        }
        // Shift forward
        else if (move.lr == 0 && move.fb == 1){
            if (transform.localPosition.z < origPos.z + forwardShift){
                // Correct to center if needed
                if (transform.localPosition.x > origPos.x + errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
                else if (transform.localPosition.x < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
                else{
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveRate);
                }
            }
            else{
                // Correct to center if needed
                if (transform.localPosition.x > origPos.x + errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z);
                }
                else if (transform.localPosition.x < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z);
                }
            }
        }
        // Shift backward
        else if (move.lr == 0 && move.fb == -1){
            if (transform.localPosition.z > origPos.z - backwardShift){
                // Correct to center if needed
                if (transform.localPosition.x > origPos.x + errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                else if (transform.localPosition.x < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
                else{
                    newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveRate);
                }
            }
            else{
                // Correct to center if needed
                if (transform.localPosition.x > origPos.x + errorPadding){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z);
                }
                else if (transform.localPosition.x < origPos.z - errorPadding){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z);
                }
            }
        }

        // Shift forward-right
        else if (move.lr == 1 && move.fb == 1){
            if (transform.localPosition.x < origPos.x + rightShift){
                if (transform.localPosition.z < origPos.z + forwardShift){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z + moveRate); 
                }
                else{
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z); 
                }
            }
            else if (transform.localPosition.z < origPos.z + forwardShift){
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveRate); 
            }
        }
        // Shift forward-left
        else if (move.lr == -1 && move.fb == 1){
            if (transform.localPosition.x > origPos.x - leftShift){
                if (transform.localPosition.z < origPos.z + forwardShift){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z + moveRate); 
                }
                else{
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z); 
                }
            }
            else if (transform.localPosition.z < origPos.z + forwardShift){
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveRate); 
            }
        }
        // Shift backward-right
        else if (move.lr == 1 && move.fb == -1){
            if (transform.localPosition.x < origPos.x + rightShift){
                if (transform.localPosition.z > origPos.z - backwardShift){
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z - moveRate); 
                }
                else{
                    newPos = new Vector3(transform.localPosition.x + moveRate, transform.localPosition.y, transform.localPosition.z); 
                }
            }
            else if (transform.localPosition.z > origPos.z - backwardShift){
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveRate); 
            }
        }
        // Shift backward-left
        else if (move.lr == -1 && move.fb == -1){
            if (transform.localPosition.x > origPos.x - leftShift){
                if (transform.localPosition.z > origPos.z - backwardShift){
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z - moveRate); 
                }
                else{
                    newPos = new Vector3(transform.localPosition.x - moveRate, transform.localPosition.y, transform.localPosition.z); 
                }
            }
            else if (transform.localPosition.z > origPos.z - backwardShift){
                newPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveRate); 
            }
        }
        
        // Set new position for frame
        transform.localPosition = newPos;

        // New inputs
        prevLR = move.lr;
        prevFB = move.fb;
    }
}
