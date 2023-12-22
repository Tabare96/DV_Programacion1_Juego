using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private int slowing;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PJ player = other.GetComponentInParent<PJ>();
        if (player != null)
        {
            player.slowed(slowing);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PJ player = collision.GetComponentInParent<PJ>();
        if (player != null)
        {
            player.speedRegain(slowing);
        }
    }
}
