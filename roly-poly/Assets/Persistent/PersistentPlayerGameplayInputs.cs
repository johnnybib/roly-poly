using UnityEngine.InputSystem;
using UnityEngine;

public class PersistentPlayerGameplayInputs : MonoBehaviour
{
    private bool canInput = false;
    public InputManager playerInputManager;
    public void SetPlayerController(PlayerController p)
    {
        playerInputManager = p.GetComponent<InputManager>();
    }

    public void SetCanInput(bool canInput)
    {
        this.canInput = canInput;
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (canInput)
            playerInputManager.OnStart(context);
    }
}
