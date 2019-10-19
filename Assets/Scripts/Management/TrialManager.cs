using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : Singleton<TrialManager>
{
    public static Condition currentCondition;
    private ConditionList currentSet;
    private int setIndex = -1;
    private int trialIndex = 0;

    private void OnEnable()
    {
        BoxEventSystem.OnDoorClosed += DoorClosedChecks;
        BoxEventSystem.OnSetStart += IncrementSet;
        BoxEventSystem.OnTrialEnd += IncrementTrial;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnDoorClosed -= DoorClosedChecks;
        BoxEventSystem.OnSetStart -= IncrementSet;
        BoxEventSystem.OnTrialEnd -= IncrementTrial;
    }




    private void IncrementTrial(Condition _currentCondition, bool _success)
    {
        if(trialIndex >= currentSet.list.Count)
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
        if(setIndex >= ConditionManager.Instance.Sets.Count)
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
            BoxEventSystem.Instance.RaiseTrialStart(setIndex, trialIndex);
        }
    }


    private void DoorClosedChecks(float timeBetween)
    {
        //Check to see if the hand is still in the box.
        if (Box.Instance.NumberOfHands() > 0)
        {
            BoxEventSystem.Instance.RaiseTrialEnd(currentCondition, false);
        }
        //Check to see if the ball is still in the box. 
        else if (!Box.Instance.IsBallInside())
        {
            BoxEventSystem.Instance.RaiseTrialEnd(currentCondition, true);
        }
        else
        {
            Run.After(timeBetween, DoorSlider.Instance.StartCycle);
        }
    }


}
