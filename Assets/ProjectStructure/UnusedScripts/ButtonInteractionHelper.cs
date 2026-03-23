using System.Collections;
using UnityEngine;

public class ButtonInteractionHelper : MonoBehaviour
{
    Coroutine c;
    void OnEnable()
    {
        StartButtonCoroutine();
    }

    private void StartButtonCoroutine()
    {
        if (c != null ) { StopCoroutine(c); c = null; }
        c = StartCoroutine("ButtonoInteraction");
    }

    IEnumerator ButtonoInteraction()
    {
        yield return new WaitForSeconds(1f);

        LessonEvents.RaiseSetNextButtonState(true);
    }
}
