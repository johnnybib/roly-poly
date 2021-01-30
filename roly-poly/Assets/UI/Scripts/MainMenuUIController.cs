using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Button loadGameButton;

    public void SetIsLoadGameActive(bool interactable)
    {
        loadGameButton.interactable = interactable;
    }
}
