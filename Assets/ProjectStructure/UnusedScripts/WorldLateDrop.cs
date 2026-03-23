using UnityEngine;
using System.Collections;
public class WorldLateDrop : WroldDrop
{
    [SerializeField] Animator _animator;
    Coroutine C;

    void CoroutineAnim()
    {
        if(C != null) { StopCoroutine(C); }
        C = StartCoroutine("DelayAnimation");
    }
    public override void DestroyHighlighter()
    {
        for (int i = 0; i < acceptedId.Length; i++)
        {
            if (Accept(acceptedId[i]))
            {
                CoroutineAnim();
            }
        }

    }

    public override bool Accept(string draggableId)
    {
        return base.Accept(draggableId);
    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(.5f);
        if (_animator != null)
        {
            _animator.enabled = true;
        }
    }
}
