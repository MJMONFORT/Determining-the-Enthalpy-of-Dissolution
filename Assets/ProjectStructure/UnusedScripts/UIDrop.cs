using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrop : MonoBehaviour, IDropHandler
{
    public int[] acceptedIDs;
    public void OnDrop(PointerEventData eventData)
    {
        UIToUIDrag drag = eventData.pointerDrag.GetComponent<UIToUIDrag>();
        if (drag == null) return;
        foreach(int id in acceptedIDs)
        {
            if (drag.dragID == id)
            {
                drag.droppedCorrectly = true;
               LessonContext.count++;
                if (LessonContext.count == 4)
                {
                    LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
                    LessonEvents.RaiseSetNextButtonState(true);
                }
                RectTransform dragRect = drag.GetComponent<RectTransform>();
                RectTransform dropRect = GetComponent<RectTransform>();


                dragRect.SetParent(dropRect.parent.transform);
                dragRect.position = dropRect.position;
                // this.gameObject.SetActive(false);
                return;
            }
        }   


    }
}