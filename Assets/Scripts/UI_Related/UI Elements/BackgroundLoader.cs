using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoader : MonoBehaviour
{
    public GameObject UICanvas;
    public  List<GameObject> BackgroundsToLoad;
    public  List<GameObject> BackgroundAnchors;
    private Dictionary<string, GameObject> LoadedBackgrounds = new Dictionary<string, GameObject>();

    void Start()
    {
        for (int i = 0; i < BackgroundsToLoad.Count; i++)
        {
            GameObject Loaded = Instantiate(BackgroundsToLoad[i], UICanvas.transform, false);
            LoadedBackgrounds.Add(Loaded.name, Loaded);
            Loaded.transform.SetParent(BackgroundAnchors[i].transform, true);
        }
    }

    public bool SetBackgroundByNameTo(string Name, bool Active)
    {
        if(!LoadedBackgrounds.ContainsKey(Name + "(Clone)")) return false;
        LoadedBackgrounds[Name + "(Clone)"].SetActive(Active);
        return true;
    }

    public void SetAllBackgroundsTo(bool Active)
    {
        foreach(var Element in LoadedBackgrounds) Element.Value.SetActive(Active);
    }
}