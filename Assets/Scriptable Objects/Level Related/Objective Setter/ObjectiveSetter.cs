using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Objective Setter", menuName = "Level Related/Objective Setter", order = 1)]
public class ObjectiveSetter : ScriptableObject
{
    private ObjectiveUpdater Updater = null;
    private MonoBehaviour Controller = null;

    public void Initialize(ObjectiveUpdater InGame)
    {
        Updater = InGame;
    }

    public void SetObjective(string text, MonoBehaviour Messager)
    {
        if(Controller == Messager && Updater != null) Updater.SetObjectiveTo(text);
    }

    public void SetController(MonoBehaviour NewController, ObjectiveSetter Required)
    {
        if (Controller != NewController && Required != null) Controller = NewController;
    }

    public bool AmIControlling(MonoBehaviour Script)
    {
        return Script == Controller;
    }

    public void ReleaseControl(MonoBehaviour Script)
    {
        if (Controller == Script)
        {
            Controller = null;
            if (Updater != null) Updater.SetObjectiveTo("");
        }
    }
}
