using UnityEngine;

public class AnimatorInActive : AnimatinEventController
{
    [SerializeField] private Animator _animator;

    protected override void PlayEvent()
    {
        _animator.enabled = false;
    }
}
