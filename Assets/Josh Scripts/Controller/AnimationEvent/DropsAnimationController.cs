using UnityEngine;

public class DropsAnimationController : AnimatinEventController
{
    protected override void PlayEvent()
    {
        LessonContext.cineMachineFlowController.SwitchNextCameraInSlide();
    }
}
