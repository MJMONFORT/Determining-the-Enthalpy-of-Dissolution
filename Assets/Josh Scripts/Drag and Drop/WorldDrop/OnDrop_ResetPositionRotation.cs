using UnityEngine;

public class OnDrop_ResetPositionRotation : MonoBehaviour, IOnDropped
{
    [SerializeField] DropResetMode mode;
    [SerializeField] bool useLocalSpace = true;
    [SerializeField] Vector3 customPosition;
    [SerializeField] Vector3 customRotation;

    public void OnDropped(IWorldDraggable draggable)
    {
        var drag = draggable as Draggable;
        if (drag == null) return;

        drag.SkipDrop();

        var col = drag.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        GetPosition(drag).Apply(drag.transform, useLocalSpace);
    }

    IDropPosition GetPosition(Draggable drag)
    {
        switch (mode)
        {
            case DropResetMode.StartPosition:  return new StartPosition(drag);
            case DropResetMode.ZoneTransform:  return new ZonePosition(transform);
            case DropResetMode.CustomPosition: return new CustomPosition(customPosition, customRotation);
            default:                           return new StartPosition(drag);
        }
    }
}

public enum DropResetMode { StartPosition, ZoneTransform, CustomPosition }

interface IDropPosition
{
    void Apply(Transform t, bool local);
}

struct StartPosition : IDropPosition
{
    Vector3 pos; Quaternion rot;
    public StartPosition(Draggable d) { pos = d.DragStartPos; rot = d.DragStartRot; }
    public void Apply(Transform t, bool local)
    {
        if (local) { t.localPosition = pos; t.localRotation = rot; }
        else       { t.position = pos;      t.rotation = rot; }
    }
}

struct ZonePosition : IDropPosition
{
    Vector3 pos; Quaternion rot;
    public ZonePosition(Transform zone) { pos = zone.position; rot = zone.rotation; }
    public void Apply(Transform t, bool local)
    {
        if (local) { t.localPosition = pos; t.localRotation = rot; }
        else       { t.position = pos;      t.rotation = rot; }
    }
}

struct CustomPosition : IDropPosition
{
    Vector3 pos; Quaternion rot;
    public CustomPosition(Vector3 p, Vector3 euler) { pos = p; rot = Quaternion.Euler(euler); }
    public void Apply(Transform t, bool local)
    {
        if (local) { t.localPosition = pos; t.localRotation = rot; }
        else       { t.position = pos;      t.rotation = rot; }
    }
}