using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Singleton<Box>
{
    public GameObject ballPrefab;
    public Transform spawnPosition;
    private GameObject ballClone;
    private bool ballInsideStatus = true;
    private int numberOfHandsInside = 0;


    private void OnEnable()
    {
        BoxEventSystem.OnTrialStart += NewTrial;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnTrialStart -= NewTrial;
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
        ballClone = Instantiate(ballPrefab, spawnPosition.position, Quaternion.identity);
        float scale = ConditionManager.Instance.radiusValues[(int)TrialManager.currentCondition.size] * 2;
        ballClone.transform.localScale = new Vector3(scale, scale, scale);
        VRInteraction.VRInteractableItem interactableItem = ballClone.GetComponentInChildren<VRInteraction.VRInteractableItem>();
        if(interactableItem != null)
        {
            interactableItem.interactionDistance = scale;
        }
    }




}
