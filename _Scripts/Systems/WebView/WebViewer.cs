using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gpm.Common.ThirdParty.SharpCompress.Common;
using Gpm.WebView;
using CustomInspector;



public class WebViewer : MonoBehaviour
{


    [HorizontalLine("EVENT")]

    public UnityEvent<string> eventReceiveFromWeb;

    [HorizontalLine]
    public string customScheme = "thing-event";

    [HorizontalLine]
    public bool clearCookie = true;
    public bool clearCache = true;

    public bool navigationBarVisible = true;
    public bool backButtonVisible = true;
    public bool forwardButtonVisible = true;
    public bool closeButtonVisible = true;
    
    public bool maskViewVisible = true;
    public bool autoRotation = true;


    


    private void Awake()
    {        

#if UNITY_ANDROID
        maskViewVisible = false;
        autoRotation = false;
#endif
    }

    


    // 예) url = "https://www.google.com"
    public void ShowWebviewOnline(string url)
    {
        GpmWebView.ShowUrl(url, GetConfiguration(), OnWebViewCallback, GetCustomSchemeList());
    }


    // 예) embedded = "index.html"
    public void ShowWebviewOffline(string embedded)
    {
        var htmlFilePath = string.Empty;
    #if UNITY_IOS
            // "file://" + Application.streamingAssetsPath + "/" + "YOUR_HTML_PATH.html"
            htmlFilePath = string.Format("file://{0}/{1}", Application.streamingAssetsPath, offline_url);
    #elif UNITY_ANDROID
            // "file:///android_asset/" + "YOUR_HTML_PATH.html"
            htmlFilePath = string.Format("file:///android_asset/{0}", embedded);
    #endif

        GpmWebView.ShowHtmlFile(htmlFilePath, GetConfiguration(), OnWebViewCallback, GetCustomSchemeList());
    }


    public bool IsActive() => GpmWebView.IsActive();
    public void CloseWebview() => GpmWebView.Close();

    public bool CanGoBack() => GpmWebView.CanGoBack();        
    public void GoBack() => GpmWebView.GoBack();

    public bool CanGoForward() => GpmWebView.CanGoForward();
    public void GoForward() => GpmWebView.GoForward();

    public void EventToWeb(string json)
    {
        string eventfromunity = $"EventFromUnity('{json}');";
        GpmWebView.ExecuteJavaScript(eventfromunity);
    }

    private void EventFromWeb(string json)
    {
        Debug.Log("이벤트 from Web: " + json);
        eventReceiveFromWeb?.Invoke(json);
    }


   

    private GpmWebViewRequest.Configuration GetConfiguration()
    {
        GpmWebViewRequest.CustomSchemePostCommand customSchemePostCommand = new GpmWebViewRequest.CustomSchemePostCommand();
        customSchemePostCommand.Close("CUSTOM_SCHEME_POST_CLOSE");

        return new GpmWebViewRequest.Configuration()
        {
            style = GpmWebViewStyle.POPUP,
            orientation = GpmOrientation.PORTRAIT,
            isClearCookie = clearCookie,
            isClearCache = clearCache,
            backgroundColor = "#FFFFFF",
            isNavigationBarVisible = navigationBarVisible,
            navigationBarColor = "#4B96E6",
            title = string.Empty,
            isBackButtonVisible = backButtonVisible,
            isForwardButtonVisible = forwardButtonVisible,
            isCloseButtonVisible = closeButtonVisible,
            supportMultipleWindows = false,
            userAgentString = string.Empty,
            addJavascript = string.Empty,
            customSchemePostCommand = customSchemePostCommand,

            position = new GpmWebViewRequest.Position { hasValue = false, x = 0, y = 0 },
            size = new GpmWebViewRequest.Size { hasValue = false, width = 0, height = 0 },
            margins = new GpmWebViewRequest.Margins { hasValue = false, left = 0, top = 0, right = 0, bottom = 0},

            isBackButtonCloseCallbackUsed = true,

#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = maskViewVisible,
            isAutoRotation = autoRotation
#endif
        };
    }


    private List<string> GetCustomSchemeList()
    {
        return new List<string> { customScheme };
    }

    private void OnWebViewCallback(GpmWebViewCallback.CallbackType callbackType, string data, GpmWebViewError error)
    {
        //Debug.Log("OnWebViewCallback: " + callbackType);
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                // if (error != null)
                //     Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                break;
            case GpmWebViewCallback.CallbackType.Close:
                // if (error != null)
                //     Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                break;
            case GpmWebViewCallback.CallbackType.PageStarted:
                // if (string.IsNullOrEmpty(data) == false)
                //     Debug.LogFormat("PageStarted Url : {0}", data);                
                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                // if (string.IsNullOrEmpty(data) == false)
                //     Debug.LogFormat("Loaded Page:{0}", data);
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:
                //Debug.Log("MultiWindowOpen");
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                //Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                EventFromWeb(parseScheme(data));
                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                //Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                //Debug.Log("GoForward");
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                //Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;            

#if UNITY_ANDROID
            case GpmWebViewCallback.CallbackType.BackButtonClose:
                //Debug.Log("BackButtonClose");
                break;
#endif
        }
    }

    private string parseScheme(string data)
    {
        if (data.StartsWith("thing-event://") == false)
            return string.Empty;

        return System.Uri.UnescapeDataString(data.Substring("thing-event://".Length));
    }
    
}
