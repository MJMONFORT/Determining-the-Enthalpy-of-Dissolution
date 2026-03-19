using UnityEngine;
using UnityEngine.EventSystems;

public class DropTextForAns : UIDropText
{
    [SerializeField] CanvasGroup[] snapcg;
    RectTransform recttransform;

    void Start()
    {
        recttransform = GetComponent<RectTransform>();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        UIDragText dragged =
            eventData.pointerDrag.GetComponent<UIDragText>();

        if (dragged != null)
        {
            dragged.droppedCorrectly = true;
            dragged.bigTextcanvasGroup.alpha = 0f;
            this.enabled = false;
            Show();
        }
    }

    void Show()
    {
        for(int i = 0; i < snapcg.Length; i++)
        {
            snapcg[i].alpha = 1f;
            snapcg[i].blocksRaycasts = false;
            snapcg[i].interactable = false;
        }
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
    }
}
