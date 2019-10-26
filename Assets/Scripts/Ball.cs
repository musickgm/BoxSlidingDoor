using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public VRInteraction.VRInteractableItem interactionScript;
    public Leap.Unity.Interaction.InteractionBehaviour interactionScriptLeap;
    public bool recordDropEvent = true;

    [ReadOnly]
    public int setNumber;
    [ReadOnly]
    public int trialNumber;
    private bool destroying = false;


    private void Start()
    {
        if(recordDropEvent)
        {
            if(interactionScript != null)
            {
                interactionScript.dropEvent.AddListener(TrialManager.Instance.ReleaseBall);
            }
            else if(interactionScriptLeap != null)
            {
                interactionScriptLeap.OnGraspEnd += TrialManager.Instance.ReleaseBall;
            }
        }
    }

    public void SetScale(float _scale)
    {
        transform.localScale = new Vector3(_scale, _scale, _scale);
        if(interactionScript != null)
        {
            interactionScript.interactionDistance = _scale;
            if (_scale < 0.15)
            {
                interactionScript.interactionDistance += 0.05f;
            }
        }
    }

    public void SetTrial(int _setNumber, int _trialNumber)
    {
        setNumber = _setNumber;
        trialNumber = _trialNumber;
    }

    public void DestroySelf(bool attemptSuccess, bool basketSuccess, float time = 0)
    {
        if(destroying)
        {
            return;
        }
        destroying = true;
        if(interactionScript != null)
        {
            interactionScript.dropEvent.RemoveAllListeners();
        }
        else if (interactionScriptLeap != null)
        {
            interactionScriptLeap.OnGraspEnd -= TrialManager.Instance.ReleaseBall;
        }
        if(ScoreBoard.Instance != null)
        {
            ScoreBoard.Instance.UpdateCell(attemptSuccess, basketSuccess, setNumber, trialNumber);
        }
        StartCoroutine(WaitThenDestroy(time));
    }

    IEnumerator WaitThenDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

}
