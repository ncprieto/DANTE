using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboOverlays : MonoBehaviour
{

    public Material cMat;
    public Material rMat;

    public bool runCompleteMat;
    public bool runRestartMat;

    // Start is called before the first frame update
    void Start()
    {
        cMat.color = new Color(cMat.color.r, cMat.color.g, cMat.color.b, 0);
        rMat.color = new Color(rMat.color.r, rMat.color.g, rMat.color.b, 0);
        runCompleteMat = false;
        runRestartMat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (runCompleteMat){
            runCompleteMat = false;
            StartCoroutine(FadeMatToZeroAlpha(1f, cMat));
        }
        if (runRestartMat){
            runRestartMat = false;
            StartCoroutine(FadeMatToZeroAlpha(1f, rMat));
        }
    }

    void OnDisable(){
        cMat.color = new Color(cMat.color.r, cMat.color.g, cMat.color.b, 0);
        rMat.color = new Color(rMat.color.r, rMat.color.g, rMat.color.b, 0);
    }

    IEnumerator FadeMatToZeroAlpha(float t, Material i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
