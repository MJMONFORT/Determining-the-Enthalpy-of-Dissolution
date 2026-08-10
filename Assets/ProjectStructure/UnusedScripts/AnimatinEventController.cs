using UnityEngine;

public class AnimatinEventController : MonoBehaviour
{
    [SerializeField] protected GameObject[] activate;
    protected virtual void PlayEvent()
    {
        activate[1].SetActive(false);
        activate[0].SetActive(true);
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        LessonEvents.RaiseAnimationEnded(LessonContext.lessonFlowController.LFC_CurrentSlide);
    }
}
