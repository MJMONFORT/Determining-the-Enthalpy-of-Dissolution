using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    [SerializeField] Image[] correct;
    private void OnDisable()
    {
        for (int i = 0; i < correct.Length; i++)
        {
            correct[i].gameObject.SetActive(false);
        }
        
    }
}
