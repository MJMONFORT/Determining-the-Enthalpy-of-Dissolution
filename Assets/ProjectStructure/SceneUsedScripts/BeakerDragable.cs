using UnityEngine;
using System.Collections;


public class BeakerDragable : MonoBehaviour, IWorldDraggable
{
    float duration = .5f;
    [SerializeField] protected string id;
    [SerializeField] protected string animationName;
    [SerializeField] protected float dropTargetdist;
    public string Id { get { return id; } }

    private bool isinteractable = false;
    public bool IsInteractable { get { return isinteractable; } }

    [Tooltip("Positive = hold further from the camera, negative = closer. 0 keeps the pickup depth.")]
    [SerializeField] protected float depthOffset = 0f;
    public float DepthOffset { get { return depthOffset; } }

    protected Coroutine attachC, notattachC;
    [SerializeField] protected float dragSmooth = 15f;
    protected bool isDragging;
    [SerializeField] protected Transform camTrans;
    [SerializeField] private GameObject[] go;
    [SerializeField] protected Vector3  BeakerPos;
    [SerializeField] protected SetBool isThisMicroScope; 
    public enum SetBool { ismicro, notmicro};

    void OnEnable()
    {
        isinteractable = true;
    }
    protected virtual void CoroutineToTarget(Transform attachtransform)
    {
        if (attachC != null) { StopCoroutine(attachC); }
        if (notattachC != null) { StopCoroutine(notattachC); notattachC = null; }
        attachC = StartCoroutine(PositoinToTarget(attachtransform));
    }
    protected void CoroutineBackToNorma(Vector3 pos, Quaternion rot)
    {
        if (notattachC != null) { StopCoroutine(notattachC);}
        if (attachC != null) { StopCoroutine(attachC); attachC = null; }
        notattachC = StartCoroutine(PositionRotationBackToNormal(pos, rot));

    }
    public virtual void Attach(Transform attachTrans)
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

        Vector3 target = worldPosition;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * dragSmooth);
    }

    public void EndDrag()
    {

    }

    public void Notatach(Vector3 pos, Quaternion rot)
    {
        isDragging = false;
        CoroutineBackToNorma(pos, rot);
    }

    protected virtual IEnumerator PositoinToTarget(Transform attachtransform)
    {
        float elapsedtime = 0f;
       // transform.SetParent(null);
        Vector3 startpos = transform.position;
        Vector3 endpos = BeakerPos;

        while (elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            float t = elapsedtime / duration;
            transform.position = Vector3.Lerp(startpos, endpos, t);
            yield return null;
        }
        transform.position = endpos;

        LessonEvents.RaiseInteractionSettled(LessonContext.lessonFlowController.LFC_CurrentSlide);
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        if (transform.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = true;
            LessonContext.microscopeSLide = true;
            if (isThisMicroScope == SetBool.ismicro)
            {
                if (Vector3.Distance(startpos, endpos) < dropTargetdist)
                {
                    this.gameObject.SetActive(false);
                    go[0].SetActive(true);
                    if (go[0].activeSelf && LessonContext.animator != null && !string.IsNullOrEmpty(animationName))
                    {
                        LessonContext.animator.Play(animationName);
                    }

                }
            }
        }
    }

    protected IEnumerator PositionRotationBackToNormal(Vector3 pos, Quaternion rot)
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
       // transform.SetParent(null);
        transform.position = target;
        if (transform.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = true;
        }
    }
    void OnDisable()
    {
        isinteractable = false;
    }
}
