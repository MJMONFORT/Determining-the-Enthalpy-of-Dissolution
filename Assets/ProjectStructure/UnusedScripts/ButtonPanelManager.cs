using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonPanelManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonPanelPair
    {
        public Button button;
        public GameObject panel;
    }

    [Header("Button → Panel Mapping")]
    public List<ButtonPanelPair> mappings = new List<ButtonPanelPair>();

    void Start()
    {
        // Hide all panels at start and hook buttons
        foreach (var pair in mappings)
        {
            if (pair.panel != null)
                pair.panel.SetActive(false);

            if (pair.button != null)
            {
                Button cachedButton = pair.button;
                GameObject cachedPanel = pair.panel;

                cachedButton.onClick.AddListener(() =>
                {
                    ShowOnly(cachedPanel);
                });
            }
        }
    }

    void ShowOnly(GameObject panelToShow)
    {
        foreach (var pair in mappings)
        {
            if (pair.panel != null)
                pair.panel.SetActive(pair.panel == panelToShow);
        }
    }
}
