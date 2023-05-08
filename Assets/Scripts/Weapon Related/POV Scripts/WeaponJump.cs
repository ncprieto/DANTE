using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponJump : MonoBehaviour
{

    public Movement move;

    public float jumpUpShift;
    public float jumpDownShift;

    public float landUpShift;
    public float landDownShift;

    public float moveRate;
    public float decayFactor;
    public float errorPadding;
    private float origRate;

    private Vector3 origPos;
    private Vector3 newPos;

    private bool didJump;
    private bool didLand;

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.localPosition;
        origRate = moveRate;
        newPos = origPos;
        // didJump = false;
        // didLand = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Jumping code
        // if (move.justJumped && !didJump){
        //     didLand = false;
        //     didJump = true;

        // }

        // // Landing code
        // if (move.IsGrounded && move.wasInAir && !didLand){
        //     didJump = false;
        //     didLand = true;

        // }
    }
}
