using UnityEngine;

public class DraggableSlideActivator : MonoBehaviour
{
    Draggable draggable;
    [SerializeField] int[] activeSlides;
    [SerializeField] int draggableLayer;
    [SerializeField] int inactiveLayer;

    void Awake()
    {
        draggable = GetComponent<Draggable>();
        LessonEvents.ShowSlide += OnSlideChanged;
    }

    void OnDestroy()
    {
        LessonEvents.ShowSlide -= OnSlideChanged;
    }

    void OnSlideChanged(int slide)
    {
        if (!enabled) return;
        Activate(slide);
    }

    void OnEnable()
    {
        Activate(LessonContext.lessonFlowController.LFC_CurrentSlide);
    }

    void OnDisable()
    {
        if (draggable == null) return;
        draggable.SetInteractable(false, inactiveLayer);
    }

    void Activate(int slide)
    {
        if (draggable == null) return;

        for (int i = 0; i < activeSlides.Length; i++)
        {
            if (activeSlides[i] != slide) continue;
            draggable.SetInteractable(true, draggableLayer);
            return;
        }

        draggable.SetInteractable(false, inactiveLayer);
    }
}