using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class ObjectsActivationController : MonoBehaviour
{
    public GameObjectsToActivateData[] _gameobjectActivationdata;
    public GameObjectToDeActivateData[] _gameObjectDeActivationdata;
    public ScriptsActivationData[] _scriptsActivationData;
    HashSet<int> H_activationslidesGO = new HashSet<int>();
    HashSet<int> H_DeactivationslidesGO = new HashSet<int>();
    HashSet<int> H_activationslidesScripts = new HashSet<int>();
    Dictionary<int, GameObject[]> D_slideactivateGO = new Dictionary<int, GameObject[]>();
    Dictionary<int, GameObject[]> D_slidesdeactivateGO = new Dictionary<int, GameObject[]>();
    Dictionary<int, MonoBehaviour[]> D_slideactivateScripts = new Dictionary<int, MonoBehaviour[]>();

    [SerializeField] int currentslideGO,currentslideDeGO;
   [SerializeField] int currentslideScripts;
    private void Awake()
    {
        foreach(var data in _gameobjectActivationdata)
        {
            H_activationslidesGO.Add(data._gameobjectSlideIndex);
            D_slideactivateGO[data._gameobjectSlideIndex] = data.go;
        }

        foreach(var data in _scriptsActivationData)
        {
            H_activationslidesScripts.Add(data._scriptsSlideIndex);
            D_slideactivateScripts[data._scriptsSlideIndex] = data.scripts;
        }

        foreach(var data in _gameObjectDeActivationdata)
        {
            H_DeactivationslidesGO.Add(data._gameobjectSlideIndex);
            D_slidesdeactivateGO[data._gameobjectSlideIndex] = data.go;
        }

    }

    void OnEnable()
    {
        LessonEvents.ShowSlide += OnSlideChanged;
         
    }

    private void Start()
    {
        currentslideGO = 0;
        currentslideScripts = 0;
    }
    void OnSlideChanged(int newSlideIndex)
    {
        DeactivatePrevious(newSlideIndex);
        ActivateCurrent(newSlideIndex);
    }

    // ---------------- ACTIVATE ----------------

    void ActivateCurrent(int index)
    {
        // GameObjects
        if (H_activationslidesGO.Contains(index))
        {
            GO_SetActiveForSlide(index, true);
            currentslideGO = index;
        }

        // Scripts
        if (H_activationslidesScripts.Contains(index))
        {
            Scripts_SetActivateForSlide(index, true);
            currentslideScripts = index;
        }

        if (H_DeactivationslidesGO.Contains(index))
        {
            GO_DeactivateForSlide(index, false);
            currentslideDeGO = index;
        }
    }

    // ---------------- DEACTIVATE ----------------

    void DeactivatePrevious(int newIndex)
    {
        // GameObjects
        if (currentslideGO != -1 && currentslideGO != newIndex)
        {
            GO_SetActiveForSlide(currentslideGO, false);
            currentslideGO = -1;
        }

        // Scripts
        if (currentslideScripts != -1 && currentslideScripts != newIndex)
        {
            Scripts_SetActivateForSlide(currentslideScripts, false);
            currentslideScripts = -1;
        }

        if (currentslideDeGO != -1 && currentslideDeGO != newIndex)
        {
            GO_DeactivateForSlide(currentslideDeGO, true);
            currentslideDeGO = -1;
        }
    }

    // ---------------- HELPERS ----------------

    void GO_SetActiveForSlide(int slideIndex, bool state)
    {
        if (!D_slideactivateGO.TryGetValue(slideIndex, out var objects))
            return;

        foreach (var obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }

    void GO_DeactivateForSlide(int slideIndex,bool state)
    {
        if (!D_slidesdeactivateGO.TryGetValue(slideIndex, out var objects))
            return;
        foreach(var obj in objects)
        {
            if(obj != null)
                obj.SetActive(state);
        }
    }
    void Scripts_SetActivateForSlide(int slideIndex, bool state)
    {
        if (!D_slideactivateScripts.TryGetValue(slideIndex, out var scripts))
            return;

        foreach (var script in scripts)
        {
            if (script != null)
                script.enabled = state;
        }
    }

   
    private void OnDisable()
    {
        LessonEvents.ShowSlide -= OnSlideChanged;
    }

}

[System.Serializable]
public class GameObjectsToActivateData
{
    public int _gameobjectSlideIndex;
    public GameObject[] go;
}

[System.Serializable]
public class ScriptsActivationData
{
    public int _scriptsSlideIndex;
    public MonoBehaviour[] scripts;
}

[System.Serializable]
public class GameObjectToDeActivateData
{
    public int _gameobjectSlideIndex;
    public GameObject[] go; 
}
