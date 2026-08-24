using UnityEngine;

public class AnimatorInActive : AnimatinEventController
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool disableAnimatorOnPlay = true;

    protected override void PlayEvent()
    {
        if (disableAnimatorOnPlay)
            _animator.enabled = false;

        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        LessonEvents.RaiseAnimationEnded(LessonContext.lessonFlowController.LFC_CurrentSlide);
    }
}
