using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentPlayerController : MonoBehaviour
{
    public PersistentPlayerGameplayInputs gameplayInputs;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCanInput(bool canInput)
    {
        gameplayInputs.SetCanInput(canInput);
    }
}
