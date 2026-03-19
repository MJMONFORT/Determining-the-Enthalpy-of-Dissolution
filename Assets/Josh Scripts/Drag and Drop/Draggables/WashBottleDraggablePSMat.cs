using UnityEngine;
using System.Collections;

public class WashBottleDraggablePSMat : WashBottleDraggable
{
    [SerializeField] protected ParticleSystem waterPour;
    [SerializeField] Renderer waterMaterial;
    [SerializeField] Material mat;
    float currfluidamount;
    [SerializeField] float fillamount,fillduration = 1f;

    void Awake()
    {
        mat = waterMaterial.material;
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
