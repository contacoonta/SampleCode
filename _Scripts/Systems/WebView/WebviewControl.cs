
using UnityEngine;
using CustomInspector;



public class WebviewControl : MonoBehaviour
{

    [Space(30)] [HideField,SerializeField] bool _s0;
    [HorizontalLine("WebView (모바일만 작동)")][HideField,SerializeField] bool _l0;
    
    [SerializeField] bool online = false;

    [ShowIf(nameof(online))]
    [SerializeField] string onlineUrl = "https://www.google.com";
    [ShowIfNot(nameof(online))]
    [SerializeField] string offlineEmbedded = "index.html";
    

    [Space(30)] [HideField,SerializeField] bool _s1;
    [HorizontalLine]
    [SerializeField] GameObject episodeui_dev;
    [SerializeField] SceneReference sceneEpisode;


    private WebViewer webviewer;


    private void OnValidate() 
    {
        webviewer = GetComponent<WebViewer>();   
        if (webviewer == null)     
            Debug.LogError("WebviewManager 없음");                
    }

    private void Awake() 
    {
        webviewer = GetComponent<WebViewer>();
        if (webviewer == null)     
            Debug.LogError("WebviewManager 없음");
    }

    void Start()
    {
        if (webviewer == null) return;

        episodeui_dev.SetActive(Application.isEditor);

        if (Application.isMobilePlatform && online == true)
            webviewer.ShowWebviewOnline(onlineUrl);
        else if (Application.isMobilePlatform && online == false)
            webviewer.ShowWebviewOffline(offlineEmbedded);
    }



    public class JsonData
    {
        public string episode;
    }


    public void EventFromWeb(string json = null)
    {
        if (string.IsNullOrWhiteSpace(json)) return;

        JsonData data = JsonUtility.FromJson<JsonData>(json);
        
        if (data != null && !string.IsNullOrEmpty(data.episode))
        {
            string episode = data.episode;
            LoadEpisode();
            webviewer.CloseWebview();
        }

        // else if (message.ToLower().Contains("userlog"))     
        // {
        //     string jstr = JsonUtility.ToJson(userLog);
        //     webviewer.EventSendToWeb($"USERLOG:{jstr}");
        // }
    }

    public void LoadEpisode()
    {
        GlobalEvent.I.LoadScene(sceneEpisode.ScenePath);
    }

}
