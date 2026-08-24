using System.Collections.Generic;
using UnityEngine;

// Shows a highlighter GameObject next to whichever interactable the user should touch
// next, and hides it the moment they start interacting with it.
// Each entry is set by hand: pick the slide, drag in that slide's highlighter prop.
public class HighlighterActivationController : MonoBehaviour
{
    [System.Serializable]
    public class HighlightEntry
    {
        public int slideIndex;
        public GameObject highlightGO;
    }

    [SerializeField] HighlightEntry[] entries;

    Dictionary<int, GameObject> map;

    void Awake()
    {
        map = new Dictionary<int, GameObject>();
        foreach (var e in entries)
        {
            if (e == null || e.highlightGO == null) continue;
            map[e.slideIndex] = e.highlightGO;
            e.highlightGO.SetActive(false);
        }
    }

    void OnEnable()
    {
        LessonEvents.ShowSlide += OnSlideChanged;
        LessonEvents.InteractionStarted += OnInteractionHide;
        LessonEvents.InteractionSettled += OnInteractionHide;
    }

    void OnDisable()
    {
        LessonEvents.ShowSlide -= OnSlideChanged;
        LessonEvents.InteractionStarted -= OnInteractionHide;
        LessonEvents.InteractionSettled -= OnInteractionHide;
    }

    void OnSlideChanged(int slide)
    {
        foreach (var kvp in map)
            kvp.Value.SetActive(false);

        if (map.ContainsKey(slide)
            && !LessonContext.lessonFlowController.slideCompletionState.ContainsKey(slide))
        {
            map[slide].SetActive(true);
        }
    }

    void OnInteractionHide(int slide)
    {
        if (map.ContainsKey(slide))
            map[slide].SetActive(false);
    }
}
