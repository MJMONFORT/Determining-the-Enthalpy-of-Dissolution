using UnityEngine;
using System.Collections;
public class BeakerdragRotAnim : BeakerDragable
{
    [SerializeField] string animatinonameofname;
    float Rduration = 1f;
    [SerializeField] Vector3 Beakerrot;
    Coroutine rotateToPour;
    [SerializeField] Animator animator;
    [SerializeField] float waittime;

    [Header("Played after this object hides")]
    [Tooltip("A second animator on another object, e.g. the Cardboard. Left empty means nothing plays.")]
    [SerializeField] Animator afterHideAnimator;
    [Tooltip("State name to play on afterHideAnimator, e.g. CardboardDown.")]
    [SerializeField] string afterHideStateName;
    protected override void CoroutineToTarget(Transform attachtransform)
    {
        animationName = animatinonameofname;
        base.CoroutineToTarget(attachtransform);
    }
    void CoroutineRotatePour()
    {
        if (rotateToPour != null) { StopCoroutine(rotateToPour); }
        rotateToPour = StartCoroutine(RotationtoTarget());
    }



    protected override IEnumerator PositoinToTarget(Transform attachtransform)
    {

        float elapsedtime = 0f;
        // transform.SetParent(null);
        Vector3 startpos = transform.position;
        Vector3 endpos = BeakerPos;
        while (elapsedtime < Rduration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / Rduration;
            transform.position = Vector3.Lerp(startpos, endpos, t); ;
            yield return null;
        }

        transform.position = endpos;
        if (Vector3.Distance(startpos, endpos) < dropTargetdist)
        {
            PlayDropAnimation();
            CoroutineRotatePour();
        }
    }

    // Plays the clip named in animatinonameofname on the assigned animator.
    // Both fields are inspector driven, so instances that leave them blank keep
    // their existing behaviour and only play the animator's default state.
    void PlayDropAnimation()
    {
        if (animator == null || string.IsNullOrEmpty(animationName)) return;

        animator.enabled = true;
        animator.Play(animationName, 0, 0f);
    }

    // Drives a second animator that lives on a different object, so it keeps
    // playing after this one is hidden. Blank fields mean nothing plays.
    void PlayAfterHideAnimation()
    {
        if (afterHideAnimator == null || string.IsNullOrEmpty(afterHideStateName)) return;

        afterHideAnimator.enabled = true;
        afterHideAnimator.Play(afterHideStateName, 0, 0f);
    }

    // Blocks until the clip started by PlayDropAnimation has run to its end.
    // Returns immediately when no clip was requested, so blank-field instances
    // keep deactivating straight away as before.
    IEnumerator WaitForDropAnimation()
    {
        if (animator == null || string.IsNullOrEmpty(animationName)) yield break;
        if (!animator.isActiveAndEnabled) yield break;

        // Let the Play() call above take effect before sampling the state.
        yield return null;

        while (animator.isActiveAndEnabled &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
    }

    IEnumerator RotationtoTarget()
    {
       // yield return new WaitForSeconds(waittime);
        
        float elapsedtime = 0f;
        Quaternion startrot = transform.rotation;
        Quaternion endrot = Quaternion.Euler(Beakerrot);
        while (elapsedtime < Rduration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / Rduration;
            transform.rotation = Quaternion.Slerp(startrot, endrot, t);
            yield return null;
        }

        transform.rotation = endrot;

        // The animator sits on a child of this object, so deactivating here would
        // cut the clip off mid play. Hold until it has finished.
        yield return WaitForDropAnimation();

        gameObject.SetActive(false);

        // Deactivating stops this coroutine at the next yield, but the statements
        // below still run, so the follow up animator is safe to trigger here.
        PlayAfterHideAnimation();

        LessonEvents.RaiseInteractionSettled(LessonContext.lessonFlowController.LFC_CurrentSlide);
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        LessonEvents.RaiseAnimationEnded(LessonContext.lessonFlowController.LFC_CurrentSlide);
    }

}