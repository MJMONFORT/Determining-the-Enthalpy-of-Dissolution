using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField] string acceptedId;
    bool consumed;

    public string AcceptedId => acceptedId;

    void OnEnable()
    {
        // If SlideScope re-activates this GO, shut down immediately if already consumed
        if (consumed) enabled = false;
    }

    public bool Accept(string draggableId, IWorldDraggable draggable)
    {
        if (draggableId != acceptedId) return false;

        consumed = true;

        int slide = LessonContext.lessonFlowController.LFC_CurrentSlide;
        //SlideDropRecord.Record(slide, draggableId);
        LessonEvents.RaiseDragCompleted(draggableId);

        var zoneCallbacks = GetComponents<IOnZoneReceive>();
        for (int i = 0; i < zoneCallbacks.Length; i++)
        {
            var mb = zoneCallbacks[i] as MonoBehaviour;
            if (!mb.enabled) continue;
            zoneCallbacks[i].Execute(draggableId);
        }

        var successCallbacks = GetComponents<IOnDropSuccess>();
        for (int i = 0; i < successCallbacks.Length; i++)
        {
            var mb = successCallbacks[i] as MonoBehaviour;
            if (!mb.enabled) continue;
            successCallbacks[i].Execute();
        }

        var droppedCallbacks = GetComponents<IOnDropped>();
        for (int i = 0; i < droppedCallbacks.Length; i++)
        {
            var mb = droppedCallbacks[i] as MonoBehaviour;
            if (!mb.enabled) continue;
            droppedCallbacks[i].OnDropped(draggable);
        }

        this.enabled = false;
        return true;
    }

    public void ResetConsumed() => consumed = false;
}
