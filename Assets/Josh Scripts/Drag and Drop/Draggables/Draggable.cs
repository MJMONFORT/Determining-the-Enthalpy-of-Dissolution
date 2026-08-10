using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour, IWorldDraggable
{
    [Header("Identity")]
    [SerializeField] string id;
    [SerializeField] bool isinteractable;

    [Header("Drag Settings")]
    [SerializeField] float dragSmooth = 15f;
    [SerializeField] Vector3 camHoldOffset;
    [SerializeField] float dropDuration = 0.5f;
    [SerializeField] float returnDuration = 0.5f;

    [Header("Drop")]
    [SerializeField] bool reparentOnDrop;
    [SerializeField] bool rotateOnDrop;
    [SerializeField] Vector3 dropRotationEuler;
    [SerializeField] bool dropRotationRelativeToZone;

    [Header("Collider")]
    [SerializeField] bool disableColliderOnPickup = true;
    [SerializeField] bool enableColliderOnDrop = true;

    public string Id => id;
    public bool IsInteractable => isinteractable;
    public Vector3 DragStartPos => dragStartPos;
    public Quaternion DragStartRot => dragStartRot;
    public bool HasCamOffset => camHoldOffset != Vector3.zero;
    public Vector3 CamHoldOffset => camHoldOffset;

    bool isDragging;
    bool skipDropLerp;
    int originalLayer;
    Collider col;
    Coroutine activeRoutine;
    Vector3 dragStartPos;
    Quaternion dragStartRot;

    void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void SetInteractable(bool state, int layer)
    {
        isinteractable = state;
        gameObject.layer = layer;
    }

    public void BeginDrag(Vector3 hitPoint)
    {
        dragStartPos = transform.position;
        dragStartRot = transform.rotation;
        isDragging = true;

        if (disableColliderOnPickup && col != null)
            col.enabled = false;

        FireDragBegin();
    }

    public void Drag(Vector3 worldPosition)
    {
        if (!isDragging) return;
        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * dragSmooth);
    }

    public void EndDrag() { }

    public void SkipDrop()
    {
        skipDropLerp = true;
    }

    public void Attach(Transform zoneTrans)
    {
        isDragging = false;
        StopActive();
        if (skipDropLerp)
        {
            skipDropLerp = false;
            return;
        }
        activeRoutine = StartCoroutine(DropLerp(zoneTrans));
    }

    public void Notatach(Vector3 pos, Quaternion rot)
    {
        isDragging = false;
        StopActive();
        activeRoutine = StartCoroutine(ReturnLerp(pos, rot));
    }

    IEnumerator DropLerp(Transform zoneTrans)
    {
        if (reparentOnDrop && zoneTrans != null)
            transform.SetParent(zoneTrans.parent, true);

        Vector3 startPos = transform.position;
        Vector3 endPos = zoneTrans.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = ComputeDropRotation(zoneTrans);

        float elapsed = 0f;
        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            if (rotateOnDrop)
                transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.position = endPos;
        if (rotateOnDrop) transform.rotation = endRot;

        if (enableColliderOnDrop && col != null)
            col.enabled = true;


        FireDropSuccess();
    }

    IEnumerator ReturnLerp(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float elapsed = 0f;
        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        if (col != null)
            col.enabled = true;

        FireDropReturn();
    }

    Quaternion ComputeDropRotation(Transform zoneTrans)
    {
        if (!rotateOnDrop) return transform.rotation;

        if (dropRotationRelativeToZone && zoneTrans != null)
            return zoneTrans.rotation * Quaternion.Euler(dropRotationEuler);

        return Quaternion.Euler(dropRotationEuler);
    }

    void FireDragBegin()
    {
        var callbacks = GetComponents<IOnDragBegin>();
        for (int i = 0; i < callbacks.Length; i++)
        {
            var mb = callbacks[i] as MonoBehaviour;
            if (mb != null && !mb.enabled) continue;
            callbacks[i].Execute();
        }
    }

    void FireDropSuccess()
    {
        var callbacks = GetComponents<IOnDropSuccess>();
        for (int i = 0; i < callbacks.Length; i++)
        {
            var mb = callbacks[i] as MonoBehaviour;
            if (mb != null && !mb.enabled) continue;
            callbacks[i].Execute();
        }
    }

    void FireDropReturn()
    {
        var callbacks = GetComponents<IOnDropReturn>();
        for (int i = 0; i < callbacks.Length; i++)
        {
            var mb = callbacks[i] as MonoBehaviour;
            if (mb != null && !mb.enabled) continue;
            callbacks[i].Execute();
        }
    }

    void StopActive()
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);
    }
}