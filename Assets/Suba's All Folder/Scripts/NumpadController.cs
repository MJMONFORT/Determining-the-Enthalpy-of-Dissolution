using UnityEngine;
using TMPro;

public class NumpadController : MonoBehaviour
{
    ////public TMP_Text[] boxes; // 0,1 input | 2 result

    ////[Header("Feedback Icons")]
    ////public GameObject greenTick1;
    ////public GameObject greenTick2;
    ////public GameObject redTick1;
    ////public GameObject redTick2;

    ////private int activeBox = 0;

    ////public void SelectBox(int index)
    ////{
    ////    if (index == 2) return; // lock result box
    ////    activeBox = index;
    ////}

    ////public void OnNumpadPress(string value)
    ////{
    ////    boxes[activeBox].text += value;
    ////}

    ////public void Backspace()
    ////{
    ////    var t = boxes[activeBox].text;
    ////    if (t.Length > 0)
    ////        boxes[activeBox].text = t.Substring(0, t.Length - 1);
    ////}

    ////public void ClearAll()
    ////{
    ////    boxes[0].text = "";
    ////    boxes[1].text = "";
    ////    boxes[2].text = "";
    ////}

    ////public void CheckAnswer()
    ////{
    ////    float b1, b2;

    ////    if (!float.TryParse(boxes[0].text, out b1) ||
    ////        !float.TryParse(boxes[1].text, out b2))
    ////    {
    ////        ShowWrong();
    ////        return;
    ////    }

    ////    if (Mathf.Approximately(b1, 100f) &&
    ////        Mathf.Approximately(b2, 8.7f))
    ////    {
    ////        ShowCorrect(b1, b2);
    ////    }
    ////    else
    ////    {
    ////        ShowWrong();
    ////    }
    ////}

    ////void ShowCorrect(float b1, float b2)
    ////{
    ////    greenTick1.SetActive(true);
    ////    greenTick2.SetActive(true);
    ////    redTick1.SetActive(false);
    ////    redTick2.SetActive(false);

    ////    boxes[2].text = (b1 + b2).ToString("F1");

    ////   // FindObjectOfType<SlideManager>().CompleteCurrentStep();
    ////}

    ////void ShowWrong()
    ////{
    ////    greenTick1.SetActive(false);
    ////    greenTick2.SetActive(false);
    ////    redTick1.SetActive(true);
    ////    redTick2.SetActive(true);

    ////    boxes[2].text = ""; // result cleared

    ////    // Optional behaviour:
    ////    // ClearAll();  // uncomment if you want reset


    public TMP_Text[] boxes; // 0,1 input | 2 result

    [Header("Correct Answers")]
    public float correctBox1;
    public float correctBox2;

    public enum Operation { Add, Subtract, Multiply, Divide }
    public Operation operation;

    [Header("Feedback")]
    public GameObject greenTick1;
    public GameObject greenTick2;
    public GameObject redTick1;
    public GameObject redTick2;

    private int activeBox = 0;

    public void SelectBox(int index)
    {
        if (index == 2) return;
        activeBox = index;
    }

    public void OnNumpadPress(string value)
    {
        boxes[activeBox].text += value;
    }

    public void Backspace()
    {
        var t = boxes[activeBox].text;
        if (t.Length > 0)
            boxes[activeBox].text = t.Substring(0, t.Length - 1);
    }

    public void CheckAnswer()
    {
        float b1, b2;

        if (!float.TryParse(boxes[0].text, out b1) ||
            !float.TryParse(boxes[1].text, out b2))
        {
            ShowWrong();
            return;
        }

        if (Mathf.Approximately(b1, correctBox1) &&
            Mathf.Approximately(b2, correctBox2))
        {
            ShowCorrect(b1, b2);
        }
        else
        {
            ShowWrong();
        }
    }

    void ShowCorrect(float b1, float b2)
    {
        greenTick1.SetActive(true);
        greenTick2.SetActive(true);
        redTick1.SetActive(false);
        redTick2.SetActive(false);

        float result = Calculate(b1, b2);
        boxes[2].text = result.ToString("F1");

      // FindObjectOfType<SlideManager>().CompleteCurrentStep();
    }

    void ShowWrong()
    {
        greenTick1.SetActive(false);
        greenTick2.SetActive(false);
        redTick1.SetActive(true);
        redTick2.SetActive(true);
        boxes[2].text = "";
    }

    float Calculate(float a, float b)
    {
        switch (operation)
        {
            case Operation.Add: return a + b;
            case Operation.Subtract: return a - b;
            case Operation.Multiply: return a * b;
            case Operation.Divide: return a / b;
        }
        return 0;
    }
}
