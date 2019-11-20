using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class BasketCollider : MonoBehaviour
{
    public enum BasketType { goal, number, delete, start };
    public BasketType basketType = BasketType.goal;
    public int basketNumber = 0;
    public AudioClip goalAudioClip;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponentInParent<Ball>();
            if (ball != null)
            {
                ball.DestroySelf(true, true, 1);
            }
            if(basketType == BasketType.goal)
            {
                AudioManager.Instance.PlayAudioClip(goalAudioClip, transform.position);
            }
            else if (basketType == BasketType.number)
            {
                ParticipantNumberSelection.Instance.AddParticipantNumber(basketNumber);
            }
            else if (basketType == BasketType.delete)
            {
                ParticipantNumberSelection.Instance.RemoveParticipantNumber();
            }
            else if (basketType == BasketType.start)
            {
                ParticipantNumberSelection.Instance.SaveParticipantNumber();
                //SceneManager.LoadScene("Main");
                //StartCoroutine(WaitThenLoad());
                Valve.VR.SteamVR_LoadLevel.Begin("Main");
            }
        }
    }

}
