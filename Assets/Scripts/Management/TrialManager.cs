using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : Singleton<TrialManager>
{
    public static Condition currentCondition;
    public AudioClip alarmClip;
    private ConditionList currentSet;
    [HideInInspector]
    public int setIndex = -1;
    [HideInInspector]
    public int trialIndex = 0;
    private bool alarmed = false;

    private void OnEnable()
    {
        BoxEventSystem.OnDoorClosed += DoorClosedChecks;
        BoxEventSystem.OnTrialEnd += IncrementTrial;
        BoxEventSystem.OnSetEnd += IncrementSet;
        BoxEventSystem.OnStartExperiment += StartExperiment;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnDoorClosed -= DoorClosedChecks;
        BoxEventSystem.OnTrialEnd -= IncrementTrial;
        BoxEventSystem.OnSetEnd -= IncrementSet;
        BoxEventSystem.OnStartExperiment -= StartExperiment;
    }


    private void StartExperiment()
    {
        setIndex = 0;
        trialIndex = 0;
        currentSet = ConditionManager.Instance.Sets[setIndex];
        currentCondition = currentSet.list[trialIndex];
        BoxEventSystem.Instance.RaiseSetStart(0);
    }

    private void IncrementTrial(Condition _currentCondition, bool _success)
    {
        alarmed = false;
        if(trialIndex >= currentSet.list.Count - 1)
        {
            BoxEventSystem.Instance.RaiseSetEnd();
            return;
        }
        else
        {
            trialIndex++;
            currentCondition = currentSet.list[trialIndex];
            BoxEventSystem.Instance.RaiseTrialStart(setIndex, trialIndex);
        }
    }

    private void IncrementSet()
    {
        if(setIndex >= ConditionManager.Instance.Sets.Count - 1)
        {
            BoxEventSystem.Instance.RaiseAllSetsEnd();
            return;
        }
        else
        {
            setIndex++;
            trialIndex = 0;
            currentSet = ConditionManager.Instance.Sets[setIndex];
            currentCondition = currentSet.list[trialIndex];
            BoxEventSystem.Instance.RaiseSetStart(setIndex);
        }
    }


    private void DoorClosedChecks(float timeBetween)
    {
        if(alarmed)
        {
            return;
        }
        //Check to see if the hand is still in the box.
        if (Box.Instance.NumberOfHands() > 0)
        {
            AudioManager.Instance.PlayAudioClip(alarmClip, Box.Instance.transform.position);
            BoxEventSystem.Instance.RaiseTrialEnd(currentCondition, false);
        }
        else if (!Box.Instance.IsBallInside())
        {
            return;
        }
        else
        {
            Run.After(timeBetween, DoorSlider.Instance.StartCycle);
        }
    }

    public void ReleaseBall()
    {
        if(!alarmed)
        {
            if(!Box.Instance.IsBallInside())   //Check to see if the ball is still in the box. 
            {
                BoxEventSystem.Instance.RaiseTrialEnd(currentCondition, true);
            }
        }
    }

    public void SetAlarm()
    {
        if(alarmed)
        {
            return;
        }
        alarmed = true;
        AudioManager.Instance.PlayAudioClip(alarmClip, Box.Instance.transform.position);
        BoxEventSystem.Instance.RaiseTrialEnd(currentCondition, false);
        if(Box.ballClone != null)
        {
            Box.ballClone.DestroySelf(false, false);
        }
        DoorSlider.Instance.EndCycle();
    }

}
