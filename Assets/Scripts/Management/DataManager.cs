using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionData
{
    public List<Vector3> playerPositions = new List<Vector3>();
    public List<Vector3> playerDirection = new List<Vector3>();
}

[System.Serializable]
public class SelectionData
{
    public List<SelectionStruct> playerSelections = new List<SelectionStruct>();
}

[System.Serializable]
public struct SelectionStruct
{
    public float time;
    public enum SelectionType { rock, tool, question}
    public string selectionType;
    public string question;
    public string selection;
    public bool correct;
}


public class DataManager : Singleton<DataManager>
{
    public static int participantNumber;
    private static SelectionData selectData = new SelectionData();
    private PositionData posData = new PositionData();
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        IEnumerator _RecordPosition = RecordPosition();
        StartCoroutine(_RecordPosition);
    }

    /*private void OnEnable()
    {
        BoxEventSystem.OnRockEnd += SavePosData;
        BoxEventSystem.OnRockEnd += SaveSelectionData;
        BoxEventSystem.OnDoorClosed += RecordRockSelect;
        BoxEventSystem.OnTrialStart += RecordAcidTool;
        BoxEventSystem.OnSetStart += RecordLensTool;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnRockEnd -= SavePosData;
        BoxEventSystem.OnRockEnd -= SaveSelectionData;
        BoxEventSystem.OnDoorClosed -= RecordRockSelect;
        BoxEventSystem.OnTrialStart -= RecordAcidTool;
        BoxEventSystem.OnSetStart -= RecordLensTool;
    }*/

    void SavePosData()
    {
        SaveLoadManager.Save(posData, "PositionData");
    }

    void SaveSelectionData()
    {
        SaveLoadManager.Save(selectData, "SelectionData");
    }

    public void RecordSelection(SelectionStruct.SelectionType _type, string _selection, string _question = "", bool _correct = true)
    {
        SelectionStruct currentSelection;
        currentSelection.time = Time.realtimeSinceStartup;
        currentSelection.selectionType = _type.ToString();
        currentSelection.question = _question;
        currentSelection.selection = _selection;
        currentSelection.correct = _correct;
        selectData.playerSelections.Add(currentSelection);
    }

    public void RecordSelection(SelectionStruct.SelectionType _type, bool _acidSelection, string _question = "", bool _correct = true)
    {
        SelectionStruct currentSelection;
        currentSelection.time = Time.realtimeSinceStartup;
        currentSelection.selectionType = _type.ToString();
        currentSelection.question = _question;
        if(_acidSelection)
        {
            currentSelection.selection = "AcidReaction";
        }
        else
        {
            currentSelection.selection = "NoReaction";
        }
        currentSelection.correct = _correct;
        selectData.playerSelections.Add(currentSelection);
    }

    private void RecordRockSelect()
    {
        //RecordSelection(SelectionStruct.SelectionType.rock, LevelProgress.Instance.currentRock.name.ToString());
    }

    public void RecordAcidTool()
    {
        RecordSelection(SelectionStruct.SelectionType.tool, "AcidTool");
    }

    private void RecordLensTool()
    {
        RecordSelection(SelectionStruct.SelectionType.tool, "LensTool");
    }

    IEnumerator RecordPosition()
    {
        while(mainCamera != null)
        {
            posData.playerPositions.Add(mainCamera.transform.position);
            posData.playerDirection.Add(mainCamera.transform.forward);
            yield return new WaitForSeconds(1);
        }
    }

    public static void SetParticipantNumber(int number)
    {
        participantNumber = number;
        Debug.Log("Participant number = " + number);
    }
}
