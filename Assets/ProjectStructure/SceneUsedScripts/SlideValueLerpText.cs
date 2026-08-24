using System.Collections;
using TMPro;
using UnityEngine;

// Lerps a shared TMP text from 0 up to a per-slide target value.
// Wire PlayLerp() to a UnityEvent (e.g. ClickControllerInternalSwitchNextSlide.onClickSuccess).
public class SlideValueLerpText : MonoBehaviour
{
    [System.Serializable]
    public class SlideValueEntry
    {
        [Tooltip("Matches LessonContext.lessonFlowController.LFC_CurrentSlide.")]
        public int slideNumber;
        public float value;
    }

    [Header("Common Reference")]
    [SerializeField] TMP_Text targetText;

    [Header("Slide Wise Target Values")]
    [Tooltip("Only slides listed here will lerp. Slides not listed are ignored on click.")]
    [SerializeField] SlideValueEntry[] slideValues;

    [Header("Lerp Settings")]
    [SerializeField] float lerpDuration = 1f;
    [SerializeField] private string decimalcount = "F0";
    Coroutine lerpRoutine;

    // Public so it can be hooked to a UnityEvent from the inspector.
    public void PlayLerp()
    {
        if (targetText == null) return;
        if (slideValues == null || slideValues.Length == 0) return;

        int currentSlide = LessonContext.lessonFlowController.LFC_CurrentSlide;

        for (int i = 0; i < slideValues.Length; i++)
        {
            if (slideValues[i].slideNumber != currentSlide) continue;

            if (lerpRoutine != null) StopCoroutine(lerpRoutine);
            lerpRoutine = StartCoroutine(LerpRoutine(slideValues[i].value));
            return;
        }
        // No entry for this slide: leave the text untouched.
    }

    IEnumerator LerpRoutine(float target)
    {
        float elapsed = 0f;
        while (elapsed < lerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lerpDuration;
            float value = Mathf.Lerp(0f, target, t);
            targetText.text = value.ToString(decimalcount);
            yield return null;
        }

        targetText.text = target.ToString(decimalcount);
    }
}
