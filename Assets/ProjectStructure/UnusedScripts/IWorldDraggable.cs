using UnityEngine;

public interface IWorldDraggable
{
    string Id { get; }
    bool IsInteractable { get; }

    // Added to the camera-space depth captured when the object is picked up.
    // Positive pushes it further from the camera, negative pulls it closer.
    float DepthOffset { get; }
    void BeginDrag(Vector3 hitPoint);
    void Drag(Vector3 worldPosition);
    void EndDrag();

    void Attach(Transform attachTrans);

    void Notatach(Vector3 pos,Quaternion rot);
}
