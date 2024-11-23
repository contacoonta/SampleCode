using System;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetContainer<T> : ScriptableObject where T : UnityEngine.Object
{
    [Space(30)][HideField] public bool _s0;
    [HorizontalLine("해당 폴더 및 하위 폴더 포함"), HideField] public bool _l0;

    [SerializeField] string folderPath = "";

#if UNITY_EDITOR
    [Space(20)][Button(nameof(OpenPath), label = "Open Folder Path"), HideField] public bool _openpath;
    public void OpenPath()
    {
        string selectedPath = EditorUtility.OpenFolderPanel("Asset 폴더 선택", Application.dataPath, "");
        if (!string.IsNullOrEmpty(selectedPath))
        {
            if (selectedPath.StartsWith(Application.dataPath))
                folderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            else
                EditorUtility.DisplayDialog("유효하지 않은 폴더", "Assets 폴더 내의 폴더를 선택해주세요.", "확인");
        }
    }

    [Space][Button(nameof(LoadAssetsFromFolder), label = "Load Assets"), HideField] public bool _loadassets;
    [Space][Button(nameof(Clear), label = "Clear All Assets"), HideField] public bool _clearall;
#endif

    [Space(30)][HideField] public bool _s1;
    [HorizontalLine("Assets"), HideField] public bool _l1;

    [SerializeField]
    private SerializableSortedDictionary<string, T> assets = new SerializableSortedDictionary<string, T>();

    public SerializableSortedDictionary<string, T> Assets => assets;

    public void AddAsset(string key, T asset)
    {
        if (assets.ContainsKey(key))
        {
            Debug.LogWarning($"Key '{key}' already exists in the container.");
            return;
        }
        assets.Add(key, asset);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public bool TryGetAsset(string key, out T asset)
    {
        if (key == null)
        {
            asset = null;
            return false;
        }

        return assets.TryGetValue(key, out asset);
    }

    public void RemoveAsset(string key)
    {
        if (assets.Remove(key))
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        else
        {
            Debug.LogWarning($"Key '{key}' does not exist in the container.");
        }
    }

    public void Clear()
    {
        assets.Clear();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void LoadAssetsFromFolder()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogWarning("Folder path is not set.");
            return;
        }

        LoadAssets();
#endif
    }

    private void LoadAssets()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogWarning("Folder path is not set.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                string key = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                if (!assets.ContainsKey(key))
                {
                    assets.Add(key, asset);
                }
                else
                {
                    Debug.LogWarning($"Key '{key}' already exists. Skipping.");
                }
            }
        }

        EditorUtility.SetDirty(this);
#endif
    }
}