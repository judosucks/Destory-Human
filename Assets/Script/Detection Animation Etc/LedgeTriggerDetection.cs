using System;
using UnityEngine;

public class LedgeTriggerDetection : MonoBehaviour
{
    public bool isTouchingLedge{ get; private set;}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
               
                isTouchingLedge = true;
            } 
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                
                isTouchingLedge = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                
                isTouchingLedge = false;
            }
        }
    }
}
