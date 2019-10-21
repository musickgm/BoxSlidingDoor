using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public VRInteraction.VRInteractableItem interactionScript;
    public bool recordDropEvent = true;


    private void Start()
    {
        if(recordDropEvent)
        {
            interactionScript.dropEvent.AddListener(TrialManager.Instance.ReleaseBall);
        }
    }

    public void SetScale(float _scale)
    {
        transform.localScale = new Vector3(_scale, _scale, _scale);
        interactionScript.interactionDistance = _scale;
        print(_scale);
        if(_scale < 0.15)
        {
            interactionScript.interactionDistance += 0.05f;
        }
    }

    public void DestroySelf()
    {
        interactionScript.dropEvent.RemoveAllListeners();
        Destroy(this.gameObject);
    }
}
