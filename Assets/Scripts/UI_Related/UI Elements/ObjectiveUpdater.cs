using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveUpdater : MonoBehaviour
{
    [Header ("UI Elements")]
    public  GameObject UICanvas;
    public  GameObject ObjectivePrefab;
    public  bool isVirgil;
    private GameObject Objective;
    private TextMeshProUGUI ObjectiveText;

    [Header ("Source")]
    public ObjectiveSetter Setter;

    void Awake()
    {
        Setter.Initialize(this);
    }

    void Start()
    {
        Objective  = Instantiate(ObjectivePrefab, UICanvas.transform, false);
        if (!isVirgil){
            ObjectiveText = Objective.GetComponent<TextMeshProUGUI>();
        }
        else{
            ObjectiveText = Objective.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }

    public void SetObjectiveTo(string text)
    {
        if(ObjectiveText != null) ObjectiveText.text = text;
    }
}
