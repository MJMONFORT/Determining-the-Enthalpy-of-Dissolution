using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int dragID;                 // MANUAL ID
    public Transform originalParent;
    [SerializeField] protected CanvasGroup cg;
    protected Vector3 originalPosition;
    private Canvas canvas;
    public bool droppedCorrectly;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        cg = GetComponent<CanvasGroup>();

        originalParent = transform.parent;
        originalPosition = transform.position;
    }

    public  void OnBeginDrag(PointerEventData eventData)
    {
        droppedCorrectly = false;
        originalPosition = transform.position;

        transform.SetParent(canvas.transform);
        cg.blocksRaycasts = false;
    }

    public  void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        PhysicsRayCast(eventData.position);
        cg.blocksRaycasts = true;
        if (!droppedCorrectly)
        {
            transform.position = originalPosition;
            transform.SetParent(originalParent);
        }
    }

    protected virtual void PhysicsRayCast(Vector3 pos)
    {

    }
}