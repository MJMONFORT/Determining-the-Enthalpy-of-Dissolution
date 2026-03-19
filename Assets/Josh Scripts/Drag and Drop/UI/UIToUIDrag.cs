using UnityEngine;
using UnityEngine.EventSystems;

public class UIToUIDrag : UIDrag
{

    public override void OnEndDrag(PointerEventData eventData)
    {
       
        if (!droppedCorrectly)
        {
            transform.position = originalPosition;
            transform.SetParent(originalParent);
            LessonEvents.RaiseShowNegativePrompt(0);
            cg.blocksRaycasts = true;
        }
       
    }
}
