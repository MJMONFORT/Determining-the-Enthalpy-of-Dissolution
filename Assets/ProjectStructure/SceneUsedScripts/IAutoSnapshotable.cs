namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    public interface IAutoSnapshotable
    {
        void CaptureState(SceneSnapshot snapshot);
        void RestoreState(SceneSnapshot snapshot);
    }
}
