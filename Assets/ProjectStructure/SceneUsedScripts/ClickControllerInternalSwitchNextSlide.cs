using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ClickControllerInternalSwitchNextSlide : ClickController
{
    [SerializeField] UnityEvent onClickSuccess;

    protected override void Click(InputAction.CallbackContext context)
    {
        base.Click(context);
    }

    protected override void RayCastHitSuccess()
    {
        LessonContext.lessonFlowController.ShowCurrentPromptIteratted();
        onClickSuccess?.Invoke();
    }
}
