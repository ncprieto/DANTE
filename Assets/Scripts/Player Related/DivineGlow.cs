using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineGlow : MonoBehaviour
{

    public float timer;

    private Material divMat;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        divMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        Vector3 locPos = new Vector3(transform.localPosition.x, transform.localPosition.y - .1f, transform.localPosition.z);
        transform.localPosition = locPos;
        if (timer > .25f){
            Color newAlpha = divMat.color;
            newAlpha.a -= 0.0035f;
            newAlpha.a = Mathf.Clamp(newAlpha.a, 0f, 1f);
            divMat.color = newAlpha;
            Vector3 locScale = new Vector3(transform.localScale.x * 1.05f, transform.localScale.y * 1.05f, transform.localScale.z * 1.05f);
            transform.localScale = locScale;
        }
        if (timer > 1.5f){
            Destroy(this.gameObject);
        }
    }
}
