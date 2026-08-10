using UnityEngine;
using System.Collections;

public class OnDropRotate : MonoBehaviour, IOnDropSuccess
{
    [SerializeField] Vector3 targetEuler;
    [SerializeField] float duration = 1f;
    [SerializeField] float delay;

    public void Execute()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        if (delay > 0) yield return new WaitForSeconds(delay);

        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(targetEuler);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
        transform.rotation = endRot;
    }
}
