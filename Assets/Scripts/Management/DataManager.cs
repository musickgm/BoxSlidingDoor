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
    public float time;
    public DataManager.SelectionType selectionType;
    public ObjectSize size;
    public DoorFrequency frequency;
    public bool successful;
}


public class DataManager : Singleton<DataManager>
{
    public static int participantNumber;
    public static SelectionType selectionType;
    public enum SelectionType { gesture, controller }
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
        print("SAVING...");
    }

    public void RecordSelection(Condition currentCondition, bool success)
    {
        print("recorded selection");
        ConditionStruct currentSelection;
        currentSelection.time = Time.realtimeSinceStartup;
        currentSelection.selectionType = selectionType;
        currentSelection.size = currentCondition.size;
        currentSelection.frequency = currentCondition.frequency;
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
