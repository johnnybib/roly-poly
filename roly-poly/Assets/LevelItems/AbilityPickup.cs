using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{

    public AbilitiesToUnlock thisAbility;

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().UnlockAbility(thisAbility);
            Debug.Log("Got " + thisAbility.ToString());
        }
        Destroy(gameObject); //todo add animation with inenumerate before destory.
    }
}
