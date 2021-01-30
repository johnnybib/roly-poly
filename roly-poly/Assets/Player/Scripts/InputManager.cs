using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;


public class InputManager : MonoBehaviour
{
    [Tooltip("How snappy do you want your controls to be? (smaller is mooshier)")]
    public float controlSnappiness;
    public float SmashThreshold;
    [System.Serializable]
    public class StickEvent : UnityEvent<Vector2> { }
    public StickEvent OnStick;

    [System.Serializable]
    public class StickSmashEvent : UnityEvent<int> { }
    public StickSmashEvent OnStickSmash;

    [System.Serializable]
    // The parameter value is not actually used. The float is used as a placeholder for a new parameter in the future.
    public class ButtonPressEvent : UnityEvent<float> { }

    [System.Serializable]
    public class ButtonReleaseEvent : UnityEvent<float> { }

    [System.Serializable]
    public struct ButtonEvent
    {
        public ButtonPressEvent pressEvent;
        public ButtonReleaseEvent releaseEvent;
    }

    public ButtonEvent NorthAny;
    public ButtonEvent NorthCenter;
    public ButtonEvent NorthUp;
    public ButtonEvent NorthDown;
    public ButtonEvent NorthSide;
    public ButtonEvent SouthAny;
    public ButtonEvent SouthCenter;
    public ButtonEvent SouthUp;
    public ButtonEvent SouthDown;
    public ButtonEvent SouthSide;
    public ButtonEvent EastAny;
    public ButtonEvent EastCenter;
    public ButtonEvent EastUp;
    public ButtonEvent EastDown;
    public ButtonEvent EastSide;
    public ButtonEvent WestAny;
    public ButtonEvent WestCenter;
    public ButtonEvent WestUp;
    public ButtonEvent WestDown;
    public ButtonEvent WestSide;
    public ButtonEvent RTriggerAny;
    public ButtonEvent LTriggerAny;
    public ButtonEvent RBumperAny;
    public ButtonEvent LBumperAny;
    public ButtonEvent StartAny;

    public enum StickInput { Any, Center, Up, Down, Side };
    public enum ButtonInput { None, Any, North, South, East, West, LTrigger, RTrigger, LBumper, RBumper, Start, QuitOut };

    public InputSettings settings;
    private PlayerInputs currentInputs;
    private Vector2 previousStickPos;
    private float systemXInput;
    private float systemYInput;

    // Manually set a smash cooldown so that it can't be spammed
    private float smashStickCD = 0.1f;
    private float smashStickCDCounter = 0f;
    private bool isSmashingX = false;

    Dictionary<ComboInput, ButtonEvent> buttonMap =
    new Dictionary<ComboInput, ButtonEvent>();

    public struct PlayerInputs
    {
        private float LStickVerticalDeadzoneMin;
        private float LStickHorizontalDeadzoneMin;
        private StickInput stickXDir
        {
            // Helper property for stickDir
            // Handles logic for x-dir of stick. Returns Center or Side.
            get
            {
                if (Mathf.Abs(stickXValue) < LStickHorizontalDeadzoneMin)
                {
                    return StickInput.Center;
                }
                else
                {
                    return StickInput.Side;
                }
            }
        }
        private StickInput stickYDir
        {
            // Helper property for stickDir
            // Handles logic for y-dir of stick. Returns Center, Up, or Down
            get
            {
                if (Mathf.Abs(stickYValue) < LStickVerticalDeadzoneMin)
                {
                    return StickInput.Center;
                }
                else if (stickYValue > 0f)
                {
                    return StickInput.Up;
                }
                else
                {
                    return StickInput.Down;
                }
            }
        }
        public StickInput stickDir
        {
            get
            {
                if (stickXDir == StickInput.Center && stickYDir == StickInput.Center)
                {
                    return StickInput.Center;
                }
                else if (stickXDir == StickInput.Center) // Stick is vertical
                {
                    return stickYDir;
                }
                else if (stickYDir == StickInput.Center) // Stick is horizontal
                {
                    return stickXDir;
                }
                else // Stick is Diagonal
                {
                    // Check if more vertical or more horizontal
                    // Equal case: use horizontal
                    if (Mathf.Abs(stickXValue) >= Mathf.Abs(stickYValue)) return stickXDir;
                    else return stickYDir;
                }
            }
        }

        public float stickXValue;
        public float stickYValue;
        public ButtonInput button;
        public PlayerInputs(float stickXValue, float stickYValue, float xDeadZone, float yDeadZone)
        {
            this.stickXValue = stickXValue;
            this.stickYValue = stickYValue;
            this.LStickHorizontalDeadzoneMin = xDeadZone;
            this.LStickVerticalDeadzoneMin = yDeadZone;
            this.button = ButtonInput.Any;
        }

    }

