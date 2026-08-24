using UnityEngine;

public class MicroSCopeDrop : WroldDrop
{
    [SerializeField] Animator animator;
    private void Awake()
    {
        LessonContext.Init_MicroSlide(animator);
    }
    // Only called once the drop has already been accepted, so no id check here.
    public override void DestroyHighlighter()
    {
        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    public override bool Accept(string draggableId)
    {
        return base.Accept(draggableId);
    }
}
