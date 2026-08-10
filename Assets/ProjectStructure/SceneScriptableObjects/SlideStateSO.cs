using System.Collections.Generic;
using UnityEngine;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    [CreateAssetMenu(menuName = "Lesson/Slide State SO")]
    public class SlideStateSO : ScriptableObject
    {
        readonly Dictionary<int, SceneSnapshot> snapshots = new();

        void OnEnable() { snapshots.Clear(); }
        void OnDisable() { snapshots.Clear(); }

        public void Store(int slide, SceneSnapshot snapshot)
        {
            snapshots[slide] = snapshot;
        }

        public bool TryGet(int slide, out SceneSnapshot snapshot)
        {
            return snapshots.TryGetValue(slide, out snapshot);
        }

        public bool HasState(int slide) { return snapshots.ContainsKey(slide); }

        public void Clear() { snapshots.Clear(); }

#if UNITY_EDITOR
        public List<int> EditorGetStoredSlides()
        {
            return new List<int>(snapshots.Keys);
        }

        public SceneSnapshot EditorGetSnapshot(int slide)
        {
            snapshots.TryGetValue(slide, out var snapshot);
            return snapshot;
        }
#endif
    }
}
