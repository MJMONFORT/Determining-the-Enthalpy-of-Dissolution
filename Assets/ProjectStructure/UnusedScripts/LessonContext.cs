using UnityEngine;

public static class LessonContext
{
    public static SlideData[] slides_SO { get; private set; }

    public static SlidePromptData[] slides_CG { get; private set; }

    public static SlideViewController slideviewcontroller { get; private set; }

    public static LessonFlowController lessonFlowController { get;  set; }

    public static CineMachineFlowController cineMachineFlowController { get; set; }

    public static int naildropcount { get; set; }

    public static int count { get; set; }

    public static Animator animator { get; set; }
    public static bool microscopeSLide { get; set; }
    public static DragInputController modelDragController { get; set; } 


    public static void Init_MicroSlide(Animator _animator)
    {
        animator = _animator;
    }
    public static void Init_SO(SlideData[] slides_so)
    {
        slides_SO = slides_so;
    }
    
    public static void Init_CG(SlidePromptData[] slides_cg)
    {
        slides_CG = slides_cg;
    }

    public static void Init_SlideView(SlideViewController _slideViewController)
    {
        slideviewcontroller = _slideViewController;
    }
    public static void Init_LessonFlowController(LessonFlowController _lessonFlowController)
    {
        lessonFlowController = _lessonFlowController;
    }

    public static void Init_DragInputController(DragInputController _dragInputController)
    {
        modelDragController = _dragInputController;
    }

    public static void Init_CineMachineFlowController(CineMachineFlowController _cineMachineFlowController)
    {
        cineMachineFlowController = _cineMachineFlowController;
    }
   
}
