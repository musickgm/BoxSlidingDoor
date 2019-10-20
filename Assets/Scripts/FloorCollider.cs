using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Ball ball = other.GetComponentInParent<Ball>();
            if(ball != null)
            {
                ball.DestroySelf();
            }
        }
    }
}
