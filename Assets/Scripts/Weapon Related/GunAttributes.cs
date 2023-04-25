using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttributes : MonoBehaviour
{
    [Header ("Base Variables")]
    public float fireRate;
    public float weaponRange;
    public LayerMask hitboxLayer;
    public GunDamage damageValues;
    //public float recoilStrength;
    //public float damageValue;

    [Header ("Bullet Trail Variables")]
    //public Transform mainCam;
    public Transform trailOrigin;
    public float trailDuration;
    LineRenderer shotTrail;

    [Header ("Gun Movement")]
    public GunMovement gunMovement;
    
    private Animator fireAnim;
    private float sinceLastFire = 0;

    void Awake(){
        shotTrail = GetComponent<LineRenderer>();
        fireAnim = GetComponent<Animator>();
        gunMovement.Initialize(GameObject.Find("Player"), this);
        //playerAim = GameObject.Find("Orientation").transform;
        //mainCam = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastFire += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && (sinceLastFire > fireRate)){
            sinceLastFire = 0;
            fireAnim.SetTrigger("FireWeapon");
            shotTrail.SetPosition(0, trailOrigin.position);
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, weaponRange, hitboxLayer)){
                shotTrail.SetPosition(1, hit.point);
                GameObject parent = hit.transform.parent.gameObject;

                // calculate damage based on which hit box was hit
                float damageToGive = damageValues.CalculateDamage(parent.name);
                hit.transform.parent.parent.GetComponent<Enemy>().ReceiveDamage(damageToGive);
                gunMovement.ReceiveHitInfo(parent.name);
            }
            else{
                shotTrail.SetPosition(1, rayOrigin + (Camera.main.transform.forward * weaponRange));
            }
            StartCoroutine(DrawTrail());
            // Quaternion recoilRotation = Camera.main.transform.localRotation;
            // recoilRotation.x -= recoilStrength;
            // recoilRotation.y += recoilStrength;
            // Camera.main.transform.localRotation = recoilRotation;
            //Camera.main.transform.localRotation = Quaternion.Euler(Camera.main.transform.localRotation.x - recoilStrength, Camera.main.transform.localRotation.y + recoilStrength, Camera.main.transform.localRotation.z);
        }
    }

    IEnumerator DrawTrail()
    {
        shotTrail.enabled = true;
        yield return new WaitForSeconds(trailDuration);
        shotTrail.enabled = false;
    }
}
