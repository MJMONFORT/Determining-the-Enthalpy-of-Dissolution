using MorphologyOfDifferentPlantGroupsCryptograms;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropTextForAns : UIDropText
{
    [SerializeField] CanvasGroup[] snapcg;
    [SerializeField] CanvasGroup highlighter;
    
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
        ButtonHighlightPulse scripthighlighter = highlighter.gameObject.GetComponent<ButtonHighlightPulse>();
        scripthighlighter.enabled = false;
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        highlighter.alpha = 0;
        LessonEvents.RaiseSetNextButtonState(true);
    }
}
