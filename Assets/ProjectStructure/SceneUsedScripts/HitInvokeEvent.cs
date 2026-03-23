using UnityEngine;
using UnityEngine.Events;

public class HitInvokeEvent : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    public GameObjectEvent onClickEvent;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit: " + hit.transform.name);
                onClickEvent.Invoke(hit.transform.gameObject);
            }
        }
    }

}
