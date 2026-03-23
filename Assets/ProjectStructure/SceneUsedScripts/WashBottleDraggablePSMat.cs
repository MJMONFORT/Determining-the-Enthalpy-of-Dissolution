using UnityEngine;
using System.Collections;

public class WashBottleDraggablePSMat : WashBottleDraggable
{
    [SerializeField] protected ParticleSystem waterPour;
    [SerializeField] Renderer waterMaterial;
    [SerializeField] Material mat;
    float currfluidamount;
    [SerializeField] float fillamount,fillduration = 1f;
    float duration = .5f;

    void Awake()
    {
        mat = waterMaterial.material;
    }

    protected override IEnumerator PositoinRotateToTarget(Transform attachtransform)
    {
        float elapsedtime = 0f;
        //transform.SetParent(null);
        Quaternion startrot = transform.localRotation;
        Quaternion endrot = attachtransform.localRotation;
        // offset = new Vector3(.005f, 0.192f, 0.389f);
        Vector3 startpos = transform.localPosition;
        Vector3 endpos = attachtransform.localPosition + offset;
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.localPosition = Vector3.Slerp(startpos, endpos, t);
            transform.localRotation = Quaternion.Slerp(startrot, endrot, t);
            yield return null;
        }
        base.CoroutineWash();
    }
    protected override IEnumerator WashRotation()
    {
      float startval = mat.GetFloat("_FillHeight");
      float endval = fillamount;
        LessonContext.lessonFlowController.ShowCurrentPromptIteratted();
      yield return base.WashRotation();
      waterPour.Play();
        if (waterPour.isPlaying)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fillduration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fillduration;
                currfluidamount = Mathf.Lerp(startval, endval, t);
                mat.SetFloat("_FillHeight", currfluidamount);
                yield return null;
            }
            mat.SetFloat("_FillHeight", endval);
            waterPour.Stop();   
        }
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
    }
}
