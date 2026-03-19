using UnityEngine;
using System.Collections;

public class LiquidColorChange : MonoBehaviour
{
    public Color startColor = Color.white;
    public Color targetColor = Color.blue;
    public float delay = 2f;
    public float changeDuration = 2f; // how slow the change is

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.color = startColor;

        StartCoroutine(ChangeColorRoutine());
    }

    IEnumerator ChangeColorRoutine()
    {
        // wait 2 seconds
        yield return new WaitForSeconds(delay);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / changeDuration;
            mat.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        mat.color = targetColor; // ensure final color
    }
}
