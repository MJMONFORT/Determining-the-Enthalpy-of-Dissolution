using UnityEngine;
using UnityEngine.Events;

public class WorldDropZone : WroldDrop
{

    [SerializeField] protected Animator animator;
    [SerializeField] protected UnityEvent onDropAccepted;

    // Only called once the drop has already been accepted, so no id check here.
    public override void DestroyHighlighter()
    {
        if (animator != null)
        {
            animator.enabled = true;
        }

        onDropAccepted?.Invoke();
    }

    public override bool Accept(string draggableId)
    {
        return base.Accept(draggableId);
    }
}
