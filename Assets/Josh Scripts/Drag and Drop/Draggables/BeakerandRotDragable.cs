using UnityEngine;
using System.Collections;
public class BeakerandRotDragable : BeakerDragable
{
    float Rduration = 1f;
    [SerializeField] float rotTargetangle;
    Quaternion Beakerrot;
    [SerializeField]Vector3 beakernewpos;
    Coroutine rotbackC,rotateToPour;
    void CoroutineRotBack()
    {
        if (rotbackC != null) { StopCoroutine(rotbackC); }
        rotbackC = StartCoroutine("RotationandPosBack");
        animationName = "CardboardDown";
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
            transform.position = Vector3.Lerp(startpos, endpos, t);;
            yield return null;
        }

        transform.position = endpos;
        if (Vector3.Distance(startpos, endpos) < dropTargetdist)
        {
           LessonContext.lessonFlowController.ShowCurrentPromptIteratted();
           LessonContext.animator.Play(animationName);
           CoroutineRotatePour();
        }
    }

    IEnumerator RotationtoTarget()
    {
        Quaternion quaternion = Quaternion.Euler(0, 0, 86f);
        Beakerrot = quaternion;
        yield return new WaitForSeconds(1f);

        float elapsedtime = 0f;
        Quaternion startrot = transform.rotation;
        Quaternion endrot = Beakerrot;
        while (elapsedtime < Rduration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / Rduration;
            transform.rotation = Quaternion.Slerp(startrot, endrot, t);
            yield return null;
        }

        transform.rotation = endrot;

        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
            if (Quaternion.Angle(startrot, endrot) > rotTargetangle)
            {

                CoroutineRotBack();
            }
    }
    IEnumerator RotationandPosBack()
    {
        Quaternion quaternion = Quaternion.Euler(0, 180f, 0);
        Beakerrot = quaternion;
        yield return new WaitForSeconds(1f);
        LessonContext.animator.Play(animationName);
        float elapsedtime = 0f;
        Vector3 startpos = transform.position;
        Vector3 endpos = beakernewpos;
        Quaternion startrot = transform.rotation;
        Quaternion endrrot = Beakerrot;
        while (elapsedtime < Rduration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / Rduration;
            transform.position = Vector3.Slerp(startpos, endpos, t);
            transform.rotation = Quaternion.Slerp(startrot, endrrot, t);
            yield return null;
        }
        transform.position = endpos;
        transform.rotation = endrrot;

    }
}
