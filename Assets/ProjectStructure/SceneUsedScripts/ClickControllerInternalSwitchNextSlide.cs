using UnityEngine;
using UnityEngine.InputSystem;

public class ClickControllerInternalSwitchNextSlide : ClickController
{
    protected override void Click(InputAction.CallbackContext context)
    {
        base.Click(context);
    }

    protected override void RayCastHitSuccess()
    {
        LessonContext.lessonFlowController.ShowCurrentPromptIteratted();
    }
}
