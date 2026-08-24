using System.Collections;
using UnityEngine;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    // Pulses a CanvasGroup's alpha to draw attention to the Next button while it is enabled.
    // Adapted for CryptoGam: LessonEvents is a static event bus in this namespace (no Instance).
    public class ButtonHighlightPulse : MonoBehaviour
    {
        [SerializeField] float minAlpha = 0.2f;
        [SerializeField] float maxAlpha = 1f;
        [SerializeField] float pulseDuration = 0.6f;

        [Tooltip("Pulse as soon as this object is enabled instead of waiting for the Next button event. " +
                 "Use for highlighters that must draw attention while the user still has work to do.")]
        [SerializeField] bool pulseOnEnable;

        CanvasGroup canvasGroup;
        Coroutine pulseRoutine;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void OnEnable()
        {
            if (pulseOnEnable)
            {
                StartPulse();
                return;
            }

            LessonEvents.SetNextButtonState += OnNextButtonStateChanged;
        }

        void OnDisable()
        {
            if (!pulseOnEnable)
                LessonEvents.SetNextButtonState -= OnNextButtonStateChanged;

            if (pulseRoutine != null)
            {
                StopCoroutine(pulseRoutine);
                pulseRoutine = null;
            }
        }

        void OnNextButtonStateChanged(bool nextEnabled)
        {
            if (nextEnabled) StartPulse();
            else StopPulse();
        }

        void StartPulse()
        {
            if (canvasGroup == null || pulseRoutine != null) return;
            pulseRoutine = StartCoroutine(PulseLoop());
        }

        void StopPulse()
        {
            if (pulseRoutine != null)
            {
                StopCoroutine(pulseRoutine);
                pulseRoutine = null;
            }

            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        IEnumerator PulseLoop()
        {
            while (true)
            {
                yield return LerpAlpha(canvasGroup.alpha, maxAlpha);
                yield return LerpAlpha(canvasGroup.alpha, minAlpha);
            }
        }

        IEnumerator LerpAlpha(float from, float to)
        {
            float t = 0f;
            while (t < pulseDuration)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, t / pulseDuration);
                yield return null;
            }
            canvasGroup.alpha = to;
        }
    }
}
