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
    protected override void CoroutineToTarget(Transform attachtransform)
    {
        base.CoroutineToTarget(attachtransform);
        animationName = animatinonameofname;
    }
    void CoroutineRotatePour()
    {
        if (rotateToPour != null) { StopCoroutine(rotateToPour); }
        rotateToPour = StartCoroutine("RotationtoTarget");
    }



    protected override IEnumerator PositoinToTarget(Transform attachtransform)
    {

        float elapsedtime = 0f;
        // transform.SetParent(null);
        Vector3 startpos = transform.position;
        Vector3 endpos = BeakerPos;
        Debug.Log(endpos);
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
            CoroutineRotatePour();
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
        gameObject.SetActive(false);
        LessonEvents.RaiseInteractionSettled(LessonContext.lessonFlowController.LFC_CurrentSlide);
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        LessonEvents.RaiseAnimationEnded(LessonContext.lessonFlowController.LFC_CurrentSlide);
    }

}