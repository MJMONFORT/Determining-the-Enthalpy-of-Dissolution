using UnityEngine;

[CreateAssetMenu(fileName = "SlideData", menuName = "Scriptable Objects/SlideData")]
public class SlideData : ScriptableObject
{
    public int dragslideIndex;
    public PromptData prompts; // size = 2
}
