using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class GunAttributes : MonoBehaviour
{
    [Header ("Base Variables")]
    public KeyCode shoot;
    public float fireRate;
    public float weaponRange;
    public LayerMask hitboxLayer;
    public GunDamage damageValues;

    [Header ("SFX Key and Events")]
    public string gunShotSFXKey;
    public FMOD.Studio.EventInstance gunShotSFXEvent;

    [Header ("Bullet Trail/Flash Variables")]
    public Transform trailOrigin;
    public float trailDuration;
    LineRenderer shotTrail;
    public GameObject muzzleFlash;
    public ParticleSystem vertGunSmoke;
    public ParticleSystem horizGunSmoke;

    [Header ("Gun Movement")]
    public GunMovement gunMovement;

    [Header ("UI Elements")]
    public GameObject NormalHitmarkerPrefab;
    public GameObject CritHitmarkerPrefab;
    protected GameObject UICanvas;
    protected GameObject NormalHitmarker;
    protected GameObject CritHitmarker;
    private Image NormalHitmarkImage;
    private Image CritHitmarkImage;

    private Animator fireAnim;
    private Animator hammerAnim;
    private float sinceLastFire = 0;
    private AntiStuck antiStuckScript;
    private Movement movement;
    
    void Awake(){
        shotTrail = GetComponent<LineRenderer>();
        fireAnim  = GetComponent<Animator>();
        hammerAnim = transform.Find("idlerevolver").GetComponent<Animator>();
        GameObject player = GameObject.Find("Player");
        movement = player.GetComponent<Movement>();
        SetUpUI();
        gunMovement.Initialize(player, this, GameObject.Find("SoundSystem"), UICanvas);
        antiStuckScript = GameObject.Find("AntiStuckCheck").GetComponent<AntiStuck>();
        shoot = (KeyCode)PlayerPrefs.GetInt("Shoot", 323);
        gunShotSFXEvent = RuntimeManager.CreateInstance(gunShotSFXKey);
    }

    // Update is called once per frame
    void Update()
    {
        sinceLastFire += Time.deltaTime;
        if (Input.GetKey(shoot) && (sinceLastFire > fireRate)){
            sinceLastFire = 0;
            PlayShootVFX();
            gunShotSFXEvent.start();
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, Mathf.Infinity, hitboxLayer)){
                shotTrail.SetPosition(1, hit.point);
                GameObject root   = hit.transform.parent.parent.gameObject;
                GameObject hitbox = hit.transform.parent.gameObject;
                Enemy enemyHit    = root.GetComponent<Enemy>();
                float damageToGive = damageValues.CalculateDamage(hit.distance, movement.bHopCount, hitbox.name);     // calculate damage that the enemy will take
                if(enemyHit.IsThisDamageLethal(damageToGive))                                                         // if this damage is lethal then update time on the UI
                {
                    float timeToAdd = root.GetComponent<Enemy>().GetTimeRewardValue(hitbox.name);
                    // UI.AddTime(timeToAdd);
                    if (antiStuckScript.enemiesNear > 0) antiStuckScript.enemiesNear--;
                }
                enemyHit.ReceiveDamage(damageToGive);                                                // actually apply damage to the enemy that was hit
                enemyHit.BloodParticles(hit.transform);
                gunMovement.ReceiveHitInfo(enemyHit.IsThisDamageLethal(damageToGive) ? "Lethal" : hitbox.name);
                DisplayHitmarker(hitbox.name);
            }
            else{
                gunMovement.ReceiveHitInfo(null);
                shotTrail.SetPosition(1, rayOrigin + (Camera.main.transform.forward * weaponRange));
            }
        }
    }

    void PlayShootVFX()
    {
        StartCoroutine(DrawTrail());
        StartCoroutine(FlashMuzzle());
        vertGunSmoke.Play();
        horizGunSmoke.Play();
        fireAnim.SetTrigger("FireWeapon");
        hammerAnim.SetTrigger("HammerPull");
        shotTrail.SetPosition(0, trailOrigin.position);
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
        yield return new WaitForSeconds(.03f);
        muzzleFlash.SetActive(false);
    }

    private void SetUpUI()
    {
        UICanvas = GameObject.Find("Canvas");
        NormalHitmarker = Instantiate(NormalHitmarkerPrefab, UICanvas.transform, false);
        CritHitmarker   = Instantiate(CritHitmarkerPrefab, UICanvas.transform, false);
        NormalHitmarkImage = NormalHitmarker.GetComponent<Image>();
        CritHitmarkImage   = CritHitmarker.GetComponent<Image>();
    }

    private void DisplayHitmarker(string tag)
    {
        Image hitmarker = tag == "CritHitbox" ? CritHitmarkImage : NormalHitmarkImage;
        StartCoroutine(FadeImageToZeroFrom(.3f, hitmarker, .5f));
    }

    private IEnumerator FadeImageToZeroFrom(float startAlpha, Image i, float t)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, startAlpha);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
