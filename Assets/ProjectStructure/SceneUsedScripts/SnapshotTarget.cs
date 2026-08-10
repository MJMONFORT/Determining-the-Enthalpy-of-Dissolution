using System;
using System.Collections.Generic;
using UnityEngine;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    public class SnapshotTarget : MonoBehaviour
    {
        [Flags]
        public enum Strategy
        {
            None                = 0,
            ActiveState         = 1 << 0,
            LocalPosition       = 1 << 1,
            WorldPosition       = 1 << 2,
            LocalRotation       = 1 << 3,
            FullTransform       = 1 << 4,
            MeshRenderer        = 1 << 5,
            SkinnedMeshRenderer = 1 << 6
        }

        [SerializeField] Strategy strategies = Strategy.ActiveState;

        List<ISnapshotStrategy> built;

#if UNITY_EDITOR
        public Strategy EditorStrategies => strategies;
#endif

        public void Initialize()
        {
            Build();
        }

        public void Build()
        {
            if (built != null) return;

            built = new List<ISnapshotStrategy>();

            if ((strategies & Strategy.ActiveState) != 0)
                built.Add(new ActiveStateStrategy());

            if ((strategies & Strategy.LocalPosition) != 0)
                built.Add(new LocalPositionStrategy());

            if ((strategies & Strategy.WorldPosition) != 0)
                built.Add(new WorldPositionStrategy());

            if ((strategies & Strategy.LocalRotation) != 0)
                built.Add(new LocalRotationStrategy());

            if ((strategies & Strategy.FullTransform) != 0)
                built.Add(new FullTransformStrategy());

            if ((strategies & Strategy.MeshRenderer) != 0)
                built.Add(new MeshRendererStrategy());

            if ((strategies & Strategy.SkinnedMeshRenderer) != 0)
                built.Add(new SkinnedMeshRendererStrategy());
        }

        public void Capture(SceneSnapshot snapshot)
        {
            for (int i = 0; i < built.Count; i++)
                built[i].Capture(transform, snapshot);
        }

        public void Restore(SceneSnapshot snapshot)
        {
            for (int i = 0; i < built.Count; i++)
                built[i].Restore(transform, snapshot);
        }
    }
}
