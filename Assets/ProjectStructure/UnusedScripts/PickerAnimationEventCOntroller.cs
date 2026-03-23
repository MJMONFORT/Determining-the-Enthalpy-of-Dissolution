using UnityEngine;

public class PickerAnimationEventCOntroller : AnimatinEventController
{
    protected override void PlayEvent()
    {
        activate[0].SetActive(true);
    }
}
