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

    [Header ("Bullet Trail/Flash Variables")]
    //public Transform mainCam;
    public Transform trailOrigin;
    public float trailDuration;
    LineRenderer shotTrail;
    public GameObject muzzleFlash;

    [Header ("Gun Movement")]
    public GunMovement gunMovement;
    
    private Animator fireAnim;
    private float sinceLastFire = 0;
    private UI_Script UI;

    void Awake(){
        shotTrail = GetComponent<LineRenderer>();
        fireAnim = GetComponent<Animator>();
        gunMovement.Initialize(GameObject.Find("Player"), this);
        UI = GameObject.Find("Canvas").GetComponent<UI_Script>();

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

                // get correct gameobjects and enemy base script
                GameObject root    = hit.transform.parent.parent.gameObject;
                GameObject hitbox  = hit.transform.parent.gameObject;
                Enemy enemyHit = root.GetComponent<Enemy>();
                // calculate damage and if necessary update time UI
                float damageToGive = damageValues.CalculateDamage(hitbox.name);                      // calculate damage that the enemy will take
                if(enemyHit.IsThisDamageLethal(damageToGive))                                        // if this damage is lethal then update time on the UI
                {
                    float timeToAdd = root.GetComponent<Enemy>().GetTimeRewardValue(hitbox.name);
                    UI.AddTime(timeToAdd);
                }
                enemyHit.ReceiveDamage(damageToGive);                                                // actually apply damage to the enemy that was hit
                enemyHit.BloodParticles(hit.transform);
                gunMovement.ReceiveHitInfo(hitbox.name);
            }
            else{
                gunMovement.ReceiveHitInfo(null);
                shotTrail.SetPosition(1, rayOrigin + (Camera.main.transform.forward * weaponRange));
            }
            StartCoroutine(DrawTrail());
            StartCoroutine(FlashMuzzle());
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

    IEnumerator FlashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }
}
