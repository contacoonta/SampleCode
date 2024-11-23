using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using CustomInspector;


[CreateAssetMenu(menuName = "THING/Profile/JourneyProfile")]
public class JourneyProfile : ScriptableObject
{
    [Serializable]
    public class LogWrapper
    {
        public LogTypes logType;
        public string logTime;

        [Dictionary]
        public SerializableSortedDictionary<string, string> logData;

        public string GetValue(string key) => logData.TryGetValue(key, out var k) ? k : $"{key} : _NONE_";
    }


    [SerializeField] public List<LogWrapper> logs = new List<LogWrapper>();

#if UNITY_EDITOR
    [Space(30)] [HideField] public bool _s0;
    [HorizontalLine,HideField] public bool _l0;    
    
    [Button(nameof(OpenPath), label = "Open Saved Path"), HideField] public bool _openpath;
    void OpenPath() => Serializer.OpenPathWindow();

    [Button(nameof(SaveFile), label = "Save To JSON"), HideField] public bool _savefile;
    void SaveFile() => Serializer.SaveJson("JourneyLog", ToJson(), false);

    [Button(nameof(ForceReset), label = "Force Reset"), HideField] public bool _forcereset;
    void ForceReset() => Reset();
#endif

    public void Reset()
    {
        Serializer.DeleteFile(nameof(GlobalData), false);

        Clear();
    }

    public void Clear()
    {
        logs.Clear();
    }

    public void AddLog(LogTypes logtype, SerializableSortedDictionary<string, string> logdata)
    {
        logs.Add( new LogWrapper() { logType = logtype, logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logData = logdata });
    }

    public string ToJson()
    {
        var jsonLogs = logs.Select(log => new { log.logType, log.logTime, log.logData });

        return JsonUtility.ToJson(new { UserLog = jsonLogs.ToList() }, true);
    }

}