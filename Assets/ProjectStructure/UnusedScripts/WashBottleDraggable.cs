using UnityEngine;
using System.Collections;


public class WashBottleDraggable : MonoBehaviour, IWorldDraggable
{
    [SerializeField] private string id;
    public string Id { get { return id; } }
    private bool isinteractable = false;
    public bool IsInteractable { get { return isinteractable; } }

    [Tooltip("Positive = hold further from the camera, negative = closer. 0 keeps the pickup depth.")]
    [SerializeField] protected float depthOffset = 0f;
    public float DepthOffset { get { return depthOffset; } }

    [SerializeField] float dragSmooth = 15f;
    float duration = .5f;
    int beakerIndex;
    bool isDragging;
    [SerializeField] Transform[] beakers;
    [SerializeField] protected Transform camTargetTrans,BeakerTargetTrans;

    Coroutine attachC, notattachC, washC, afterwashC, backtoPosaAfterWashC, beakermoveC;
    protected Vector3 washbottelposition;
    protected Vector3 init_washbottlerotation;
    [SerializeField] protected Vector3 offset, trgt_washbottlerotation, after_washbottlerotation;
    
    void CoroutineToTarget(Transform attachtransform)
    {
         if (attachC != null) { StopCoroutine(attachC); }
        if (notattachC != null) { StopCoroutine(notattachC); notattachC = null; }

        attachC = StartCoroutine(PositoinRotateToTarget(attachtransform));
    }
    void CoroutineBackToNorma(Vector3 pos, Quaternion rot)
    {
        if (notattachC != null) { StopCoroutine(notattachC); }
        if (attachC != null) { StopCoroutine(attachC); attachC = null; }

        notattachC = StartCoroutine(PositionRotationBackToNormal(pos, rot));

    }

    protected void CoroutineWash()
    {
        if(washC != null) { StopCoroutine(washC);}
        
        washC = StartCoroutine("WashRotation");
    }

    void CoroutineAfterWash()
    {
        if(afterwashC != null) { StopCoroutine (afterwashC);}
       
        afterwashC = StartCoroutine("AfterWash");
    }

    void CoroutineBackToPosAfterWash()
    {
        if(backtoPosaAfterWashC != null) { StopCoroutine(backtoPosaAfterWashC); }
       
        backtoPosaAfterWashC = StartCoroutine("BackToPosAferWash");
    }

    void CoroutineBeakerMove()
    {
        if (beakermoveC != null) { StopCoroutine(beakermoveC); }
       
        beakermoveC = StartCoroutine("MoveBeaker");
    }
    void OnEnable()
    {
        isinteractable = true;
        washbottelposition = transform.position;
        init_washbottlerotation = transform.rotation.eulerAngles;
    }
    public void Attach(Transform attachTrans)
    {
        isDragging = false;
        CoroutineToTarget(attachTrans);
    }

    public void BeginDrag(Vector3 hitPoint)
    {
        isDragging = true;

        if (transform.TryGetComponent<Collider>(out Collider col))
            col.enabled = false;
    }

    public void Drag(Vector3 worldPosition)
    {
        if (!isDragging) return;

        // Smooth movement (no jitter)
        transform.position = Vector3.Lerp(
            transform.position,
            worldPosition,
            Time.deltaTime * dragSmooth
        );
    }

    public void EndDrag()
    {
       
    }

    public void Notatach(Vector3 pos, Quaternion rot)
    {
        isDragging = false;
        CoroutineBackToNorma(pos, rot);
    }

    protected virtual IEnumerator PositoinRotateToTarget(Transform attachtransform)
    {
        float elapsedtime = 0f;
        //transform.SetParent(null);
        Quaternion startrot = transform.localRotation;
        Quaternion endrot = attachtransform.localRotation;
        // offset = new Vector3(.005f, 0.192f, 0.389f);
        Vector3 startpos = transform.localPosition;
        Vector3 endpos = attachtransform.localPosition - offset;
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

    IEnumerator PositionRotationBackToNormal(Vector3 pos, Quaternion rot)
    {
        float elapsedtime = 0f;
        Vector3 position = transform.position;
        Vector3 target = pos;
        Quaternion startrot = transform.rotation;
        Quaternion rotation = rot;
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.rotation = Quaternion.Slerp(startrot, rotation, t);
            transform.position = Vector3.Slerp(position, target, t);
            yield return null;

        }
      //  transform.SetParent(null);
       // if (Vector3.Distance(position, target) < .9f)
       // {
            transform.position = target;
            if (transform.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = true;
            }
    }

    protected virtual IEnumerator WashRotation()
    {
        float elapsedTime = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(trgt_washbottlerotation); //Quaternion.Euler(0f, 87.6f, -45f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        transform.rotation = targetRot;
        CoroutineAfterWash();
        LessonContext.cineMachineFlowController.SwitchNextCameraInSlide();
    }

   protected virtual IEnumerator AfterWash()
    {
        yield return new WaitForSeconds(2f);
        float elapsedTime = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(after_washbottlerotation); //Quaternion.Euler(180f, 0f, 180f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        transform.rotation = targetRot;
        CoroutineBackToPosAfterWash();
    }

    IEnumerator BackToPosAferWash()
    {
       
        yield return new WaitForSeconds(.5f);
        float elapsedtime = 0f;
        Vector3 position = transform.position;
        Vector3 target = washbottelposition;
        Quaternion startrot = transform.rotation;
        Quaternion rotation = Quaternion.Euler(init_washbottlerotation);
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.rotation = Quaternion.Slerp(startrot, rotation, t);
            transform.position = Vector3.Slerp(position, target, t);
            yield return null;

        }
    
        if(Vector3.Distance(position,target) < .9)
        {
            transform.position = target;
            if (transform.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = true;
                CoroutineBeakerMove();
            }
        }
    }

    IEnumerator MoveBeaker()
    {
        Transform beakerTrans;
        Debug.Log(beakerIndex);
        //OTHER PROJECTS SCRIPT CHANGE NEEDED TO MAINTAIN SAME SCRIPTS TO ALL PROJECTS
        if (beakers.Length > 0)
        {
            beakerTrans = beakers[beakerIndex];
            float elapsedtime = 0f;
            Vector3 position = beakerTrans.position;
            Vector3 target = BeakerTargetTrans.position;
            while (elapsedtime < duration)
            {
                elapsedtime += Time.deltaTime;
                float t = elapsedtime / duration;
                beakerTrans.position = Vector3.Slerp(position, target, t);
                yield return null;
            }
            beakerTrans.position = target;
            beakerIndex++;
            if (Vector3.Distance(position, target) < .9f)
            {
                beakerTrans.gameObject.SetActive(false);
            }
        }
    }
    void OnDisable()
    {
        isinteractable = false;
    }
}
