using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverMovement : GunMovement
{
    
    [Header("Movement Effect Variables")]
    public float slowScale;
    public float maxChainedShots;
    public float maxTimeBetweenShots;

    int shotsHit;
    public override void ReceiveHitInfo(RaycastHit hit)
    {
        if(hit.transform == null)                                                  // if players missed then completely cancel the slow effect
        {
            StopCountdownCoroutine();
            Time.timeScale = 1f;
            shotsHit = 0;
            return;
        }
        else
        {
            shotsHit++;
            if(shotsHit == 1) StartCountdownCoroutine();                           // if player has hit a shot then start the check
            else if(shotsHit > 1 && inChainShotWindow)
            {
                if(shotsHit < maxChainedShots + 1) Time.timeScale -= slowScale;    // if players has chained shots together they apply slow effect
                StartCountdownCoroutine();
            }
        }
    }

    bool ChainShotRunning;
    IEnumerator ChainShotCoroutine;
    void StartCountdownCoroutine()
    {
        StopCountdownCoroutine();
        ChainShotCoroutine = StartCountdown();
        StartCoroutine(ChainShotCoroutine);
    }

    void StopCountdownCoroutine()
    {
        if(ChainShotRunning) StopCoroutine(ChainShotCoroutine);
    }

    bool inChainShotWindow;
    private IEnumerator StartCountdown()
    {
        ChainShotRunning = true;
        inChainShotWindow = true;
        yield return new WaitForSeconds(maxTimeBetweenShots);
        inChainShotWindow = false;
        ChainShotRunning = false;
        Time.timeScale = 1f;
        shotsHit = 0;
    }
}
