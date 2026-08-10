using UnityEngine;
using System.Collections;

public class OnDrop_SwitchCamera : MonoBehaviour, IOnDropSuccess
{
    
    public void Execute()
    {
        //int slide = LessonContext.lessonFlowController.LFC_CurrentSlide;
        //if (activeSlide ==   null && !activeSlide.Contains(slide)) return;
        LessonContext.cineMachineFlowController.SwitchNextCameraInSlide();
        Debug.Log("SwitchedCamera");
    }
}

