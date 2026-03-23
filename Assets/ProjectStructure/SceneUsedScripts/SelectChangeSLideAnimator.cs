using UnityEngine;
using UnityEngine.InputSystem;

public class SelectChangeSLideAnimator : ClickController
{
    [SerializeField] Animator animator;
    bool alreadyOn = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (alreadyOn)
        {
            animator.enabled = true;
        }
    }
    protected override void Click(InputAction.CallbackContext context)
    {
        base.Click(context);
    }

    protected override void RayCastHitSuccess()
    {
        animator.enabled = true;
        alreadyOn = true;
        LessonContext.lessonFlowController.Advance();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        animator.enabled = false;
    }
}
