using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //transform.localPosition = new Vector3(x, y, -10f);
            transform.localPosition = new Vector3(x + originalPosition.x, y + originalPosition.y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = originalPosition;
    }
   
}
