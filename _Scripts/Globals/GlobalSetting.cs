
using UnityEngine;
using CustomInspector;
using IngameDebugConsole;
using DG.Tweening;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif



public enum BuildType { dev, production }
public enum PlatformType { EDITOR, ANDROID, IPHONE }

[AssetPath("GlobalSetting")]
public class GlobalSetting : ScriptableSingleton<GlobalSetting>
{
    //EVENT
    public UnityAction<GlobalSetting> eventGlobalSettingUpdated;



    [HorizontalLine("NON-SERIALIZED"), HideField] public bool _nonserialized;
    
    [Hook(nameof(Apply))]
    [SerializeField] BuildType buildtype = BuildType.dev;
    public BuildType Buildtype => buildtype;

    [Space]

    [Hook(nameof(Apply))]
    [Validate(nameof(IsVersion), "x.x.x 형태여야 합니다 !")]
    [SerializeField] string buildversion = string.Empty;
    public string BuildVersion => buildversion;
    private bool IsVersion(string v) => v.Split('.').Length == 3;

    [Space]

    [Hook(nameof(Apply))]
    [SerializeField, ReadOnly] PlatformType platformtype;
    public string PlatformType => platformtype.ToString();

    


    // On/Off line 체크
    public bool CheckConnected 
    {
        get {       
            try
            {
                using (var client = new System.Net.WebClient())
                using (client.OpenRead("http://google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }



    [HorizontalLine("SERIALIZED"), HideField] public bool _serialized;



    [SerializeField] bool intro = true;
    public bool Intro { get => intro; set { intro = value; Apply(); /*Save();*/ }}

    [Space]

    [SerializeField] bool sound = true;
    public bool Sound { get => sound; set { sound = value; Apply(); /*Save();*/ }}


    


    public void Reset()
    {
        Serializer.DeleteFile(nameof(GlobalSetting), false);

        // 사운드
        sound = true;        

        Apply();
        //Save();
        
        Debug.Log("GlobalSetting - RESET");
    }

    
    public void Apply()
    {

#if UNITY_EDITOR // Unity6 이후 버젼
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            Il2CppCompilerConfiguration config;
            if (buildtype == BuildType.production)
                config = Il2CppCompilerConfiguration.Master;
            else
                config = Il2CppCompilerConfiguration.Release;

            PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup), config);

            SetVersion(buildversion);
#endif


        // 플랫폼 타입
        if (Application.isEditor)
            platformtype = global::PlatformType.EDITOR;
        else if (Application.platform == RuntimePlatform.Android)
            platformtype = global::PlatformType.ANDROID;
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            platformtype = global::PlatformType.IPHONE;


        // 사운드        
        //SoundManager.allVolume = sound ? 1f : 0f;

        // if (GlobalSetting.I.Sound)
        //     MusicPlayer.I.Play();   
        // else
        //     MusicPlayer.I.Stop();

        

        // 개발 모드
        // if (buildtype == BuildType.dev)
        // {
        //     ConsoleSettings.EnableConsole(true);
        // }

        // 출시 모드
        else if (buildtype == BuildType.production)
        {
            // if (AFPSCounter.Instance != null)
            //     Destroy(AFPSCounter.Instance.gameObject);

            // if (QuantumConsole.Instance != null)
            //     Destroy(QuantumConsole.Instance.gameObject);

            if (DebugLogManager.Instance != null)
                Destroy(DebugLogManager.Instance.gameObject);
        }

        eventGlobalSettingUpdated?.Invoke(this);
    }


    // 저장을 위한 래퍼 클래스
    [System.Serializable]
    public class GlobalSettingWrapper
    {
        public bool sound;  // 사운드 On/Off
        public bool haptic; // 햅틱(진동) On/Off
    }

    
    Tween _delaysave;
    public void Save()
    {
        _delaysave?.Kill();
        _delaysave = DOVirtual.DelayedCall(0.5f, () => 
        {
            GlobalSettingWrapper wrapper = new GlobalSettingWrapper();
            //wrapper.language = language;
            wrapper.sound = sound;
            //wrapper.haptic = haptic;

            Serializer.SaveJson(nameof(GlobalSetting), wrapper, false);
        });
    }

    public void Load(System.Action oncomplete)
    {
        if (Serializer.LoadJson(nameof(GlobalSetting), out GlobalSettingWrapper wrapper, false))
        {
            //language = wrapper.language;
            sound = wrapper.sound;
            //haptic = wrapper.haptic;

            Apply();

            oncomplete?.Invoke();
        }
        else
            Reset();
    }

    
#if UNITY_EDITOR
    [HorizontalLine]
    [Button(nameof(OpenPath), label = "Open Saved Path"), HideField] public bool _openpath;
    void OpenPath() => Serializer.OpenPathWindow();

    private void SetVersion(string ver)
    {
        string[] versionParts = ver.Split('.');
        
        if(versionParts.Length != 3)
            Debug.LogError("올바르지 않은 버전 형식입니다.");
        else
        {
            // 중간 부분을 정수로 변환하여 번들(빌드) 번호로 설정
            if(int.TryParse(versionParts[1], out var middleNumber) == false)
                Debug.LogError("버전의 중간 부분을 정수로 변환할 수 없습니다.");
            else
            {
                PlayerSettings.bundleVersion = ver;
                PlayerSettings.Android.bundleVersionCode = middleNumber;
                PlayerSettings.iOS.buildNumber = middleNumber.ToString();
            }            
        }        
    }
#endif
}
