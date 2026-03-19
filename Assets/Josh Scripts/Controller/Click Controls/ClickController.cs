using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickController : MonoBehaviour
{

    [SerializeField] protected LayerMask mask;
    private Coroutine c;
    protected virtual void OnEnable()
    {
        if (c != null) { StopCoroutine(c); }
        c = StartCoroutine("InputEnable");
    }
   protected  IEnumerator InputEnable()
    {
        while (LessonContext.modelDragController == null)
            yield return null;
        //LessonContext.modelDragController.PressAction.action.Enable();
        InputManager.Instance.pressAction.action.performed += Click;
    }
    protected virtual void Click(InputAction.CallbackContext context)
    {
        Ray ray = LessonContext.modelDragController.RaycastCamera.ScreenPointToRay(LessonContext.modelDragController.PointerPosition());
        if(Physics.Raycast(ray,out RaycastHit hit,100f,mask))
        {
            if(LessonContext.slideviewcontroller.TimingPromptLength() > 0)
            {
               LessonEvents.RaiseShowTimingPrompts(true, 0);
            }
            LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
            LessonEvents.RaiseSetNextButtonState(true);

            RayCastHitSuccess();
                 
        }
    }

    protected virtual void RayCastHitSuccess()
    {
        
    }

    protected virtual void OnDisable()
    {
        InputManager.Instance.pressAction.action.performed -= Click;
    }
}
