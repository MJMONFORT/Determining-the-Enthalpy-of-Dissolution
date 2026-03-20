using UnityEngine;
using UnityEngine.EventSystems;

public class DropTextDrag : UIDropText
{
    [SerializeField] CanvasGroup snapcg;
  //  [SerializeField] UIDragText forAnsDrag;
    RectTransform recttransform;

    void Start()
    {
        recttransform = GetComponent<RectTransform>();
       // forAnsDrag.enabled = false;
    }
    public override void OnDrop(PointerEventData eventData)
    {
        UIDragText dragged =
            eventData.pointerDrag.GetComponent<UIDragText>();

        if (dragged != null)
        {
            dragged.droppedCorrectly = true;
            dragged.bigTextcanvasGroup.alpha = 0f;
            dragged.bigTextrectTransform.position = recttransform.position;
            this.enabled = false;
           // forAnsDrag.enabled = true;
            Show();
        }
    }

    void Show()
    {
        snapcg.alpha = 1f;
        snapcg.blocksRaycasts = true;
        snapcg.interactable = true;
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
    }
}
