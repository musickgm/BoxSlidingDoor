using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    public AudioClip dropBallClip;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponentInParent<Ball>();
            if(ball != null)
            {
                AudioManager.Instance.PlayAudioClip(dropBallClip, ball.transform.position);
                ball.DestroySelf(true, false, 1);
            }
        }
    }
}
