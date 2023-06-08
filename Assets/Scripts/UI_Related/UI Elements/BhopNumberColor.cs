using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BhopNumberColor : MonoBehaviour
{
    public TextMeshProUGUI numText;

    private Movement move;
    private int prevBhopCount;

    // Start is called before the first frame update
    void Start()
    {
        move = GameObject.Find("Player").GetComponent<Movement>();
        prevBhopCount = move.bHopCount;
    }

    // Update is called once per frame
    void Update()
    {
        numText.text = move.bHopCount.ToString() + "x";
        if (move.bHopCount != prevBhopCount){
            StartCoroutine(FadeTextToZeroAlpha(1f, numText));
        }
        prevBhopCount = move.bHopCount;
    }

    private IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
