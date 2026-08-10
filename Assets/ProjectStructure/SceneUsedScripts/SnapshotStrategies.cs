using System;
using UnityEngine;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    [Serializable]
    public class ActiveStateStrategy : ISnapshotStrategy
    {
        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            snapshot.activeStates[t.gameObject.GetInstanceID()] = t.gameObject.activeSelf;
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            if (snapshot.activeStates.TryGetValue(t.gameObject.GetInstanceID(), out bool active))
                t.gameObject.SetActive(active);
        }
    }

    [Serializable]
    public class LocalPositionStrategy : ISnapshotStrategy
    {
        string Key(Transform t) => $"{t.gameObject.GetInstanceID()}_lpos";

        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            string k = Key(t);
            snapshot.floatValues[$"{k}_x"] = t.localPosition.x;
            snapshot.floatValues[$"{k}_y"] = t.localPosition.y;
            snapshot.floatValues[$"{k}_z"] = t.localPosition.z;
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            string k = Key(t);
            if (!snapshot.floatValues.TryGetValue($"{k}_x", out float x)) return;
            if (!snapshot.floatValues.TryGetValue($"{k}_y", out float y)) return;
            if (!snapshot.floatValues.TryGetValue($"{k}_z", out float z)) return;
            t.localPosition = new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class WorldPositionStrategy : ISnapshotStrategy
    {
        string Key(Transform t) => $"{t.gameObject.GetInstanceID()}_wpos";

        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            string k = Key(t);
            snapshot.floatValues[$"{k}_x"] = t.position.x;
            snapshot.floatValues[$"{k}_y"] = t.position.y;
            snapshot.floatValues[$"{k}_z"] = t.position.z;
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            string k = Key(t);
            if (!snapshot.floatValues.TryGetValue($"{k}_x", out float x)) return;
            if (!snapshot.floatValues.TryGetValue($"{k}_y", out float y)) return;
            if (!snapshot.floatValues.TryGetValue($"{k}_z", out float z)) return;
            t.position = new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class LocalRotationStrategy : ISnapshotStrategy
    {
        string Key(Transform t) => $"{t.gameObject.GetInstanceID()}_lrot";

        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            string k = Key(t);
            Vector3 e = t.localRotation.eulerAngles;
            snapshot.floatValues[$"{k}_x"] = e.x;
            snapshot.floatValues[$"{k}_y"] = e.y;
            snapshot.floatValues[$"{k}_z"] = e.z;
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            string k = Key(t);
            if (!snapshot.floatValues.TryGetValue($"{k}_x", out float x)) return;
            if (!snapshot.floatValues.TryGetValue($"{k}_y", out float y)) return;
            if (!snapshot.floatValues.TryGetValue($"{k}_z", out float z)) return;
            t.localRotation = Quaternion.Euler(x, y, z);
        }
    }

    [Serializable]
    public class FullTransformStrategy : ISnapshotStrategy
    {
        readonly LocalPositionStrategy pos = new();
        readonly LocalRotationStrategy rot = new();

        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            pos.Capture(t, snapshot);
            rot.Capture(t, snapshot);
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            pos.Restore(t, snapshot);
            rot.Restore(t, snapshot);
        }
    }

    [Serializable]
    public class MeshRendererStrategy : ISnapshotStrategy
    {
        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            var renderers = t.GetComponents<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                snapshot.meshStates[renderers[i].GetInstanceID()] = renderers[i].enabled;
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            var renderers = t.GetComponents<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                if (snapshot.meshStates.TryGetValue(renderers[i].GetInstanceID(), out bool state))
                    renderers[i].enabled = state;
        }
    }

    [Serializable]
    public class SkinnedMeshRendererStrategy : ISnapshotStrategy
    {
        public void Capture(Transform t, SceneSnapshot snapshot)
        {
            var renderers = t.GetComponents<SkinnedMeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                snapshot.meshStates[renderers[i].GetInstanceID()] = renderers[i].enabled;
        }

        public void Restore(Transform t, SceneSnapshot snapshot)
        {
            var renderers = t.GetComponents<SkinnedMeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
                if (snapshot.meshStates.TryGetValue(renderers[i].GetInstanceID(), out bool state))
                    renderers[i].enabled = state;
        }
    }
}
