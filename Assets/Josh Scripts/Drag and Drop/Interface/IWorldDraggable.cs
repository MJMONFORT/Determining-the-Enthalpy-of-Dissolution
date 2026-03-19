using UnityEngine;

public interface IWorldDraggable
{
    string Id { get; }
    bool IsInteractable { get; }
    void BeginDrag(Vector3 hitPoint);
    void Drag(Vector3 worldPosition);
    void EndDrag();

    void Attach(Transform attachTrans);

    void Notatach(Vector3 pos,Quaternion rot);
}
