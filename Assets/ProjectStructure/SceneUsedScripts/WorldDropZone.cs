using UnityEngine;

public class WorldDropZone : WroldDrop
{

    [SerializeField] protected Animator animator;

    public override void DestroyHighlighter()
    {
        if (Accept(acceptedId[0]))
        {
            if(animator != null)
            {
                animator.enabled = true;
            }
        }        
    }

    public override bool Accept(string draggableId)
    {
        return base.Accept(draggableId);
    }
}
