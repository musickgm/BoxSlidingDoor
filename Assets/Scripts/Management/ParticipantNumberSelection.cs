﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ParticipantNumberSelection : Singleton<ParticipantNumberSelection>
{
    public TextMeshProUGUI participantText;
    public AudioClip clickSound;
    private string participantNumberString = "";
    public List<int> participantNumbers = new List<int>();
    private readonly int maxNumbers = 4;
    private IEnumerator InputDelay;
    private bool readyForInput = true;
    private int adminPID;

    public void AddParticipantNumber(int number)
    {
        if (participantNumbers.Count >= maxNumbers || !readyForInput)
        {
            return;
        }
        participantNumbers.Add(number);
        UpdateParticipantText();
    }

    public void RemoveParticipantNumber()
    {
        if(!readyForInput)
        {
            return;
        }
        if (participantNumbers.Count <= 0)
        {
            return;
        }
        participantNumbers.RemoveAt(participantNumbers.Count - 1);
        UpdateParticipantText();
    }

    public void ClearParticipantNumber()
    {
        if (participantNumbers.Count == 0)
        {
            return;
        }
        for (int i = 0; i < participantNumbers.Count; i++)
        {
            RemoveParticipantNumber();
        }
    }

    private void UpdateParticipantText()
    {
        if(InputDelay != null)
        {
            StopCoroutine(InputDelay);
        }
        InputDelay = DelayAfterInput();
        StartCoroutine(InputDelay);
        if (participantNumbers.Count == 0)
        {
            participantText.text = "-";
            return;
        }
        participantText.text = "";
        for (int i = 0; i < participantNumbers.Count; i++)
        {
            participantText.text += participantNumbers[i];
        }
    }

    public void SaveParticipantNumber(bool adminOverride = false)
    {
        if(adminOverride)
        {
            DataManager.SetParticipantNumber(adminPID);
        }
        else
        {
            participantNumberString = "";
            for (int i = 0; i < participantNumbers.Count; i++)
            {
                participantNumberString += participantNumbers[i];
            }
            if (System.Int32.TryParse(participantNumberString, out int participantFinalNumber))
            {
                DataManager.SetParticipantNumber(participantFinalNumber);
                gameObject.SetActive(false);
            }
        }
    }

    public void SetAdminParticipantNumber(string input)
    {
        adminPID = int.Parse(input);
    }


    private IEnumerator DelayAfterInput()
    {
        readyForInput = false;
        yield return new WaitForSeconds(0.2f);
        readyForInput = true;
    }
}
