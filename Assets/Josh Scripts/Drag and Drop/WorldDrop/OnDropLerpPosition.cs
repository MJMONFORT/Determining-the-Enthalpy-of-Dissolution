using UnityEngine;
using System.Collections;

public class OnDropLerpPosition : MonoBehaviour, IOnDropSuccess
{
    [SerializeField] Vector3 targetPosition;
    [SerializeField] bool localSpace;
    [SerializeField] float duration = 1f;
    [SerializeField] float delay;

    public void Execute()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        if (delay > 0) yield return new WaitForSeconds(delay);

        Vector3 startPos = transform.position;
        Vector3 endPos = localSpace ? transform.parent.TransformPoint(targetPosition) : targetPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
    }
}