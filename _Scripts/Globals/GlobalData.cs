
using UnityEngine;
using CustomInspector;
using DG.Tweening;
using NodeCanvas.StateMachines;
using System.Collections.Generic;


[AssetPath("GlobalData")]
public class GlobalData : ScriptableSingleton<GlobalData>
{

    [HorizontalLine("NON-SERIALIZED"), HideField] public bool _nonserialized;

    [ReadOnly] public ChapterProfile currentChapter;
    [ReadOnly] public EpisodeProfile currentEpisode;
    [ReadOnly] public string currentSequence;

    [Space(10)]
    [Foldout] public ChapterContainerData chapterContainer;



    [Space(10)]

    [HorizontalLine("SERIALIZED"), HideField] public bool _serialized;

    [Foldout] public JourneyProfile journeyProfile;



#if UNITY_EDITOR

    [Space(30)] [HideField] public bool _s0;
    [HorizontalLine,HideField] public bool _l0;    
    
    [Button(nameof(OpenPath), label = "Open Saved Path"), HideField] public bool _openpath;
    void OpenPath() => Serializer.OpenPathWindow();

    [Button(nameof(ForceReset), label = "Clear All Datas"), HideField] public bool _forcereset;
    void ForceReset() => Reset();
#endif

    public void Reset()
    {
        Serializer.DeleteFile(nameof(GlobalData), false);
    }

    // 저장을 위한 래퍼 클래스
    [System.Serializable]
    public class GlobalDataWrapper
    {        
        // public List<string> stageProfilesCleared; // 클리어한 스테이지들
        // public SerializableSet<string> goodsOwned; // 보유한 상품 패키지
        // public string stagePrev;    // 직전 플레이한 스테이지      
        // public int money = 0;       // 보유한 게임 머니        
        // public int jewel = 0;       // 보유한 게임 보석        
        // public bool isShowAd = true; // 광고 제거권(true 광고보임)
        // public SkinFlags skinflags;  // 보유한 스킨들
    }


    
    Tween _delaysave;
    public void Save() 
    {
        try
        {
            _delaysave?.Kill();
            _delaysave = DOVirtual.DelayedCall(0.5f, () => 
            {
                GlobalDataWrapper wrapper = new GlobalDataWrapper();
                //wrapper.stageProfilesCleared = stageProfilesCleared;
                //wrapper.goodsOwned = goodsOwned;
                //wrapper.stagePrev = stagePrev;
                //wrapper.stageNext = stageNext;                

                Serializer.SaveJson(nameof(GlobalData), wrapper, false);                
            });
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Load()
    {
        if (Serializer.LoadJson(nameof(GlobalData), out GlobalDataWrapper wrapper, false))
        {
            // stageProfilesCleared = wrapper.stageProfilesCleared;
            // goodsOwned = wrapper.goodsOwned;
            // stagePrev = wrapper.stagePrev;            
        }
        else
        {
            Reset();
            return;
        }        
    }
}
