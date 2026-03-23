using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WorldToScreenDrag: MonoBehaviour
{
    [Header("World Space")]
    public TextMeshProUGUI worldText;        // Drag source (assign in Inspector)

    [Header("Screen Space")]
    public Canvas screenCanvas;              // Screen Space Canvas
    public RectTransform snapPoint;          // Drop location
    public TextMeshProUGUI screenText;       // Screen-space text (disabled)

    [Header("Settings")]
    public float snapDistance = 80f;

    private CanvasGroup worldCanvasGroup;
    private bool dragging;
    private bool dropped;

    void Start()
    {
        if (worldText == null)
        {
            Debug.LogError("World Text not assigned!");
            enabled = false;
            return;
        }

        worldCanvasGroup = worldText.GetComponent<CanvasGroup>();
        if (worldCanvasGroup == null)
            worldCanvasGroup = worldText.gameObject.AddComponent<CanvasGroup>();

        screenText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (dropped) return;

        // MOUSE / TOUCH DOWN
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverWorldText())
            {
                StartDrag();
            }
        }

        // DRAG
        if (dragging)
        {
            UpdateScreenTextPosition();

            // RELEASE
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }

    void StartDrag()
    {
        dragging = true;
        worldCanvasGroup.blocksRaycasts = false;

        screenText.text = worldText.text;
        screenText.gameObject.SetActive(true);

        UpdateScreenTextPosition();
    }

    void EndDrag()
    {
        dragging = false;
        worldCanvasGroup.blocksRaycasts = true;

        float dist = Vector2.Distance(
            screenText.rectTransform.position,
            snapPoint.position
        );

        if (dist <= snapDistance)
        {
            // SNAP SUCCESS
            screenText.rectTransform.position = snapPoint.position;
            dropped = true;

            worldText.enabled = false;
        }
        else
        {
            // CANCEL
            screenText.gameObject.SetActive(false);
        }
    }

    void UpdateScreenTextPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            screenCanvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPos
        );

        screenText.rectTransform.localPosition = localPos;
    }

    bool IsPointerOverWorldText()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        foreach (var r in results)
        {
            if (r.gameObject == worldText.gameObject)
                return true;
        }

        return false;
    }
}
