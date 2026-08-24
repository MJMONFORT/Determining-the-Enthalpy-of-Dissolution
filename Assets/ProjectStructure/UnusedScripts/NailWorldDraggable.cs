using UnityEngine;
using System.Collections;

public class NailWorldDraggable : MonoBehaviour,IWorldDraggable
{
    [SerializeField] private string id;
    public string Id { get { return id; } }

    private bool isinteractable = false;
   
    public bool IsInteractable { get { return isinteractable; } }

    [Tooltip("Positive = hold further from the camera, negative = closer. 0 keeps the pickup depth.")]
    [SerializeField] float depthOffset = 0f;
    public float DepthOffset { get { return depthOffset; } }

    float duration = .2f;
    [SerializeField] Transform camTargetPos/*, parenTrans*/;
    Coroutine nailrot,nailtoTarget,nailbacktodefault;
    void CoroutineStart()
    {
        if(nailrot != null) { StopCoroutine(nailrot); }

        nailrot = StartCoroutine("PositionRotationLerpTowardsCam");
    }

    void CoroutineToTarget(Transform attachtransform)
    {
        if(nailtoTarget != null) { StopCoroutine (nailtoTarget); }
        nailtoTarget = StartCoroutine(PositoinRotateToTarget(attachtransform));
    }

    void CoroutineBackToNorma(Vector3 pos, Quaternion rot)
    {
        if (nailbacktodefault != null) { StopCoroutine(nailbacktodefault); }
        nailbacktodefault = StartCoroutine(PositionRotationBackToNormal(pos,rot));

    }
    public void BeginDrag(Vector3 hit)
    {
        CoroutineStart();
    }

    public void Drag(Vector3 pos)
    {
        camTargetPos.transform.position = pos;
    }

    public void EndDrag() 
    {
   
    }

    IEnumerator PositoinRotateToTarget(Transform attachtransform)
    {
        float elapsedtime = 0f;
        transform.SetParent(null);
        Quaternion startrot = transform.rotation;
        Quaternion endrot = attachtransform.rotation;
        Vector3 offset = new Vector3(0, .03f, 0);
        Vector3 startpos = transform.position;
        Vector3 endpos = attachtransform.position +  offset;
        
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.rotation = Quaternion.Slerp(startrot, endrot, t);
            transform.position = Vector3.Slerp(startpos, endpos, t);
            yield return null;
        }
       // Debug.Log(Vector3.Distance(startpos, endpos));

        if(Vector3.Distance(startpos, endpos) < .6f)
        {
            transform.SetParent(attachtransform.parent.transform);
        }
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
        transform.SetParent(null);
        if (Vector3.Distance(position, target) < .9f)
        {
            transform.position = target;
            if (transform.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = true;
            }
        }
    }
    IEnumerator PositionRotationLerpTowardsCam()
    {
        float elapsedtime = 0f;
        Vector3 position = transform.position;
        Vector3 target = camTargetPos.position;
        Quaternion startrot = transform.rotation;
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.rotation = Quaternion.Slerp(startrot,rotation, t);
            transform.position = Vector3.Slerp(position, target, t);
            yield return null;
            
        }
        if(Vector3.Distance(position, target) < .9f)
        {
            transform.SetParent(camTargetPos);
            transform.localPosition = Vector3.zero;
            if(transform.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }
        }
    }

    public void Attach(Transform attchtransform)
    {
        CoroutineToTarget(attchtransform);
        LessonContext.naildropcount++;
        if(LessonContext.naildropcount == 2)
        {
            LessonContext.lessonFlowController.ShowCurrentPromptIteratted();
        }
    }

    public void Notatach(Vector3 pos, Quaternion rot)
    {
        CoroutineBackToNorma(pos,rot);
    }
}
