using UnityEngine;
using TMPro;
using System.Collections;

public class TemperatureInput
    : MonoBehaviour
{
    public TMP_Text timerText;

    public float maxValue = 25f;
    public float speed = 1f; // 1 = real seconds

    private float currentValue = 0f;
    private bool isRunning = false;

    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        // Mouse
        if (Input.GetMouseButtonDown(0))
        {
            DetectInput(Input.mousePosition);
        }

        // Touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DetectInput(Input.GetTouch(0).position);
        }
    }

    void DetectInput(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform && !isRunning)
            {
                StartCoroutine(StartStopwatch());
            }
        }
    }

    IEnumerator StartStopwatch()
    {
        isRunning = true;
        currentValue = 0f;

        while (currentValue < maxValue)
        {
            currentValue += Time.deltaTime * speed;
            UpdateText();
            yield return null;
        }

        currentValue = maxValue;
        UpdateText();
        isRunning = false;
    }

    void UpdateText()
    {
        timerText.text = Mathf.FloorToInt(currentValue).ToString();
    }
}
