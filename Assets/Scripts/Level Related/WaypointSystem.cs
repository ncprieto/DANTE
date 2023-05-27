using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class WaypointSystem : MonoBehaviour
{

    public GameObject waypointParent;
    public Image img;
    public TextMeshProUGUI distanceText;
    public Transform target;
    public Vector3 offset;
    public float edgePadding;
    public TimeRingSpawns trScript;

    public float minFadeDist;
    public float maxFadeDist;

    private Vector2 screenCenter;
    private float distFromCenter;

    void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (trScript != null){
            if (trScript.isRingActive && !waypointParent.activeSelf){
                waypointParent.SetActive(true);
            }
            else if (!trScript.isRingActive && waypointParent.activeSelf){
                waypointParent.SetActive(false);
            }
        }

        if (target != null && waypointParent.activeSelf){
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

            targetPos.x = Mathf.Clamp(targetPos.x, minX + edgePadding, maxX - edgePadding);
            targetPos.y = Mathf.Clamp(targetPos.y, minY + edgePadding, maxY - edgePadding);

            img.transform.position = targetPos;

            distanceText.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + " m";

            distFromCenter = Vector2.Distance(screenCenter, img.transform.position);
            if (distFromCenter < minFadeDist){
                float newAlpha = Mathf.Clamp(math.remap(maxFadeDist, minFadeDist, 0.1f, 1f, distFromCenter), 0.1f, 1f);
                Color tempImgColor = img.color;
                tempImgColor.a = newAlpha;
                img.color = tempImgColor;
                Color tempTextColor = distanceText.color;
                tempTextColor.a = newAlpha;
                distanceText.color = tempTextColor;
            }
        }

    }
}
