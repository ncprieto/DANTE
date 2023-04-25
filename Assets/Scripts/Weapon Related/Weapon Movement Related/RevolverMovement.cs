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
    public override void ReceiveHitInfo(string tag)
    {
        if(tag != null)
        {
            shotsHit++;
            if(shotsHit == 1) StartCountdownCoroutine();           // if player has hit a shot then start the check
            else if(shotsHit > 1 && ChainShotCoroutine != null)
            {
                if(shotsHit < maxChainedShots + 1)
                {
                    Time.timeScale -= slowScale;                  // if players has chained shots together they apply slow effect
                    fovVFX.RevolverChainShotVFX(shotsHit);
                }
                StartCountdownCoroutine();
            }
        }
        else if(tag == null) UndoSlowAndVFX();
    }

    IEnumerator ChainShotCoroutine;
    void StartCountdownCoroutine()
    {
        if(ChainShotCoroutine != null) StopCoroutine(ChainShotCoroutine);
        ChainShotCoroutine = StartCountdown();
        StartCoroutine(ChainShotCoroutine);
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(maxTimeBetweenShots);
        UndoSlowAndVFX();
    }

    private void UndoSlowAndVFX()
    {
        if(shotsHit > 1)
        {
            fovVFX.UndoRevolverVFX();
            Time.timeScale = 1f;
            shotsHit = 0;
        }
    }
}
