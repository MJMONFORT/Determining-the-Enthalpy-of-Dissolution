using UnityEngine;

public class MicroSCopeDrop : WroldDrop
{
    [SerializeField] Animator animator;
    private void Awake()
    {
        LessonContext.Init_MicroSlide(animator);
    }
    public override void DestroyHighlighter()
    {
        for(int i = 0;i < acceptedId.Length;i++)
        {
            if (Accept(acceptedId[i]))
            {
                if (animator != null)
                {
                    animator.enabled = true;
                }
            }
        }
        
    }

    public override bool Accept(string draggableId)
    {
        return base.Accept(draggableId);
    }
}
