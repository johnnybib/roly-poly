using UnityEngine.InputSystem;
using UnityEngine;

public class PersistentPlayerGameplayInputs : MonoBehaviour
{
    private bool canInput = false;
    [HideInInspector]
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
    public void OnHorizontal(InputAction.CallbackContext context)
    {
        if (canInput)
            playerInputManager.OnHorizontal(context);
    }
    public void OnSpace(InputAction.CallbackContext context)
    {
        if (canInput)
            playerInputManager.OnSouth(context);
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        if (canInput)
            playerInputManager.OnNorth(context);
    }

    public void OnJ(InputAction.CallbackContext context)
    {
        if (canInput)
            playerInputManager.OnWest(context);
    }

    public void OnK(InputAction.CallbackContext context)
    {
        if (canInput)
            playerInputManager.OnEast(context);
    }
}
