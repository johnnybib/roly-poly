using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenSingleton : MonoBehaviour
{
    private static LoadingScreenSingleton instance;   //To check if already exists. If it does don't create a new one
    public static LoadingScreenSingleton Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

    }
}
