using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : MonoBehaviour
{
    public GameObject saveText;
    void Awake()
    {
        saveText.SetActive(false);
    }
    void Save(PlayerController p)
    {
        SaveSystem.SavePlayerControllerData(p);
        StartCoroutine("ShowThenHideText");
    }

    IEnumerator ShowThenHideText()
    {
        saveText.SetActive(true);
        yield return new WaitForSeconds(1f);
        saveText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        if (other.CompareTag("Player"))
        {
            PlayerController p = other.GetComponentInParent<PlayerController>();
            if (p.abilities.abilities.egg.unlocked)
            {
                p.PlayerWin();
            }
            else
            {
                Debug.Log("Saving at Save Station!");
                if (GlobalSFX.Instance)
                {
                    GlobalSFX.Instance.PlayGameSave();
                }
                Save(p);
            }

        }
    }
}
