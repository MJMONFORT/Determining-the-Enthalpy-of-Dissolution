using UnityEngine;
using UnityEngine.EventSystems;
public class UIDragForAnim : UIDrag
{
    [SerializeField] string _tag;
    [SerializeField] Animator animator;
    [SerializeField] GameObject drag_GO;

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
            if (go.tag == _tag && animator != null)
            {
                drag_GO.SetActive(true);
                animator.enabled = true;
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
