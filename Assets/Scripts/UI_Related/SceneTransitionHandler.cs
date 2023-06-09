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
    public TimeUpdater timeUI;

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
        escape = (KeyCode)PlayerPrefs.GetInt("Escape", 27);
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

        if (Input.GetKeyDown(escape)) SceneManager.LoadScene("MainMenu");                              // transition to main scene when player exits
        if (lvlHandler.enemiesKilled >= (int)lvlHandler.enemiesToKill && currTime == 0f)
        {
            currTime = timePassed;
            UnlockCursor();
            Time.timeScale = 0;
            statsOverlay.transform.Find("WinText").gameObject.SetActive(true);
            statsOverlay.transform.Find("NumKills").gameObject.GetComponent<TextMeshProUGUI>().text = levelHandler.enemiesKilled.ToString();
            statsOverlay.transform.Find("CurrTime").gameObject.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(currTime / 60).ToString() + ":" + Mathf.FloorToInt(currTime % 60).ToString();
            statsOverlay.transform.Find("Grade").gameObject.GetComponent<TextMeshProUGUI>().text = CalculateGrade((float)lvlHandler.enemiesKilled / (float)lvlHandler.enemiesToKill, currTime);
            statsOverlay.SetActive(true);
        }
        if ((playerHealth.playerCurrentHealth <= 0 || timeUI.timeLeft == -1) && currTime == 0f)
        {
            currTime = timePassed;
            StartCoroutine(WaitForDeathAnim());
        }
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
        statsOverlay.transform.Find("CurrTime").gameObject.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(currTime / 60).ToString() + ":" + Mathf.FloorToInt(currTime % 60).ToString();
        statsOverlay.transform.Find("Grade").gameObject.GetComponent<TextMeshProUGUI>().text = CalculateGrade((float)lvlHandler.enemiesKilled / (float)lvlHandler.enemiesToKill, currTime);
        statsOverlay.SetActive(true);
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LustLevel");
    }

    public void SendToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public string CalculateGrade(float killsPercent, float time){
        float parTime = PlayerPrefs.GetFloat("Par Time");
        float timeGrade = 0f;
        float killGrade = 0f;
        if (time <= parTime){
            timeGrade = 5f;
        }
        else if (time <= parTime + 30f){
            timeGrade = 4f;
        }
        else if (time <= parTime + 75f){
            timeGrade = 3f;
        }
        else if (time <= parTime + 135f){
            timeGrade = 2f;
        }
        else{
            timeGrade = 1f;
        }

        if (killsPercent >= .999f){
            killGrade = 5f;
        }
        else if (killsPercent >= .75f){
            killGrade = 4f;
        }
        else if (killsPercent >= .5f){
            killGrade = 3f;
        }
        else if (killsPercent >= .25f){
            killGrade = 2f;
        }
        else{
            killGrade = 1f;
        }

        float avg = Mathf.Floor((timeGrade + killGrade) / 2f);
        if (playerHealth.playerCurrentHealth <= 0f){
            avg -= 2f;
            avg = Mathf.Clamp(avg, 1f, 5f);
        }

        Debug.Log(timeGrade);
        Debug.Log(killGrade);
        Debug.Log(killsPercent);
        Debug.Log(avg);

        if (avg == 5f){
            return "S";
        }
        else if (avg == 4f){
            return "A";
        }
        else if (avg == 3f){
            return "B";
        }
        else if (avg == 2f){
            return "C";
        }
        else{
            return "D";
        }
    }
}
