using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractableController : MonoBehaviour
{
    [SerializeField] Button nextButton;

    void OnEnable()
    {
        LessonEvents.SetNextButtonState += SetState;
    }

    void OnDisable()
    {
        LessonEvents.SetNextButtonState -= SetState;
    }

    void SetState(bool interactable)
    {
        nextButton.interactable = interactable;
    }
}
