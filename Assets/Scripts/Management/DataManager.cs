using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SelectionData
{
    public List<ConditionStruct> playerSelections = new List<ConditionStruct>();
}

[System.Serializable]
public struct ConditionStruct
{
    public int participantNumber;
    public float time;
    public string selectionType;
    public string size;
    public float frequency;
    public bool successful;
}


public class DataManager : Singleton<DataManager>
{
    public static int participantNumber;
    public static SelectionType selectionType;
    private static SelectionData selectData = new SelectionData();


    private void Start()
    {
    }

    private void OnEnable()
    {
        BoxEventSystem.OnTrialEnd += RecordSelection;
        BoxEventSystem.OnSetEnd += SaveSelectionData;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnTrialEnd -= RecordSelection;
        BoxEventSystem.OnSetEnd -= SaveSelectionData;
    }


    void SaveSelectionData()
    {
        SaveLoadManager.Save(selectData, "SelectionData");
    }

    public void RecordSelection(Condition currentCondition, bool success)
    {
        ConditionStruct currentSelection;
        currentSelection.participantNumber = participantNumber;
        currentSelection.time = Time.realtimeSinceStartup;
        currentSelection.selectionType = selectionType.ToString();
        currentSelection.size = currentCondition.size.ToString();
        currentSelection.frequency = ConditionManager.Instance.frequencyValues[(int)TrialManager.currentCondition.frequency];
        currentSelection.successful = success;
        selectData.playerSelections.Add(currentSelection);
    }


    public static void SetParticipantNumber(int number)
    {
        participantNumber = number;
        Debug.Log("Participant number = " + number);
    }

    public static void SetSelectionType(SelectionType type)
    {
        selectionType = type;
        Debug.Log("Participant type = " + type);
    }
}
