using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    [Header("Input Actions")]
    public InputActionReference pressAction;
    public InputActionReference dragAction;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        pressAction.action.Enable();
        dragAction.action.Enable();
    }

    private void OnDisable()
    {
        pressAction.action.Disable();
        dragAction.action.Disable();
    }
}
