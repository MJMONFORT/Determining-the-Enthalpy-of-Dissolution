using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropDownController : MonoBehaviour
{
    [SerializeField] int num;
    TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI text;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    void Start()
    {
        dropdown.value = -1;
    }

    void OnValueChanged(int index)
    {
        if (index != num) return;

        text.text = dropdown.options[index].text;

        dropdown.gameObject.SetActive(false);

        // ⭐ NEW — SlideViewController will show timing panel + auto hide
        LessonEvents.RaiseShowTimingPrompts(true, 0);

        var lfc = LessonContext.lessonFlowController;
        lfc.slideCompletionState[lfc.LFC_CurrentSlide] = true;

        LessonEvents.RaiseSetNextButtonState(true);
    }
}
