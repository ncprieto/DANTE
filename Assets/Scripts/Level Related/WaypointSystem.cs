using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaypointSystem : MonoBehaviour
{

    public Image img;
    public TextMeshProUGUI distanceText;
    public Transform target;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (target != null){
            // Constrain waypoint sprite to screen
            float minX = img.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = img.GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minY;

            Vector2 targetPos = Camera.main.WorldToScreenPoint(target.position + offset);

            // Check if waypoint target is behind camera
            if (Vector3.Dot((target.position - transform.position), transform.forward) < 0){
                if (targetPos.x < Screen.width / 2){
                    targetPos.x = maxX;
                }
                else{
                    targetPos.x = minX;
                }
            }

            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

            img.transform.position = targetPos;

            distanceText.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m"; 
        }
    }
}
