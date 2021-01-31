using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIcon : MonoBehaviour
{
    public Sprite fullImage;
    public Sprite emptyImage;


    private Image uiImage;
    void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    public void SetEmpty()
    {
        uiImage.sprite = emptyImage;
    }

    public void SetFull()
    {
        uiImage.sprite = fullImage;
    }
}
