using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the geology events.
/// </summary>
public class BoxEventSystem : Singleton<BoxEventSystem>
{
    #region delegates and events
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

    public void RaiseDoorClosed(float timeBetween)
    {
        OnDoorClosed?.Invoke(timeBetween);
    }

    public void RaiseTrialStart()
    {
        OnTrialStart?.Invoke();
    }

    public void RaiseTrialEnd(Condition currentCondition, bool success)
    {
        OnTrialEnd?.Invoke(currentCondition, success);
    }

    public void RaiseSetStart()
    {
        OnSetStart?.Invoke();
    }

    public void RaiseSetEnd()
    {
        OnSetEnd?.Invoke();
    }

    public void RaiseAllSetsEnd()
    {
        OnAllSetsEnd?.Invoke();
    }
}
