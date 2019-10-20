using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the box events.
/// </summary>
public class BoxEventSystem : Singleton<BoxEventSystem>
{
    public float timeBetweenTrials = 3;

    #region delegates and events
    public delegate void StartExperiment();
    public static event StartExperiment OnStartExperiment;
    public delegate void DoorClosed(float timeBetween);
    public static event DoorClosed OnDoorClosed;
    public delegate void TrialStart();
    public static event TrialStart OnTrialStart;
    public delegate void TrialEnd(Condition currentCondition, bool success);
    public static event TrialEnd OnTrialEnd;
    public delegate void SetStart();
    public static event SetStart OnSetStart;
    public delegate void SetEnd();
    public static event SetEnd OnSetEnd;
    public delegate void AllSetsEnd();
    public static event AllSetsEnd OnAllSetsEnd;
    #endregion

    public void RaiseStartExperiment()
    {
        OnStartExperiment?.Invoke();
    }

    public void RaiseDoorClosed(float timeBetween)
    {
        OnDoorClosed?.Invoke(timeBetween);
    }

    public void RaiseTrialStart(int setNumber, int trialNumber)
    {
        print("Starting set " + (setNumber+1) + "; trial " + (trialNumber+1) + 
            "(Radius = " + TrialManager.currentCondition.size + "; Frequency = " + TrialManager.currentCondition.frequency + ")");
        IEnumerator trialWait = WaitBetweenTrials();
        StartCoroutine(trialWait);
    }

    public void RaiseTrialEnd(Condition currentCondition, bool success)
    {
        OnTrialEnd?.Invoke(currentCondition, success);
    }

    public void RaiseSetStart(int setNumber)
    {
        OnSetStart?.Invoke();
        RaiseTrialStart(setNumber, 0);
    }

    public void RaiseSetEnd()
    {
        OnSetEnd?.Invoke();
    }

    public void RaiseAllSetsEnd()
    {
        OnAllSetsEnd?.Invoke();
    }


    private IEnumerator WaitBetweenTrials()
    {
        yield return new WaitForSeconds(timeBetweenTrials);
        OnTrialStart?.Invoke();
    }
}
