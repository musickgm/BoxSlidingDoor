using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdminDisplay : MonoBehaviour
{
    public TextMeshProUGUI setText;
    public TextMeshProUGUI trialText;
    public TextMeshProUGUI radiusText;
    public TextMeshProUGUI frequencyText;

    public void SetAdminUI(int setNumber, int trialNumber, ObjectSize size, DoorFrequency frequency)
    {
        float frequencyValue = ConditionManager.Instance.frequencyValues[(int)frequency];

        setText.text = setNumber.ToString();
        trialText.text = trialNumber.ToString();
        radiusText.text = size.ToString();
        frequencyText.text = frequency.ToString() + " - " + frequencyValue;
    }

    public void AdminRestartTrial()
    {
        BoxEventSystem.Instance.RaiseTrialStart(TrialManager.Instance.setIndex, TrialManager.Instance.trialIndex);
    }
}
