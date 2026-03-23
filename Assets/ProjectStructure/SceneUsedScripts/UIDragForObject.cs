using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragForObject : UIDrag
{
    [SerializeField] string _tag;
    [SerializeField] GameObject[] drag_GO;
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
    }

    protected override void PhysicsRayCast(Vector3 pos)
    {
        Ray ray = LessonContext.modelDragController.RaycastCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject go = hit.collider.gameObject;
            if (go.tag == _tag)
            {
                drag_GO[0].SetActive(true);
                drag_GO[1].SetActive(false);
                LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
                LessonEvents.RaiseSetNextButtonState(true);
                LessonContext.lessonFlowController.ShowCurrentPromptIteratted();
            }
            else
            {
                Debug.Log("physics");
                LessonEvents.RaiseShowNegativePrompt(0);
            }
        }
    }
}
