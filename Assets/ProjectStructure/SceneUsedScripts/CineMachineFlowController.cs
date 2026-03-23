using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineFlowController : MonoBehaviour
{
    [Header("Camera Targets Per Slide")]
    [SerializeField] CameraTransformData[] _cameraActivationData;

    Dictionary<int, Transform[]> D_activationCamData =
        new Dictionary<int, Transform[]>();

    HashSet<int> H_activationCamData =
        new HashSet<int>();

    [Header("Camera Settings")]
    [SerializeField] Transform mainCamera;   // Assign Main Camera here
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotateSpeed = 6f;

    int currentSlide = -1;
    int currentCameraIndex = 0;

    Coroutine moveRoutine;

    public CameraTransformData[] CameraActivationData
    {
        get { return _cameraActivationData; }
    }

    void Awake()
    {
        LessonContext.Init_CineMachineFlowController(this);

        foreach (var data in _cameraActivationData)
        {
            H_activationCamData.Add(data.camSlideIndex);
            D_activationCamData[data.camSlideIndex] = data.cameraTargets;
        }
    }

    void OnEnable()
    {
        LessonEvents.ShowSlide += OnSlideChanged;
    }

    void OnDisable()
    {
        LessonEvents.ShowSlide -= OnSlideChanged;
    }

    // ================================
    // SLIDE CHANGE
    // ================================
    void OnSlideChanged(int slideIndex)
    {
        if (currentSlide != slideIndex)
        {
            currentSlide = slideIndex;
            currentCameraIndex = 0;
        }

        MoveToCurrentTarget();
    }

    // ================================
    // ITERATE CAMERA INSIDE SLIDE
    // ================================
    public void SwitchNextCameraInSlide()
    {
        if (!H_activationCamData.Contains(currentSlide))
            return;

        if (!D_activationCamData.TryGetValue(currentSlide, out var targets))
            return;

        if (currentCameraIndex < targets.Length - 1)
        {
            currentCameraIndex++;
            MoveToCurrentTarget();
        }
    }

    // ================================
    // MOVE CAMERA
    // ================================
    void MoveToCurrentTarget()
    {
        if (!H_activationCamData.Contains(currentSlide))
            return;

        if (!D_activationCamData.TryGetValue(currentSlide, out var targets))
            return;

        if (targets == null || targets.Length == 0)
            return;

        Transform target = targets[currentCameraIndex];

        if (target == null || mainCamera == null)
            return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(LerpCamera(target));
    }

    IEnumerator LerpCamera(Transform target)
    {
        while (Vector3.Distance(mainCamera.position, target.position) > 0.01f)
        {
            mainCamera.position =
                Vector3.Lerp(mainCamera.position,
                             target.position,
                             Time.deltaTime * moveSpeed);

            mainCamera.rotation =
                Quaternion.Slerp(mainCamera.rotation,
                                 target.rotation,
                                 Time.deltaTime * rotateSpeed);

            yield return null;
        }

        // Snap final precision
        mainCamera.position = target.position;
        mainCamera.rotation = target.rotation;
    }

    public int GetCameraCountForCurrentSlide()
    {
        if (!H_activationCamData.Contains(currentSlide))
            return 0;

        if (!D_activationCamData.TryGetValue(currentSlide, out var targets))
            return 0;

        return targets.Length;
    }

}

[System.Serializable]
public class CameraTransformData
{
    public int camSlideIndex;
    public Transform[] cameraTargets;
}


