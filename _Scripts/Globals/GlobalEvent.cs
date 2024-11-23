using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CustomInspector;


[AssetPath("GlobalEvent")]
public class GlobalEvent : ScriptableSingleton<GlobalEvent>
{

    [Space(30)] [HideField] public bool _s0;
    [HorizontalLine("씬(Scene) 관련"),HideField] public bool _l0;
    
    public List<SceneReference> scenes;

    public void LoadSceneEpisode() => LoadScene(scenes[(int)SCENE_TYPE.EPISODE].ScenePath, true);
    public void LoadSceneEpisodeSelect() => LoadScene(scenes[(int)SCENE_TYPE.EPISODESELECT].ScenePath, true);
    public void LoadSceneJourneySelect() => LoadScene(scenes[(int)SCENE_TYPE.JOURNEYSELECT].ScenePath, true);
    

    public UnityAction eventGlobalSettingUpdated;

    public UnityAction eventLoadSceneComplete;
    public UnityAction<string, bool> eventLoadScene;
    
    public UnityAction<POPUP_TYPE, object> eventPopupOpened;
    public UnityAction eventPopupClosed;


    

    public void LoadComplete() => eventLoadSceneComplete?.Invoke();
    public void LoadScene(string scnname, bool showprogress = true) => eventLoadScene?.Invoke(scnname, showprogress);
    

    public void TriggerPopupOpen(POPUP_TYPE popuptype, object obj) => eventPopupOpened?.Invoke(popuptype, obj);
    public void TriggerPopupClose() => eventPopupClosed?.Invoke();

}
