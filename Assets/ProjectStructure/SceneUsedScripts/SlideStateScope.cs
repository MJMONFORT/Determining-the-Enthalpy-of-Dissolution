using System.Collections;
using UnityEngine;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    // Owns finished slides: captures the outgoing slide's SnapshotTargets on
    // SlideLeaving, restores them one frame after ShowSlide when the incoming
    // slide is already complete. ObjectsActivationController owns everything else.
    public class SlideStateScope : MonoBehaviour
    {
        [SerializeField] SlideStateSO store;

        SnapshotTarget[] targets;
        Coroutine restoreRoutine;

        void Awake()
        {
            targets = FindObjectsOfType<SnapshotTarget>(true);
            foreach (var target in targets)
                target.Initialize();
        }

        void OnEnable()
        {
            LessonEvents.SlideLeaving += OnSlideLeaving;
            LessonEvents.ShowSlide += OnSlideShown;
        }

        void OnDisable()
        {
            LessonEvents.SlideLeaving -= OnSlideLeaving;
            LessonEvents.ShowSlide -= OnSlideShown;
        }

        void OnSlideLeaving(int slide)
        {
            var snapshot = new SceneSnapshot();
            foreach (var target in targets)
                target.Capture(snapshot);

            store.Store(slide, snapshot);
        }

        void OnSlideShown(int slide)
        {
            if (!IsComplete(slide)) return;

            if (restoreRoutine != null)
                StopCoroutine(restoreRoutine);
            restoreRoutine = StartCoroutine(RestoreNextFrame(slide));
        }

        IEnumerator RestoreNextFrame(int slide)
        {
            yield return null; // let ObjectsActivationController's teardown land first

            if (!store.TryGet(slide, out var snapshot)) yield break;

            foreach (var target in targets)
                target.Restore(snapshot);
        }

        bool IsComplete(int slide)
        {
            return LessonContext.lessonFlowController != null &&
                   LessonContext.lessonFlowController.slideCompletionState.TryGetValue(slide, out bool done) &&
                   done;
        }
    }
}
