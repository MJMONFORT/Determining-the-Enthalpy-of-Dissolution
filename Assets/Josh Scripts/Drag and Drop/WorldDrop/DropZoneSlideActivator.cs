using UnityEngine;

public class DropZoneSlideActivator : MonoBehaviour
{
    DropZone dropZone;
    [SerializeField] int[] activeSlides;
    [SerializeField] int activeLayer;
    [SerializeField] int inactiveLayer;

    void Awake()
    {
        dropZone = GetComponent<DropZone>();
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
        if (dropZone != null) dropZone.enabled = false;
        gameObject.layer = inactiveLayer;
    }

    void Activate(int slide)
    {
        if (dropZone == null) return;

        for (int i = 0; i < activeSlides.Length; i++)
        {
            if (activeSlides[i] != slide) continue;
            dropZone.enabled = true;
            gameObject.layer = activeLayer;
            return;
        }

        dropZone.enabled = false;
        gameObject.layer = inactiveLayer;
    }
}