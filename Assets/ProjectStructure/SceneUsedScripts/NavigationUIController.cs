using UnityEngine;

public class NavigationUIController : MonoBehaviour
{
    public void Next() { LessonEvents.RaiseNext(); }
    public void Prev() { LessonEvents.RaisePrev(); }
}