    public struct ComboInput
    {
        public ComboInput(StickInput stick, ButtonInput button)
        {
            this.stick = stick;
            this.button = button;
        }
        public StickInput stick;
        public ButtonInput button;

        public override int GetHashCode() // New arbitrary hashfunction
        {
            int hash = 17;
            hash = hash * 31 + (int)stick;
            hash = hash * 31 + (int)button;
            return hash;
        }

        public override bool Equals(object other)
        {
            if (other.GetType() != typeof(ComboInput)) return false;
            ComboInput otherInput = (ComboInput)other;
            return stick == otherInput.stick && button == otherInput.button;
        }
    }

    public void Awake()
    {
        Assert.IsTrue(settings);
        // Sets deadzones to the current input settings.
        float combomInputDeadzoneMin = 0.6f;
        // currentInputs = new PlayerInputs(0f, 0f, settings.defaultDeadzoneMin, settings.defaultDeadzoneMin);
        currentInputs = new PlayerInputs(0f, 0f, combomInputDeadzoneMin, combomInputDeadzoneMin);
        ComboInput combo = new ComboInput(StickInput.Any, ButtonInput.North);
        buttonMap[combo] = NorthAny;
        combo = new ComboInput(StickInput.Center, ButtonInput.North);
        buttonMap[combo] = NorthCenter;
        combo = new ComboInput(StickInput.Up, ButtonInput.North);
        buttonMap[combo] = NorthUp;
        combo = new ComboInput(StickInput.Down, ButtonInput.North);
        buttonMap[combo] = NorthDown;
        combo = new ComboInput(StickInput.Side, ButtonInput.North);
        buttonMap[combo] = NorthSide;

        combo = new ComboInput(StickInput.Any, ButtonInput.South);
        buttonMap[combo] = SouthAny;
        combo = new ComboInput(StickInput.Center, ButtonInput.South);
        buttonMap[combo] = SouthCenter;
        combo = new ComboInput(StickInput.Up, ButtonInput.South);
        buttonMap[combo] = SouthUp;
        combo = new ComboInput(StickInput.Down, ButtonInput.South);
        buttonMap[combo] = SouthDown;
        combo = new ComboInput(StickInput.Side, ButtonInput.South);
        buttonMap[combo] = SouthSide;

        combo = new ComboInput(StickInput.Any, ButtonInput.East);
        buttonMap[combo] = EastAny;
        combo = new ComboInput(StickInput.Center, ButtonInput.East);
        buttonMap[combo] = EastCenter;
        combo = new ComboInput(StickInput.Up, ButtonInput.East);
        buttonMap[combo] = EastUp;
        combo = new ComboInput(StickInput.Down, ButtonInput.East);
        buttonMap[combo] = EastDown;
        combo = new ComboInput(StickInput.Side, ButtonInput.East);
        buttonMap[combo] = EastSide;

        combo = new ComboInput(StickInput.Any, ButtonInput.West);
        buttonMap[combo] = WestAny;
        combo = new ComboInput(StickInput.Center, ButtonInput.West);
        buttonMap[combo] = WestCenter;
        combo = new ComboInput(StickInput.Up, ButtonInput.West);
        buttonMap[combo] = WestUp;
        combo = new ComboInput(StickInput.Down, ButtonInput.West);
        buttonMap[combo] = WestDown;
        combo = new ComboInput(StickInput.Side, ButtonInput.West);
        buttonMap[combo] = WestSide;

        combo = new ComboInput(StickInput.Any, ButtonInput.RTrigger);
        buttonMap[combo] = RTriggerAny;
        combo = new ComboInput(StickInput.Any, ButtonInput.LTrigger);
        buttonMap[combo] = LTriggerAny;
        
        combo = new ComboInput(StickInput.Any, ButtonInput.RBumper);
        buttonMap[combo] = RBumperAny;
        combo = new ComboInput(StickInput.Any, ButtonInput.LBumper);
        buttonMap[combo] = LBumperAny;

        combo = new ComboInput(StickInput.Any, ButtonInput.Start);
        buttonMap[combo] = StartAny;



        previousStickPos = Vector2.zero;
    }



    private void FixedUpdate()
    {
        // Debug.Log(string.Format("Stick Direction: {0} @@ Stick X Value: {1} @@ Stick Y Value: {2}", currentInputs.stickDir, currentInputs.stickXValue, currentInputs.stickYValue));
        // Implementation of low pass filter on stick inputs needs a continous update of stick position.
        previousStickPos.y = currentInputs.stickYValue;
        previousStickPos.x = currentInputs.stickXValue;
        currentInputs.stickXValue = CalculatePersistentInput(systemXInput, currentInputs.stickXValue);
        currentInputs.stickYValue = CalculatePersistentInput(systemYInput, currentInputs.stickYValue);
        HandleStickInput();
        UpdateSmashStickChecker();
    }

