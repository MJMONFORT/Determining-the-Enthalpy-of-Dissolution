using UnityEngine;
using UnityEngine.UI;
public class SafeAreaHandler : MonoBehaviour
{
    [SerializeField] CanvasScaler canvasScaler;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ApplySafeArea();
        AdjustCanvasMatch();
    }
    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin /= screenSize;
        anchorMax /= screenSize;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }

    void AdjustCanvasMatch()
    {
        if (canvasScaler == null) return;

        float aspect = (float)Screen.width / Screen.height;
        Debug.Log(aspect);
        // Portrait → match width
        if (aspect < 1.5f)
        {
            canvasScaler.matchWidthOrHeight = 0.2f;
        }
        // Landscape → match height
        else
        {
            canvasScaler.matchWidthOrHeight = .6f;
        }
            
    }
            
 
}
