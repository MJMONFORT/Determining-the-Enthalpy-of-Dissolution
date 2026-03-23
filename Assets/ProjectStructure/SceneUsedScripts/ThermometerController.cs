using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
public class ThermometerController : MonoBehaviour
{
    public UnityEvent measureComplete;
    public TMP_Text displayText;
    public float correctTemperature = 25f;
    public float measureTime = .1f;
    private bool hasMeasured = false;
    Coroutine c;
    // Call this from ANY other script or button
    public void StartMeasurement()
    {
        if (hasMeasured) return;   // cannot run again
        hasMeasured = true;
        CoroutineTest();
    }

    void CoroutineTest()
    {
        
        if (c != null) { StopCoroutine(c); }
        c = StartCoroutine(MeasureRoutine());
    }

    IEnumerator MeasureRoutine()
    {
        float timer = 0f;
       
        while (timer < measureTime)
        {
            timer += Time.deltaTime * 1.5f;
            Debug.Log(timer);
            displayText.text = Random.Range(20, 35).ToString();
           
            yield return new WaitForSeconds(0.2f);
            
        }

        // Final stable reading
        displayText.text = correctTemperature.ToString("F2");
        measureComplete.Invoke();
        yield break;
       
    }
}
