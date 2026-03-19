using System.Collections;
using UnityEngine;

public class AllOptionsSelected : MonoBehaviour
{
    int selectioncount;
    [SerializeField] int totalSelectionCount;
    Coroutine c;
    [SerializeField] Animator UIanim;
    public void AfterSelectedOptions()
    {
        selectioncount++;
        if(selectioncount > totalSelectionCount)
        {
            LessonContext.lessonFlowController.LFC_CurrentTimingPromptIndex++;
            LessonEvents.RaiseShowTimingPrompts(true, LessonContext.lessonFlowController.LFC_CurrentTimingPromptIndex);
            LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
            LessonEvents.RaiseSetNextButtonState(true);
        }
    }

    public void EnableNextButton()
    {
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
    }

    public void SelectForTImingPrompt(int index)
    {
        LessonEvents.RaiseShowTimingPrompts(true,index);
    }

    public void UIAnimationPlay(string statename)
    {
        if(UIanim != null)
        {
            CoroutineAnim(statename);
        }
    }
    void CoroutineAnim(string statename)
    {
        if (c != null)
        {
            StopCoroutine(c);
        }
        c = StartCoroutine(UIANim(statename));
    }
    IEnumerator UIANim(string statename)
    {
        yield return new WaitForSeconds(1);
        UIanim.Play(statename);
    }
}
