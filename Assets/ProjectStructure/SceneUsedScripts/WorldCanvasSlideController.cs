using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasSlideController : MonoBehaviour
{
    [Header("Canvas Activation Per Slide")]
    [SerializeField] private CanvasActivationData[] canvasData;

    [Header("Dropdown Activation Per Slide")]
    [SerializeField] private DropdownActivationData[] dropdownData;

    Dictionary<int, Canvas[]> canvasMap = new Dictionary<int, Canvas[]>();
    Dictionary<int, GameObject[]> dropdownMap = new Dictionary<int, GameObject[]>();

    HashSet<Canvas> allCanvases = new HashSet<Canvas>();
    HashSet<GameObject> allDropdowns = new HashSet<GameObject>();

    int currentSlide = -1;

    void Awake()
    {
        // Build canvas map
        foreach (var data in canvasData)
        {
            canvasMap[data.slideIndex] = data.canvasesToEnable;

            foreach (var c in data.canvasesToEnable)
            {
                if (c != null)
                    allCanvases.Add(c);
            }
        }

        // Build dropdown map
        foreach (var data in dropdownData)
        {
            dropdownMap[data.slideIndex] = data.dropdownsToEnable;

            foreach (var go in data.dropdownsToEnable)
            {
                if (go != null)
                    allDropdowns.Add(go);
            }
        }
    }

    void OnEnable()
    {
        LessonEvents.ShowSlide += HandleSlide;
    }

    void OnDisable()
    {
        LessonEvents.ShowSlide -= HandleSlide;
    }

    // ⭐ CORE LOGIC
    void HandleSlide(int slideIndex)
    {
        currentSlide = slideIndex;

        ToggleCanvases(slideIndex);
        ToggleDropdowns(slideIndex);
    }

    // ---------------- CANVAS CONTROL ----------------
    void ToggleCanvases(int slideIndex)
    {
        // Disable ALL canvases first (no GPU draw)
        foreach (var canvas in allCanvases)
        {
            if (canvas != null && canvas.enabled)
                canvas.enabled = false;
        }

        // Enable only required canvases
        if (canvasMap.TryGetValue(slideIndex, out var list))
        {
            foreach (var canvas in list)
            {
                if (canvas != null)
                    canvas.enabled = true;
            }
        }
    }

    // ---------------- DROPDOWN CONTROL ----------------
    void ToggleDropdowns(int slideIndex)
    {
        // Disable all dropdown GameObjects
        foreach (var go in allDropdowns)
        {
            if (go != null && go.activeSelf)
                go.SetActive(false);
        }

        // Enable only required dropdowns
        if (dropdownMap.TryGetValue(slideIndex, out var list))
        {
            foreach (var go in list)
            {
                if (go != null)
                    go.SetActive(true);
            }
        }
    }
}


[System.Serializable]
public class CanvasActivationData
{
    public int slideIndex;
    public Canvas[] canvasesToEnable;
}

[System.Serializable]
public class DropdownActivationData
{
    public int slideIndex;
    public GameObject[] dropdownsToEnable;
}
