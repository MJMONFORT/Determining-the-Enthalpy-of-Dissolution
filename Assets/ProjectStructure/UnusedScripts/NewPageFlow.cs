using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class NewPageFlow : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        [Header("Objects to ENABLE in this step")]
        public GameObject[] enableObjects;

        [Header("Objects to DISABLE in this step")]
        public GameObject[] disableObjects;

        [Header("Event when step starts")]
        public UnityEvent onStepEnter;

        [HideInInspector]
        public bool completed;
    }

    [Header("All Steps")]
    public Step[] steps;

    [Header("Navigation Buttons")]
    public Button nextButton;
    public Button backButton;

    [Header("Debug")]
    public int currentStep = 0;

    // -------------------- START --------------------
    void Start()
    {
        ApplyStep(0);          // Apply first step only once
        UpdateButtons();
    }

    // -------------------- APPLY STEP (FORWARD) --------------------
    void ApplyStep(int index)
    {
        Step step = steps[index];

        // Enable required objects
        foreach (var go in step.enableObjects)
            if (go) go.SetActive(true);

        // Disable required objects
        foreach (var go in step.disableObjects)
            if (go) go.SetActive(false);

        // Call step event
        step.onStepEnter?.Invoke();

        UpdateButtons();
    }

    // -------------------- REVERSE STEP (BACKWARD) --------------------
    void ReverseStep(int index)
    {
        Step step = steps[index];

        // Undo enable -> disable
        foreach (var go in step.enableObjects)
            if (go) go.SetActive(false);

        // Undo disable -> enable
        foreach (var go in step.disableObjects)
            if (go) go.SetActive(true);
    }

    // -------------------- NEXT BUTTON --------------------
    public void Next()
    {
        if (!steps[currentStep].completed)
        {
            Debug.Log("Complete current step first!");
            return;
        }

        if (currentStep < steps.Length - 1)
        {
            currentStep++;
            ApplyStep(currentStep);
        }
    }

    // -------------------- BACK BUTTON --------------------
    public void Back()
    {
        if (currentStep > 0)
        {
            // First undo current step
            ReverseStep(currentStep);

            // Then go back
            currentStep--;
            ApplyStep(currentStep);
        }
    }

    // -------------------- COMPLETE STEP (CALL FROM EVENTS) --------------------
    public void CompleteCurrentStep()
    {
        steps[currentStep].completed = true;
        UpdateButtons();
        Debug.Log("Step " + currentStep + " completed");
    }

    // -------------------- BUTTON STATE --------------------
    void UpdateButtons()
    {
        backButton.interactable = currentStep > 0;
        nextButton.interactable = steps[currentStep].completed;
    }
}
