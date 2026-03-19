using UnityEngine;
using UnityEngine.InputSystem;

public class ClickControllerPlayAnimation : ClickController
{
    [SerializeField] private Animator anim;
    [SerializeField] private bool isOn = true;

    protected override void Click(InputAction.CallbackContext context)
    {
        base.Click(context);
    }

    protected override void RayCastHitSuccess()
    {
       
    }


    protected void AnimationPlay()
    {
        if (anim != null)
        {
            anim.Play("Down");
        }
    }
    protected void Toogle()
    {
        isOn = !isOn;
        if (!isOn)
        {
            anim.enabled = true;
            anim.SetBool("OutIn", isOn);
        }
        else
        {
            anim.SetBool("OutIn", isOn);
        }

    }
}
