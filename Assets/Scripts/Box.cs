﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Singleton<Box>
{
    public GameObject ballPrefabController;
    public GameObject ballPrefabGesture;
    public Transform spawnPosition;
    public static Ball ballClone;
    private bool ballInsideStatus = true;
    private int numberOfHandsInside = 0;


    private void OnEnable()
    {
        BoxEventSystem.OnTrialStart += NewTrial;
        BoxEventSystem.OnTrialEnd += DestroyBallIfFail;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnTrialStart -= NewTrial;
        BoxEventSystem.OnTrialEnd -= DestroyBallIfFail;
    }



    public bool IsBallInside()
    {
        return ballInsideStatus;
    }

    public int NumberOfHands()
    {
        return numberOfHandsInside;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            ballInsideStatus = true;
        }
        else if (other.CompareTag("Hand"))
        {
            numberOfHandsInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballInsideStatus = false;
        }
        else if (other.CompareTag("Hand"))
        {
            numberOfHandsInside--;
        }
    }

    private void NewTrial()
    {
        if (ballClone != null)
        {
            ballClone.DestroySelf(true, false);
        }
        GameObject clone = null;
        if (ConditionManager.Instance.selectionType == SelectionType.controller)
        {
            clone = Instantiate(ballPrefabController, spawnPosition.position, Quaternion.identity);
        }
        else if (ConditionManager.Instance.selectionType == SelectionType.gesture)
        {
            clone = Instantiate(ballPrefabGesture, spawnPosition.position, Quaternion.identity);
        }
        ballClone = clone.GetComponent<Ball>();
        ballClone.SetTrial(TrialManager.Instance.setIndex, TrialManager.Instance.trialIndex);
        float scale = ConditionManager.Instance.radiusValues[(int)TrialManager.currentCondition.size] * 2;
        ballClone.SetScale(scale);
    }

    private void DestroyBallIfFail(Condition _currentCondition, bool _success)
    {
        if(!_success && ballClone != null)
        {
            ballClone.DestroySelf(false, false);
        }
    }


}
