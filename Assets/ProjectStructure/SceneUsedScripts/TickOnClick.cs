using UnityEngine;
using UnityEngine.UI;

// Activates a tick mark child when this button is clicked.
// The listener is registered in code, so the button's OnClick list stays empty
// in the inspector and no per button wiring is needed.
[RequireComponent(typeof(Button))]
public class TickOnClick : MonoBehaviour
{
    [Tooltip("The tick mark to switch on. Auto-filled with the child named 'Tick' when left empty.")]
    [SerializeField] GameObject tick;

    [Tooltip("When true a second click hides the tick again.")]
    [SerializeField] bool toggle;

    Button button;

    void Reset()
    {
        FindTick();
    }

    void Awake()
    {
        if (tick == null) FindTick();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
    }

    void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClicked);
    }

    void FindTick()
    {
        Transform t = transform.Find("Tick");
        if (t != null) tick = t.gameObject;
    }

    // Public so it can also be driven from an inspector OnClick entry if wanted.
    public void OnClicked()
    {
        if (tick == null) return;

        tick.SetActive(toggle ? !tick.activeSelf : true);
    }
}
