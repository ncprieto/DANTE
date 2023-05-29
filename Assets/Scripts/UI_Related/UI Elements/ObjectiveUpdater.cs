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
        ObjectiveText = Objective.GetComponent<TextMeshProUGUI>();
    }

    public void SetObjectiveTo(string text)
    {
        if(ObjectiveText != null) ObjectiveText.text = text;
    }
}
