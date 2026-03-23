using UnityEngine;

public class WroldDrop : MonoBehaviour
{
    [SerializeField] protected string[] acceptedId;

    public virtual void DestroyHighlighter()
    {
       
    }

    public virtual bool Accept(string draggableId)
    {
        for(int i = 0;i < acceptedId.Length; i++)
        {
            if (draggableId == acceptedId[i])
            {
                LessonEvents.RaiseDragCompleted(draggableId);

                return true;
            }
        }
        return false;
    }
}
