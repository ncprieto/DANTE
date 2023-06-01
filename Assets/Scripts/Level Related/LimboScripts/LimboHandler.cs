using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboHandler : MonoBehaviour
{

    public int currentLimboObj;
    public bool objChanged;

    [Header ("Game Objects")]
    public GameObject revolverObj;

    [Header ("UI Scripts")]
    public Movement movementScript;
    public TimeUpdater timeUpdaterScript;
    public BackgroundLoader bgLoaderScript;

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
                    bgLoaderScript.SetBackgroundByNameTo("TopLeft", true);
                    break;
                case 2:
                    transform.position = from2.position;
                    movementScript.EnableBHopUI();
                    objSetter.SetObjective("BHOP ACROSS GAP", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>That's a large gap, but with the divine power bestowed upon you,<br>it's possible to jump it. B-Hop chains give speed and damage.<br><br><b>Press Space when landing a jump to chain B-Hops.</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("TopRight", true);
                    break;
                case 3:
                    transform.position = from3_1.position;
                    movementScript.EnableGrappleUI();
                    movementScript.grappleEnabled = true;
                    objSetter.SetObjective("GRAPPLE ACROSS", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>Ah, so you've found your grapple, allowing you to cross across<br>great distances and swing great heights. Take note that its power<br>cannot be abused. It can only go so far, and every so often.<br><br><b>Right Click - Grapple / Cancel Grapple<br>Icon under reticle indicates if possible to grapple.</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("BottomLeft", true);
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
                    revolverObj.SetActive(true);
                    objSetter.SetObjective("DESTROY TARGETS", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>Your fated revolver... back in your hands once more.<br>You haven't forgotten how to use it, have you?<br><br><b>Left Click - Fire Weapon | Has infinite ammo.</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("BottomRight", true);
                    break;
                case 7:
                    transform.position = from4_2.position;
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>The holy ones have given you the ability to slow time<br>as well, ableit breifly and with absolute focus. Take care not<br>to miss a shot, lest that focus breaks.<br><br><b>Left Shift - Time-Slow Ability<br>Extends on hits, immediately ends on misses.</b>", this);
                    break;
                case 8:
                    transform.position = from5.position;
                    timeUpdaterScript.enabled = true;
                    objSetter.SetObjective("RACE TO THE EXIT", this);
                    virgilSetter.SetObjective("<b><i>Virgil</i></b><br>You're closer to the second ring of Hell then ever before.<br>The infernal magicks have made your time alive limited— make haste!<br><br><b>Collect Time-Rings and kill demons to extend your time alive.<br>Green rings give large amounts of time, blue rings give small.</b>", this);
                    bgLoaderScript.SetBackgroundByNameTo("TopCenter", true);
                    break;
                case 9:
                    transform.position = new Vector3(0, 0, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
