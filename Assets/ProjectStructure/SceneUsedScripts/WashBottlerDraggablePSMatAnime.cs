using UnityEngine;
using System.Collections;
public class WashBottlerDraggablePSMatAnime : WashBottleDraggable
{
    [SerializeField] Animator anim;
    [SerializeField] protected ParticleSystem waterPour;
    [SerializeField] Renderer waterMaterial;
    [SerializeField] Material mat;
    float duration = .5f;
    float currfluidamount;
    [SerializeField] float fillamount, fillduration = 1f;
    enum ChargeOfValue { Pve, Nve  };
    enum SpaceOfObj { Local,World};

    [SerializeField] ChargeOfValue chargeOfValue;
    [SerializeField] SpaceOfObj spaceOfObj;
    void Awake()
    {
        mat = waterMaterial.material;
    }

    protected override IEnumerator PositoinRotateToTarget(Transform attachtransform)
    {
        switch(spaceOfObj)
        {

            case SpaceOfObj.World :

                switch (chargeOfValue)
                {
                    case ChargeOfValue.Pve :
                        { 
                        float elapsedtime = 0f;
                        //transform.SetParent(null);
                        Quaternion startrot = transform.rotation;
                        Quaternion endrot = attachtransform.rotation;
                        // offset = new Vector3(.005f, 0.192f, 0.389f);
                        Vector3 startpos = transform.position;
                        Vector3 endpos = attachtransform.position + offset;
                        while (elapsedtime < duration)
                        {
                            elapsedtime += Time.deltaTime;
                            float t = elapsedtime / duration;
                            transform.position = Vector3.Slerp(startpos, endpos, t);
                            transform.rotation = Quaternion.Slerp(startrot, endrot, t);
                            yield return null;
                        }
                        CoroutineWash();
                        }
                        break;
                    case ChargeOfValue.Nve:
                        { 
                        float elapsedtime = 0f;
                        //transform.SetParent(null);
                        Quaternion startrot = transform.rotation;
                        Quaternion endrot = attachtransform.rotation;
                        // offset = new Vector3(.005f, 0.192f, 0.389f);
                        Vector3 startpos = transform.position;
                        Vector3 endpos = attachtransform.position - offset;
                        while (elapsedtime < duration)
                        {
                            elapsedtime += Time.deltaTime;
                            float t = elapsedtime / duration;
                            transform.position = Vector3.Slerp(startpos, endpos, t);
                            transform.rotation = Quaternion.Slerp(startrot, endrot, t);
                            yield return null;
                        }
                        CoroutineWash();
                        }
                        break;
                }
            break;

            case SpaceOfObj.Local:

                switch (chargeOfValue)
                {
                    case ChargeOfValue.Pve:
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
                            CoroutineWash();
                        }
                        break;
                    case ChargeOfValue.Nve:
                        {
                            float elapsedtime = 0f;
                            //transform.SetParent(null);
                            Quaternion startrot = transform.localRotation;
                            Quaternion endrot = attachtransform.localRotation;
                            // offset = new Vector3(.005f, 0.192f, 0.389f);
                            Vector3 startpos = transform.position;
                            Vector3 endpos = attachtransform.position - offset;
                            while (elapsedtime < duration)
                            {
                                elapsedtime += Time.deltaTime;
                                float t = elapsedtime / duration;
                                transform.position = Vector3.Slerp(startpos, endpos, t);
                                transform.localRotation = Quaternion.Slerp(startrot, endrot, t);
                                yield return null;
                            }
                            CoroutineWash();
                        }
                        break;
                }

                break;

        }
       
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
    }

    protected override IEnumerator AfterWash()
    {
        yield return base.AfterWash();
        anim.Play("CardboardDown");
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
    }
}

