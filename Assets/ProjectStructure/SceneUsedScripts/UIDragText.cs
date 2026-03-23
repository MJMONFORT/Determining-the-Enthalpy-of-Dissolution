using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragText : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform bigTextrectTransform;
    private Canvas canvas;
    public CanvasGroup bigTextcanvasGroup;

    private Vector3 originalPosition;
    public bool droppedCorrectly;

    [SerializeField] private float dragScaleMultiplier = 1.2f;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        droppedCorrectly = false;
        originalPosition = bigTextrectTransform.position;
        bigTextcanvasGroup.alpha = 1f;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector3 worldPoint
        );

        bigTextrectTransform.position = worldPoint;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (!droppedCorrectly)
        {
            bigTextrectTransform.position = originalPosition;
            bigTextcanvasGroup.alpha = 0f;
        }
    }
}
