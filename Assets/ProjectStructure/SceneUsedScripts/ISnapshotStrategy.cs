using UnityEngine;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    public interface ISnapshotStrategy
    {
        void Capture(Transform t, SceneSnapshot snapshot);
        void Restore(Transform t, SceneSnapshot snapshot);
    }
}
