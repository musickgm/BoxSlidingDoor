using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialManager : Singleton<TrialManager>
{
    private Condition currentCondition;
    private ConditionList currentSet;
    private int setIndex = -1;
    private int trialIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        BoxEventSystem.OnDoorClosed += DoorClosedChecks;
        BoxEventSystem.OnSetStart += StartSet;
        BoxEventSystem.OnTrialStart += StartTrial;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnDoorClosed -= DoorClosedChecks;
        BoxEventSystem.OnSetStart -= StartSet;
        BoxEventSystem.OnTrialStart -= StartTrial;
    }


    private void StartSet()
    {
        setIndex++;
        currentSet = ConditionManager.Instance.Sets[setIndex];
        trialIndex = -1;
        BoxEventSystem.Instance.RaiseTrialStart();
    }

    private void StartTrial()
    {
        trialIndex++;
        currentCondition = currentSet.list[trialIndex];
        DoorSlider.Instance.DetermineTimeBetweenCloses(ConditionManager.Instance.frequencyValues[(int)currentCondition.frequency]);
        //Present ball
        DoorSlider.Instance.StartCycle();
    }

    private void DoorClosedChecks(float timeBetween)
    {
        //Check to see if the hand is still in the box.
        //Check to see if the ball is still in the box. 
        Run.After(timeBetween, DoorSlider.Instance.StartCycle);
    }
}
