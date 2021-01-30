using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentPlayerController : MonoBehaviour
{
    public PersistentPlayerGameplayInputs gameplayInputs;
    private PlayerController playerController;
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

    public void SetPlayerController(PlayerController p)
    {
        this.playerController = p;
        gameplayInputs.SetPlayerController(p);
    }
}
