public interface IOnDragBegin
{
    void Execute();
}

public interface IOnDropSuccess
{
    void Execute();
}

public interface IOnDropReturn
{
    void Execute();
}

public interface IOnZoneReceive
{
    void Execute(string id);
}
public interface IOnDropped
{
    void OnDropped(IWorldDraggable draggable);
}

