using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class FlowerPlucker : ClickController
{
    [SerializeField] Transform camTrans;
    [SerializeField] Vector3 originalPos;
    [SerializeField] Quaternion originalRotation;
    [SerializeField] float duration, delay;
    Coroutine cFM, cM;
    protected override void OnEnable()
    {
        base.OnEnable();
       originalPos = transform.position;
       originalRotation = transform.rotation;
    }
    protected override void Click(InputAction.CallbackContext context)
    {
        Ray ray = LessonContext.modelDragController.RaycastCamera.ScreenPointToRay(LessonContext.modelDragController.PointerPosition());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
        {
            CoroutineFM();
            LessonEvents.RaiseShowTimingPrompts(true, 0);
            LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
            LessonEvents.RaiseSetNextButtonState(true);
        }
    }
    void CoroutineFM()
    {
        if (cFM != null) { StopCoroutine(cFM); }
        cFM = StartCoroutine("MoveToTarget");
    }
    IEnumerator MoveToTarget()
    {
       
        //offset = transform.position - hitPoint;
        float elapsedtime = 0f;
        Vector3 position = transform.position;
        Vector3 target = camTrans.position;
        Quaternion startrot = transform.rotation;
        Quaternion rotation = camTrans.rotation;
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.rotation = Quaternion.Slerp(startrot, rotation, t);
            transform.position = Vector3.Slerp(position, target, t);
            yield return null;

        }

        transform.position = target;
        if (transform.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(delay);
        CoroutineM();
    }

    void CoroutineM()
    {
        if (cM != null) { StopCoroutine(cM); }
        cM = StartCoroutine("MoveToOriginal");
    }
    IEnumerator MoveToOriginal()
    {
        float elapsedtime = 0f;
        Vector3 position = transform.position;
        Vector3 target = originalPos;
        Quaternion startrot = transform.rotation;
        Quaternion rotation = originalRotation;
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.rotation = Quaternion.Slerp(startrot, rotation, t);
            transform.position = Vector3.Slerp(position, target, t);
            yield return null;

        }

        transform.position = target;
        if (transform.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = true;
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}