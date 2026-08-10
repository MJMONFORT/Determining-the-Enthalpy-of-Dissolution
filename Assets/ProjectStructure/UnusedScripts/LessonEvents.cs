using System;
using UnityEngine;

public static class LessonEvents
{
    // Navigation
    public static event Action NextClicked;
    public static event Action PrevClicked;

    // Display
    public static event Action<int> ShowSlide;
    public static event Action<int> PromptIndex;
    public static event Action<int> NegativePrompts;
    public static event Action<int> InfoPrompts;
    public static event Action<bool,int> TimingPrompts;
    public static event Action<bool> SetNextButtonState;
    public static event Action<bool> SetPrevButtonState;


    // Interaction
    public static event Action<string> DragCompleted;

    // Animation gating
    public static event Action<int> InteractionSettled;
    public static event Action<int> AnimationEnded;


    public static void RaiseNext()
    {
        NextClicked?.Invoke();
    } 
    public static void RaisePrev()
    {
        PrevClicked?.Invoke();
    } 
    public static void RaiseDragCompleted(string id)
   
    {
        DragCompleted?.Invoke(id);
    }

    public static void RaiseShowPromptIndex( int currprompt)
    {
        PromptIndex?.Invoke(currprompt);
    }

    public static void RaiseShowNegativePrompt( int currprompt)
    {
        NegativePrompts?.Invoke(currprompt);
    }
    public static void RaiseShowInfoPrompt( int currprompt)
    {
        InfoPrompts?.Invoke(currprompt);
    }

    public static void RaiseShowTimingPrompts(bool show, int currpeompt)
    {
        TimingPrompts?.Invoke(show,currpeompt);
    }
    public static void RaiseShowSLide(int currslide)
    {
        ShowSlide?.Invoke(currslide);
    }
    public static void RaiseSetNextButtonState(bool state)
    {
        SetNextButtonState?.Invoke(state);
    }
    public static void RaiseSetPrevButtonState(bool state)
    {
        SetPrevButtonState?.Invoke(state);
    }
    public static void RaiseInteractionSettled(int slide)
    {
        InteractionSettled?.Invoke(slide);
    }
    public static void RaiseAnimationEnded(int slide)
    {
        AnimationEnded?.Invoke(slide);
    }
}
