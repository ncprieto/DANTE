using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionHandler : MonoBehaviour
{
    public KeyCode escape;

    private UnityEngine.Rendering.VolumeProfile globalVolumeProfile;
    private UnityEngine.Rendering.VolumeProfile localVolumeProfile;
    private UnityEngine.Rendering.Universal.Vignette globalVignette;
    private UnityEngine.Rendering.Universal.Vignette localVignette;

    [Header ("Dependencies")]
    public LevelHandler lvlHandler;
    public PlayerHealth playerHealth;
    // public UI_Script ui;

    [Header ("FadeIn/Out Variables")]
    public float fadeToOverlayTime;

    [Header ("PlayerControlsSuspend")]
    public GameObject player;
    public GameObject mainCamera;
    public GameObject weaponShift;
    public GameObject weaponBob;
    public GameObject revolver;

    [Header("StatsOverlay")]
    public GameObject statsOverlay;
    public LevelHandler levelHandler;
    private float timePassed;
    private float currTime;

    void Start()
    {
        currTime = 0f;
        timePassed = 0f;
        mainCamera.GetComponent<Animator>().enabled = false;

        globalVolumeProfile = GameObject.Find("Global Volume").GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!globalVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!globalVolumeProfile.TryGet(out globalVignette)) throw new System.NullReferenceException(nameof(globalVignette));
        
        localVolumeProfile = GameObject.Find("Local Volume").GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!localVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if(!localVolumeProfile.TryGet(out localVignette)) throw new System.NullReferenceException(nameof(localVignette));
    }

    void Update()
    {
        
        timePassed += Time.deltaTime;

        if (Input.GetKeyDown(escape)) SceneManager.LoadScene(0);                              // transition to main scene when player exits
        if (lvlHandler.enemiesKilled >= (int)lvlHandler.enemiesToKill && currTime == 0f)
        {
            currTime = timePassed;
            UnlockCursor();
            Time.timeScale = 0;
            statsOverlay.transform.Find("WinText").gameObject.SetActive(true);
            statsOverlay.transform.Find("NumKills").gameObject.GetComponent<TextMeshProUGUI>().text = levelHandler.enemiesKilled.ToString();
            statsOverlay.transform.Find("CurrTime").gameObject.GetComponent<TextMeshProUGUI>().text = currTime.ToString();
            statsOverlay.SetActive(true);
            
        }
        // if ((playerHealth.playerCurrentHealth <= 0 || ui.timeLeft == -1) && currTime == 0f)
        // {
        //     currTime = timePassed;
        //     StartCoroutine(WaitForDeathAnim()); // go to temp transiton scene
        // }
    }

    IEnumerator FadeOverlay(float time, float start, float end)
    {
        float timeLeft = time;
        while(timeLeft > 0)
        {
            // fade alpha/background/overlay here
            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator WaitForDeathAnim(){
        globalVignette.intensity.Override(1f);
        globalVignette.color.Override(Color.red);
        localVignette.intensity.Override(1f);
        localVignette.color.Override(Color.red);
        player.GetComponent<Movement>().enabled = false;
        mainCamera.GetComponent<MoveCamera>().enabled = false;
        mainCamera.GetComponent<Animator>().enabled = true;
        weaponShift.GetComponent<WeaponShiftingNew>().enabled = false;
        weaponBob.GetComponent<WeaponBobbing>().enabled = false;
        revolver.GetComponent<GunAttributes>().enabled = false;
        revolver.GetComponent<RevolverMovement>().enabled = false;
        yield return new WaitForSeconds(3f);
        //SceneManager.LoadScene(3);
        Time.timeScale = 0;
        UnlockCursor();
        statsOverlay.transform.Find("LoseText").gameObject.SetActive(true);
        statsOverlay.transform.Find("NumKills").gameObject.GetComponent<TextMeshProUGUI>().text = levelHandler.enemiesKilled.ToString();
        statsOverlay.transform.Find("CurrTime").gameObject.GetComponent<TextMeshProUGUI>().text = currTime.ToString();
        statsOverlay.SetActive(true);
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
