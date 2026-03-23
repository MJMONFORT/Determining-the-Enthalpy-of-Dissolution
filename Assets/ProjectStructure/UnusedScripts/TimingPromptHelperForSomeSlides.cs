using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TimingPromptHelperForSomeSlides : MonoBehaviour
{
    Coroutine c;
    private void OnEnable()
    {
        CoroutineTiming();
    }

    void CoroutineTiming()
    {
        if(c != null) { StopCoroutine(c); }
        c = StartCoroutine("TimingC");
    }

    IEnumerator TimingC()
    {
        yield return null;
        LessonEvents.RaiseShowTimingPrompts(true, LessonContext.lessonFlowController.LFC_CurrentTimingPromptIndex);
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        yield return null;
        this.enabled = false;
    }
}