    private void HandleButtonInput(float duration = 0f, bool isRelease = false)
    {
        // Check if a non-combo input is specified
        ComboInput comboNoStick = new ComboInput(StickInput.Any, currentInputs.button);
        ButtonEvent newEvent;
        if (buttonMap.TryGetValue(comboNoStick, out newEvent))
        {
            if (InvokeButtonEvents(newEvent, duration, isRelease)) return;
        }
        // Find function mapped at combo input
        ComboInput comboWithStick = new ComboInput(currentInputs.stickDir, currentInputs.button);
        if (buttonMap.TryGetValue(comboWithStick, out newEvent))
        {
            if (InvokeButtonEvents(newEvent, duration, isRelease)) return;
        }

        // If no event is found, assume a center stick input. Otherwise return
        comboWithStick = new ComboInput(StickInput.Center, currentInputs.button);
        if (buttonMap.TryGetValue(comboWithStick, out newEvent))
        {
            InvokeButtonEvents(newEvent, duration, isRelease);
        }
    }

    private bool InvokeButtonEvents(ButtonEvent newEvent, float duration, bool isRelease)
    {
        if (newEvent.pressEvent.GetPersistentEventCount() != 0 && !isRelease)
        {
            newEvent.pressEvent.Invoke(duration);
            return true;
        }
        if (newEvent.releaseEvent.GetPersistentEventCount() != 0 && isRelease)
        {
            newEvent.releaseEvent.Invoke(duration);
            return true;
        }
        return false;
    }

    private void HandleStickInput()
    {
        Vector2 currentStickPos = new Vector2(currentInputs.stickXValue, currentInputs.stickYValue);
        OnStick.Invoke(currentStickPos);
        // Debug.Log(string.Format("Stick Direction: {0} @@ Stick X Value: {1} @@ Stick Y Value: {2}", currentInputs.stickDir, currentInputs.stickXValue, currentInputs.stickYValue));

    }

    private void HandleSmashStickInputX(float rawXStickPos)
    {

        float stickSpeedX = (rawXStickPos - previousStickPos.x) / Time.deltaTime;

        if (Mathf.Abs(stickSpeedX) > SmashThreshold)
        {
            //Debug.Log(string.Format("Passed stick speed for smash with pos {0}", rawXStickPos));
            if (Mathf.Abs(rawXStickPos) < 0.9f)
            {
                return;
            }
            // Check for smash inputs
            // Make sure stick is moving out
            if (Mathf.Abs(rawXStickPos) < Mathf.Abs(previousStickPos.x)) 
            {
                return;
            }
            isSmashingX = true;
            smashStickCDCounter = 0;
            OnStickSmash.Invoke((int)Mathf.Sign(rawXStickPos));
        }
    }

    private void UpdateSmashStickChecker()
    {
        if (!isSmashingX) return;
        smashStickCDCounter += Time.fixedDeltaTime;
        if (smashStickCDCounter > smashStickCD) isSmashingX = false;
    }



    private float CalculatePersistentInput(float systemInput, float persistentInput)
    {
        persistentInput = Mathf.Lerp(
             persistentInput, systemInput, controlSnappiness * Time.fixedDeltaTime);
        float minInputAmount = 0.01f;
        if (Mathf.Abs(persistentInput) < minInputAmount) return 0f;
        return persistentInput;
    }

    public void OnVertical(InputAction.CallbackContext context)
    {
        previousStickPos.y = currentInputs.stickYValue;
        systemYInput = context.ReadValue<float>();
        currentInputs.stickYValue = CalculatePersistentInput(systemYInput, currentInputs.stickYValue);
        HandleStickInput();
    }

    public void OnHorizontal(InputAction.CallbackContext context)
    {
        previousStickPos.x = currentInputs.stickXValue;
        systemXInput = context.ReadValue<float>();
        currentInputs.stickXValue = CalculatePersistentInput(systemXInput, currentInputs.stickXValue);
        HandleStickInput();
        HandleSmashStickInputX(context.ReadValue<float>());
    }

    public void OnNorth(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.North;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }

    public void OnSouth(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.South;
        if (context.started)
        {
            HandleButtonInput();
        }
        else
        {
            HandleButtonInput((float)context.duration, true);
        }
    }
    public void OnEast(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.East;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }
    public void OnWest(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.West;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }
    public void OnRTrigger(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.RTrigger;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }
    public void OnLTrigger(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.LTrigger;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }

    public void OnRBumper(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.RBumper;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }
    public void OnLBumper(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.LBumper;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        currentInputs.button = ButtonInput.Start;
        if (context.started)
        {
            HandleButtonInput();
        }
        else if(context.performed)
        {
            HandleButtonInput((float)context.duration, true);
        }
    }

}