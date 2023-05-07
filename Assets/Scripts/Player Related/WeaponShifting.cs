using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShifting : MonoBehaviour
{

    public Movement move;

    public float forwardShift;
    public float backwardShift;
    public float leftShift;
    public float rightShift;

    public float shiftTime;

    private Vector3 origPos;
    private bool newInput;

    private int prevLR;
    private int prevFB;

    // Start is called before the first frame update
    void Start()
    {
        prevLR = 0;
        prevFB = 0;
        origPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (prevLR != move.lr || prevFB != move.fb){
            newInput = true;
            // Shift center
            if (move.lr == 0 && move.fb == 0){
                StartCoroutine(ShiftOverTime(transform.localPosition, origPos, shiftTime));
            }
            // Shift right
            else if (move.lr == 1 && move.fb == 0){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x + rightShift, origPos.y, origPos.z), shiftTime));
            }
            // Shift left
            else if (move.lr == -1 && move.fb == 0){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x - leftShift, origPos.y, origPos.z), shiftTime));
            }
            // Shift forward
            else if (move.lr == 0 && move.fb == 1){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x, origPos.y, origPos.z + forwardShift), shiftTime));
            }
            // Shift backward
            else if (move.lr == 0 && move.fb == -1){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x, origPos.y, origPos.z - backwardShift), shiftTime));
            }
            // Shift forward-right
            else if (move.lr == 1 && move.fb == 1){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x + rightShift, origPos.y, origPos.z + forwardShift), shiftTime));
            }
            // Shift forward-left
            else if (move.lr == -1 && move.fb == 1){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x - leftShift, origPos.y, origPos.z + forwardShift), shiftTime));
            }
            // Shift backward-right
            else if (move.lr == 1 && move.fb == -1){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x + rightShift, origPos.y, origPos.z - backwardShift), shiftTime));
            }
            // Shift backward-left
            else if (move.lr == -1 && move.fb == -1){
                StartCoroutine(ShiftOverTime(transform.localPosition, new Vector3(origPos.x - leftShift, origPos.y, origPos.z - backwardShift), shiftTime));
            }
        }
        prevLR = move.lr;
        prevFB = move.fb;
    }

    IEnumerator ShiftOverTime(Vector3 startPos, Vector3 endPos, float shiftTime){
        newInput = false;
        float timeLeft = shiftTime;
        while (timeLeft > 0){
            if (newInput) break;
            timeLeft -= Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, endPos, (1 - Mathf.Clamp(timeLeft / shiftTime, 0f, 1f)));
            yield return null;
        }
        yield return null;
    }

}
