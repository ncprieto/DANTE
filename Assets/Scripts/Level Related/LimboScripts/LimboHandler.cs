using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LimboHandler : MonoBehaviour
{

    public int currentLimboObj;
    public bool objChanged;
    public int targetsDestroyed;
    public bool restartObj5;

    [Header ("Game Objects")]
    public GameObject player;
    public GameObject revolverObj;
    public GameObject portalLock4_1;
    public GameObject portalLock4_2;
    public GameObject portalLock5;
    public GameObject smallTimeRing;
    public GameObject largeTimeRing;

    [Header ("Transforms")]
    public Transform rsPoint5;
    public Transform smallRingAnchor;
    public Transform largeRingAnchor;

    [Header ("UI Scripts")]
    public Movement movementScript;
    public TimeUpdater timeUpdaterScript;
    public BackgroundLoader bgLoaderScript;
    public LimboOverlays limboOverlays;

    [Header ("Scriptable Objects")]
    public ObjectiveSetter objSetter;
    public ObjectiveSetter virgilSetter;

    [Header ("Waypoint Positions")]
    public Transform from1;
    public Transform from2;
    public Transform from3_1;
    public Transform from3_2;
    public Transform from3_3;
    public Transform from4_1;
    public Transform from4_2;
    public Transform from5;

    void Start()
    {
        currentLimboObj = 1;
        objChanged = true;
        targetsDestroyed = 0;
        restartObj5 = false;
        movementScript.DisableGrappleUI();
        movementScript.DisableBHopUI();
        objSetter.SetController(this, objSetter);
        virgilSetter.SetController(this, virgilSetter);
        bgLoaderScript.SetAllBackgroundsTo(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (objChanged){
            objChanged = false;
            movementScript.StopGrapple();
            switch (currentLimboObj){
                case 1:
                    transform.position = from1.position;
                    objSetter.SetObjective("PICK UP HEALTH", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>Get up, Dante. You've got demons to slay... but you're hurt<br>from entering Hell. Pick up that health and get to the portal.<br><br><b>WASD - Move | Space - Jump</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("LimboTopLeft", true);
                    break;
                case 2:
                    transform.position = from2.position;
                    movementScript.EnableBHopUI();
                    objSetter.SetObjective("B-HOP ACROSS GAP", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>That's a large gap, but with the divine power bestowed upon you,<br>it's possible to jump it. B-Hop chains give speed and damage.<br><br><b>Press Space when landing a jump to chain B-Hops.</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("LimboTopRight", true);
                    break;
                case 3:
                    transform.position = from3_1.position;
                    movementScript.EnableGrappleUI();
                    movementScript.grappleEnabled = true;
                    objSetter.SetObjective("GRAPPLE ACROSS", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>Ah, so you've found your grapple, allowing you to cross across<br>great distances and swing great heights. Take note that its power<br>cannot be abused. It can only go so far, and every so often.<br><br><b>Right Click - Grapple / Cancel Grapple<br>Icon under reticle indicates if possible to grapple.</b>", this);
                    break;
                case 4:
                    transform.position = from3_2.position;
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>These glowing constructs are gifts to you, imbuing your grapple<br>with the power to being used again immediately after use.<br>Remember to swing by looking where you want to move to.<br><br><b>When grappled onto, grapple points refund your grapple<br>cooldown, but have a respawn time between uses.</b>", this);
                    break;
                case 5:
                    transform.position = from3_3.position;
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>Here's a challenge. But I know you can do it, Dante.<br>Cross this gap and you'll find your weapon.", this);
                    break;
                case 6:
                    transform.position = from4_1.position;
                    targetsDestroyed = 0;
                    revolverObj.SetActive(true);
                    objSetter.SetObjective("DESTROY TARGETS", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>Your fated revolver... back in your hands once more.<br>You haven't forgotten how to use it, have you?<br>Destroy these targets to open up the portal.<br><br><b>Left Click - Fire Weapon | Has infinite ammo.</b>", this);
                    break;
                case 7:
                    transform.position = from4_2.position;
                    targetsDestroyed = 0;
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>The holy ones have given you the ability to slow time<br>as well, ableit breifly and with absolute focus. Take care not<br>to miss a shot, lest that focus breaks.<br><br><b>Left Shift - Time-Slow Ability / Cancel Ability<br>Extends on hits, immediately ends on misses.</b>", this);
                    break;
                case 8:
                    transform.position = from5.position;
                    targetsDestroyed = 0;
                    Instantiate(smallTimeRing, smallRingAnchor.position, Quaternion.identity, smallRingAnchor);
                    Instantiate(largeTimeRing, largeRingAnchor.position, Quaternion.identity, largeRingAnchor);
                    timeUpdaterScript.enabled = true;
                    objSetter.SetObjective("RACE TO THE EXIT", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>You're closer to the second ring of Hell then ever before.<br>The infernal magicks have made your time alive limited— make haste!<br><br><b>Collect Time-Rings and kill demons to extend your time alive.<br>Green rings give large amounts, blue rings give small.</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("LimboTopCenter", true);
                    break;
                case 9:
                    SceneManager.LoadScene("MainMenu");
                    transform.position = new Vector3(0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        if (currentLimboObj == 6){
            if (targetsDestroyed == 8){
                portalLock4_1.SetActive(false);
            }
        }
        if (currentLimboObj == 7){
            if (targetsDestroyed == 3){
                portalLock4_2.SetActive(false);
            }
        }
        if (currentLimboObj == 8){
            if (targetsDestroyed == 3){
                portalLock5.SetActive(false);
            }
            if (timeUpdaterScript.timeLeft <= 0f){
                limboOverlays.runRestartMat = true;
                restartObj5 = true;
            }
        }

        if (restartObj5){
            restartObj5 = false;
            timeUpdaterScript.timeLeft = timeUpdaterScript.startingTime;
            player.transform.position = rsPoint5.position;
            if (smallRingAnchor.childCount == 0){
                Instantiate(smallTimeRing, smallRingAnchor.position, Quaternion.identity, smallRingAnchor);
            }
            if (largeRingAnchor.childCount == 0){
                Instantiate(largeTimeRing, largeRingAnchor.position, Quaternion.identity, largeRingAnchor);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
    }
}
