using System.Collections.Generic;

namespace MorphologyOfDifferentPlantGroupsCryptograms
{
    public class SceneSnapshot
    {
        public readonly Dictionary<int, bool> activeStates = new();
        public readonly Dictionary<int, bool> meshStates = new();
        public readonly Dictionary<int, int> dropdownValues = new();
        public readonly Dictionary<string, float> floatValues = new();
        public readonly Dictionary<string, bool> boolValues = new();
        public readonly Dictionary<string, int> intValues = new();
    }
}
