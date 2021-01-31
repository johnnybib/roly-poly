using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : MonoBehaviour
{
    void Save(PlayerController p)
    {
        SaveSystem.SavePlayerControllerData(p);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        if (other.CompareTag("playerDamageBox"))
        {
            Debug.Log("Saving at Save Station!");
            Save(other.GetComponentInParent<PlayerController>());
        }
    }
}
