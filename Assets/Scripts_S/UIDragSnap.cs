using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIDragSnap : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Snap Settings")]
    public RectTransform snapTarget;

    [Header("After Snap")]
    public GameObject textToEnable;   // another text to show after drop

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 startAnchoredPos;
    private Transform startParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        startAnchoredPos = rectTransform.anchoredPosition;
        startParent = transform.parent;

        // Make sure replacement text is hidden initially
        if (textToEnable != null)
            textToEnable.SetActive(false);
    }

    // ---------------- DRAG ----------------

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector3 worldPoint
        );

        rectTransform.position = worldPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (snapTarget != null)
        {
            SnapAndReplace();
        }
        else
        {
            ResetPosition();
        }
    }

    // ---------------- LOGIC ----------------

    private void SnapAndReplace()
    {
        // Snap (optional – visual only)
        rectTransform.position = snapTarget.position;

        // Hide dragged text
        gameObject.SetActive(false);

        // Enable new text
        if (textToEnable != null)
            textToEnable.SetActive(true);
    }

    private void ResetPosition()
    {
        rectTransform.anchoredPosition = startAnchoredPos;
    }
}
