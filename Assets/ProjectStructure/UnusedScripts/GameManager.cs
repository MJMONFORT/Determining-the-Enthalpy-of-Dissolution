using System.Security.AccessControl;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Drag _Animations")]
    public DragSnapXY dragSnapXY;
    [SerializeField] private Animator animator;
    public GameObject[] Beakers;
    public GameObject AnimatedBeaker;
    private bool snapTriggered = false;

    void Start()
    {
        // dragSnapXY = GetComponent<DragSnapXY>();
    }
    void Update()
    {
        CheckSnap();
    }
    #region CheckSnap  
    private void CheckSnap()
    {
        if (dragSnapXY == null)
            return;

        if (dragSnapXY.isSnapped && !snapTriggered)
        {
            snapTriggered = true;

            AnimatedBeaker.SetActive(true);
            animator.SetBool("rotate", true);

            foreach (GameObject beaker in Beakers)
            {
                beaker.SetActive(false);
            }
        }
    }
    #endregion

}

[System.Serializable]
public class SlideGroup
{
    public GameObject[] SubOfield;
}
