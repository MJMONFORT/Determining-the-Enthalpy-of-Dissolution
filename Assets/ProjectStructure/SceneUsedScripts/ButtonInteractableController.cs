using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractableController : MonoBehaviour
{
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;

    void OnEnable()
    {
        LessonEvents.SetNextButtonState += SetNextState;
        LessonEvents.SetPrevButtonState += SetPrevState;
    }

    void OnDisable()
    {
        LessonEvents.SetNextButtonState -= SetNextState;
        LessonEvents.SetPrevButtonState -= SetPrevState;
    }

    void SetNextState(bool interactable)
    {
        nextButton.interactable = interactable;
    }

    void SetPrevState(bool interactable)
    {
        prevButton.interactable = interactable;
    }
}
