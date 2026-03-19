using TMPro;
using UnityEngine;
using System.Collections.Generic;
public class LessonFlowController : MonoBehaviour
{

    [SerializeField] private SlideData[] Slides_SO;
    [SerializeField] int currentSlide;
    [SerializeField] int currentPromptIndex,currentTimingPromptIndex;
    [SerializeField] TextMeshProUGUI textGUI;
    [SerializeField] string pageNum;

    HashSet<int> H_dragAllowed = new HashSet<int>();
    Dictionary<int, PromptData> D_Slides_SO = new Dictionary<int, PromptData>();

    public Dictionary<int, bool> slideCompletionState = new Dictionary<int, bool>();

    public int LFC_CurrentSlide { get { return currentSlide; } }
    public int LFC_CurrentTimingPromptIndex { get { return currentTimingPromptIndex; } set {currentTimingPromptIndex = value; } }

    void Awake()
    {
        LessonContext.Init_SO(Slides_SO);
        LessonContext.Init_LessonFlowController(this);

        foreach (var data in Slides_SO)
        {
            H_dragAllowed.Add(data.dragslideIndex);
            D_Slides_SO[data.dragslideIndex] = data.prompts;
        }
    }

    void OnEnable()
    {
        LessonEvents.NextClicked += OnNext;
        LessonEvents.PrevClicked += OnPrev;
        LessonEvents.DragCompleted += CheckCanDrag;
    }

    void OnDisable()
    {
        LessonEvents.NextClicked -= OnNext;
        LessonEvents.PrevClicked -= OnPrev;
        LessonEvents.DragCompleted -= CheckCanDrag;
    }

    void Start()
    {
        LoadSlide(0);
        LessonEvents.RaiseShowTimingPrompts(true, currentTimingPromptIndex);
    }

    public void LoadSlide(int slideIndex)
    {
        currentSlide = slideIndex;
        currentPromptIndex = 0;
        currentTimingPromptIndex = 0;

        LessonEvents.RaiseShowSLide(currentSlide);

        bool completed =
            slideCompletionState.ContainsKey(currentSlide) &&
            slideCompletionState[currentSlide];

        LessonEvents.RaiseSetNextButtonState(completed);

        ShowCurrentSlide();
    }

    void ShowCurrentSlide()
    {
        LessonEvents.RaiseShowSLide(currentSlide);
        ShowCurrentPrompt();
    }

    void ShowCurrentPrompt()
    {
        LessonEvents.RaiseShowPromptIndex(currentPromptIndex);
      
        LessonEvents.RaiseShowInfoPrompt(currentPromptIndex);
      
    }


    void CheckCanDrag(string id)
    {
        if (H_dragAllowed.Contains(currentSlide))
        {
            OnDragCompleted(id);
        }
    }

    void OnDragCompleted(string draggableId)
    {
        if (D_Slides_SO.TryGetValue(currentSlide, out PromptData promptdata))
        {
            if (!promptdata.requiresDrag) return;
            if (promptdata.requiredDraggableId != draggableId) return;

            if (promptdata.onAdvance)
            {
                slideCompletionState[currentSlide] = true;
                Advance();
            }
        }
    }
    public void ShowCurrentPromptIteratted()
    {
        if (LessonContext.slideviewcontroller.PropmtLength() > 1)
        {
            currentPromptIndex++;
            LessonEvents.RaiseShowPromptIndex(currentPromptIndex);
        }
        // NEW SYSTEM: SlideViewController handles UI + auto hide
        if(LessonContext.slideviewcontroller.TimingPromptLength() > 1)
        {
            LessonEvents.RaiseShowTimingPrompts(true, currentTimingPromptIndex);
        }
        if (LessonContext.cineMachineFlowController.GetCameraCountForCurrentSlide() > 1)
        {
            LessonContext.cineMachineFlowController.SwitchNextCameraInSlide();
        }
    }

    void OnNext()
    {
        Advance();
    }

    public void Advance()
    {
        if (currentSlide < LessonContext.slideviewcontroller.TotalSlides - 1)
        {
            currentSlide++;
            LoadSlide(currentSlide);

            int s = currentSlide + 1;
            textGUI.text = s.ToString() + pageNum;
        }
        else
        {
            Debug.Log("Lesson Completed");
        }
    }

    void OnPrev()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            LoadSlide(currentSlide);
            ShowCurrentSlide();

            int s = currentSlide + 1;
            textGUI.text = s.ToString() + pageNum;
        }
    }
}
