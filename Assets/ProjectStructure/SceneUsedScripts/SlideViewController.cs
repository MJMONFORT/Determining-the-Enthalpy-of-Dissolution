using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class SlideViewController : MonoBehaviour
{

    [Header("Reusable Panels")]
    [SerializeField] PromptPanel promptPanel;
    [SerializeField] PromptPanel negativePanel;
    [SerializeField] PromptPanel timingPanel;
    [SerializeField] PromptPanel infoPanel;

    [Header("Slide Prompt Data")]
    [SerializeField] SlidePromptData[] slidePromptData;

    [SerializeField]int currentSlide = -1;

    Coroutine timingHideRoutine;

    public int TotalSlides => slidePromptData.Length;
    public SlidePromptData[] SlidepromptData { get { return slidePromptData; } }

    void OnEnable()
    {
        LessonEvents.ShowSlide += ShowSlide;
        LessonEvents.PromptIndex += ShowPrompt;
        LessonEvents.NegativePrompts += ShowNegativePrompt;
        LessonEvents.TimingPrompts += ShowTimingPrompt;
        LessonEvents.InfoPrompts += ShowInfoPrompt;
    }

    void OnDisable()
    {
        LessonEvents.ShowSlide -= ShowSlide;
        LessonEvents.PromptIndex -= ShowPrompt;
        LessonEvents.NegativePrompts -= ShowNegativePrompt;
        LessonEvents.TimingPrompts -= ShowTimingPrompt;
        LessonEvents.InfoPrompts -= ShowInfoPrompt;
    }

    private void Start()
    {
        LessonContext.Init_SlideView(this);
    }
    void ShowSlide(int slideIndex)
    {
        currentSlide = slideIndex;

        SetVisible(promptPanel.canvasGroup, false);
      //  SetVisible(negativePanel.canvasGroup, false);

        //if (timingHideRoutine != null)
        //{
        //    StopCoroutine(timingHideRoutine);
        //    timingHideRoutine = null;
        //}
        SetVisible(timingPanel.canvasGroup, false);
        SetVisible(infoPanel.canvasGroup, false);

        ShowPrompt(0);
    }

    void ShowPrompt(int index)
    {
        var data = slidePromptData[currentSlide];

        if(!data.promtenable)
        {
            SetVisible(promptPanel.canvasGroup, false);
            return;
        }
        if (data.prompts.Length <= index) return;
        
        ApplyPanel(promptPanel, data.prompts[index]);
    }

    void ShowNegativePrompt(int index)
    {
        var data = slidePromptData[currentSlide];

        if (data.negativePrompts.Length <= index) return;

        ApplyPanel(negativePanel, data.negativePrompts[index]);
    }

    void ShowInfoPrompt(int index)
    {
        var data = slidePromptData[currentSlide];
        if (data.infoPrompts.Length <= index) return;
        LessonContext.lessonFlowController.slideCompletionState[LessonContext.lessonFlowController.LFC_CurrentSlide] = true;
        LessonEvents.RaiseSetNextButtonState(true);
        ApplyPanel(infoPanel, data.infoPrompts[index]);
    }
    void ShowTimingPrompt(bool show, int index)
    {
        if (!show) return;

        if (timingHideRoutine != null)
            StopCoroutine(timingHideRoutine);

        timingHideRoutine = StartCoroutine(TimingRoutine(index,currentSlide));
    }

    IEnumerator TimingRoutine(int index, int slideId)
    {
        yield return new WaitForSeconds(.5f);
        // ⭐ STOP if slide changed
        if (slideId != currentSlide)
            yield break;

        var data = slidePromptData[currentSlide];
        Debug.Log(data.timingPrompts.Length);
        if (data.timingPrompts.Length <= index)
            yield break;

        ApplyPanel(timingPanel, data.timingPrompts[index]);

        yield return new WaitForSeconds(2f);
        // ⭐ STOP if slide changed again
        if (slideId != currentSlide)
            yield break;
        SetVisible(timingPanel.canvasGroup, false);
    }

    public int PropmtLength()
    {
        var data = slidePromptData[currentSlide];
        return data.prompts.Length;
    }

    public int TimingPromptLength()
    {
        var data = slidePromptData[currentSlide];
        return data.timingPrompts.Length;
    }
    void ApplyPanel(PromptPanel panel, PromptUIData data)
    {
        panel.textTMP.text = data.text;
        panel.background.sizeDelta = data.size;

        // Stretch text to fill the background with the panel's padding.
        var rt = panel.textTMP.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(panel.padLeft, panel.padBottom);
        rt.offsetMax = new Vector2(-panel.padRight, -panel.padTop);

        SetVisible(panel.canvasGroup, true);
    }

    void SetVisible(CanvasGroup cg, bool visible)
    {
        cg.alpha = visible ? 1 : 0;
        cg.blocksRaycasts = visible;
        cg.interactable = visible;
    }
}

[System.Serializable]
public class PromptPanel
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textTMP;
    public RectTransform background;

    [Header("Inner Text Padding")]
    public float padLeft = 25f;
    public float padRight = 25f;
    public float padTop = 25f;
    public float padBottom = 25f;
}

[System.Serializable]
public class PromptUIData
{
    [TextArea] public string text;
    public Vector2 size;
}

[System.Serializable]
public class SlidePromptData
{
    public bool promtenable;

    public PromptUIData[] prompts;
    public PromptUIData[] negativePrompts;
    public PromptUIData[] timingPrompts;
    public PromptUIData[] infoPrompts;
}
