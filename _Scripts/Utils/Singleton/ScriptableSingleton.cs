using UnityEngine;

public abstract class ScriptableSingleton<TObject> : ScriptableObject where TObject : ScriptableObject
{
    private static TObject _Instance;

    public static TObject I
    {
        get
        {
            if (_Instance == null) CreateOrLoadInstance();
            return _Instance;
        }
    }

    private static void CreateOrLoadInstance()
    {
        // 이미 인스턴스가 존재하는지 먼저 확인
        if (_Instance != null) return;

        string filePath = GetResourcePath();
        if (!string.IsNullOrEmpty(filePath))
        {
            _Instance = Resources.Load<TObject>(filePath);
            if (_Instance != null) return;
        }
        
#if UNITY_EDITOR
        // 에디터에서만 새로운 인스턴스 생성
        if (_Instance == null)
        {
            _Instance = CreateInstance<TObject>();
            
            // 이미 에셋이 존재하는지 확인
            string assetPath = $"Assets/Resources/{filePath}.asset";
            TObject existingAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TObject>(assetPath);
            
            if (existingAsset != null)
            {
                _Instance = existingAsset;
            }
            else
            {
                UnityEditor.AssetDatabase.CreateAsset(_Instance, assetPath);
            }
        }
#endif
    }

    private static string GetResourcePath()
    {
        var attributes = typeof(TObject).GetCustomAttributes(true);

        foreach (object attribute in attributes)
        {
            if (attribute is AssetPathAttribute pathAttribute)
                return pathAttribute.Path;
        }
        Debug.LogError($"{typeof(TObject)} does not have {nameof(AssetPathAttribute)}.");
        return string.Empty;
    }

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (_Instance != null && _Instance != this)
            {
                // 이미 존재하는 인스턴스 처리
                DestroyImmediate(this, true);
                Debug.LogWarning($"An instance of {typeof(TObject)} already exists. This duplicate has been destroyed.");
                return;
            }
            _Instance = this as TObject;
        }
#endif
    }

    protected virtual void OnDestroy()
    {
        if (_Instance == this)
        {
            _Instance = null;
        }
    }
}