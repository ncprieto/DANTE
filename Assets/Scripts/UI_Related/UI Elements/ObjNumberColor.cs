using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjNumberColor : MonoBehaviour
{
    public TextMeshProUGUI numText;

    private LustSpawns lvlHandler;
    private int prevEnemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        lvlHandler = GameObject.Find("LevelHandler").GetComponent<LustSpawns>();
        prevEnemiesKilled = lvlHandler.enemiesKilled;
    }

    // Update is called once per frame
    void Update()
    {
        numText.text = lvlHandler.enemiesKilled.ToString();
        if (lvlHandler.enemiesKilled != prevEnemiesKilled){
            StartCoroutine(FadeTextToZeroAlpha(1f, numText));
        }
        prevEnemiesKilled = lvlHandler.enemiesKilled;
    }

    private IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
