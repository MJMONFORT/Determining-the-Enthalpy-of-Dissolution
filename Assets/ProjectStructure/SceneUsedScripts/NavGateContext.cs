using System.Collections.Generic;
using UnityEngine;

public class NavGateContext : MonoBehaviour
{
    [SerializeField] int[] animatedSlideIndices;

    HashSet<int> animatedSlides;

    void Awake()
    {
        animatedSlides = new HashSet<int>(animatedSlideIndices);
    }

    void OnEnable()
    {
        LessonEvents.InteractionSettled += OnInteractionSettled;
        LessonEvents.AnimationEnded += OnAnimationEnded;
    }

    void OnDisable()
    {
        LessonEvents.InteractionSettled -= OnInteractionSettled;
        LessonEvents.AnimationEnded -= OnAnimationEnded;
    }

    void OnInteractionSettled(int slide)
    {
        if (!animatedSlides.Contains(slide)) return;
        LessonEvents.RaiseSetNextButtonState(false);
        LessonEvents.RaiseSetPrevButtonState(false);
    }

    void OnAnimationEnded(int slide)
    {
        if (!animatedSlides.Contains(slide)) return;
        LessonEvents.RaiseSetNextButtonState(true);
        LessonEvents.RaiseSetPrevButtonState(slide > 0);
    }
}
