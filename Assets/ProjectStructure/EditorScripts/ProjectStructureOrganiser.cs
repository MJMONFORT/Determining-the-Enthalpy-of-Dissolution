using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ProjectStructureOrganiser
{
    static string root = "Assets/ProjectStructure";

    [MenuItem("Tools/Organize Project Scripts")]
    static void Organize()
    {
        CreateFolders();

        HashSet<string> usedScripts = new HashSet<string>();
        HashSet<string> usedScriptableObjects = new HashSet<string>();

        // -----------------------------
        // FIND USED SCRIPTS IN SCENE
        // -----------------------------
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in objects)
        {
            MonoBehaviour[] behaviours = obj.GetComponents<MonoBehaviour>();

            foreach (var b in behaviours)
            {
                if (b == null) continue;

                MonoScript script = MonoScript.FromMonoBehaviour(b);
                string path = AssetDatabase.GetAssetPath(script);

                if (!string.IsNullOrEmpty(path))
                    usedScripts.Add(path);
            }

            // -----------------------------
            // FIND SCRIPTABLE OBJECT REFERENCES
            // -----------------------------
            SerializedObject so = new SerializedObject(obj);
            SerializedProperty prop = so.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop.propertyType == SerializedPropertyType.ObjectReference)
                {
                    Object reference = prop.objectReferenceValue;

                    if (reference is ScriptableObject)
                    {
                        string path = AssetDatabase.GetAssetPath(reference);

                        if (!string.IsNullOrEmpty(path))
                            usedScriptableObjects.Add(path);
                    }
                }
            }
        }

        // -----------------------------
        // ORGANIZE SCRIPTS
        // -----------------------------
        string[] allScripts = AssetDatabase.FindAssets("t:MonoScript");

        foreach (string guid in allScripts)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.StartsWith("Packages/")) continue;

            if (path.Contains("/Editor/"))
            {
                MoveAsset(path, root + "/EditorScripts");
            }
            else if (usedScripts.Contains(path))
            {
                MoveAsset(path, root + "/SceneUsedScripts");
            }
            else
            {
                MoveAsset(path, root + "/UnusedScripts");
            }
        }

        // -----------------------------
        // ORGANIZE SCRIPTABLE OBJECTS
        // -----------------------------
        string[] scriptables = AssetDatabase.FindAssets("t:ScriptableObject");

        foreach (string guid in scriptables)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.StartsWith("Packages/")) continue;

            if (usedScriptableObjects.Contains(path))
            {
                MoveAsset(path, root + "/SceneScriptableObjects");
            }
        }

        // -----------------------------
        // ORGANIZE CUSTOM SHADERS (FIXED)
        // -----------------------------
        Renderer[] renderers = GameObject.FindObjectsOfType<Renderer>(true);
        HashSet<string> usedShaders = new HashSet<string>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat == null) continue;

                Shader shader = mat.shader;
                string shaderPath = AssetDatabase.GetAssetPath(shader);

                if (string.IsNullOrEmpty(shaderPath)) continue;

                // ❌ Skip Package shaders (TMP, URP, etc.)
                if (shaderPath.StartsWith("Packages/")) continue;

                // ❌ Extra TMP safety
                if (shader.name.Contains("TextMeshPro")) continue;

                usedShaders.Add(shaderPath);
            }
        }

        foreach (string shaderPath in usedShaders)
        {
            MoveAsset(shaderPath, root + "/CustomShaders");
        }

        AssetDatabase.Refresh();
        Debug.Log("Project Organized Successfully ✅");
    }

    // -----------------------------
    // FOLDER CREATION
    // -----------------------------
    static void CreateFolders()
    {
        if (!AssetDatabase.IsValidFolder(root))
            AssetDatabase.CreateFolder("Assets", "ProjectStructure");

        Create(root, "EditorScripts");
        Create(root, "SceneUsedScripts");
        Create(root, "UnusedScripts");
        Create(root, "SceneScriptableObjects");
        Create(root, "CustomShaders");
    }

    static void Create(string parent, string name)
    {
        if (!AssetDatabase.IsValidFolder(parent + "/" + name))
            AssetDatabase.CreateFolder(parent, name);
    }

    // -----------------------------
    // SAFE MOVE FUNCTION
    // -----------------------------
    static void MoveAsset(string path, string folder)
    {
        if (string.IsNullOrEmpty(path)) return;

        // ❌ Never touch Packages
        if (path.StartsWith("Packages/")) return;

        string fileName = Path.GetFileName(path);
        string newPath = folder + "/" + fileName;

        if (path == newPath) return;

        string error = AssetDatabase.MoveAsset(path, newPath);

        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogWarning($"Failed to move: {path} → {newPath}\n{error}");
        }
    }
}