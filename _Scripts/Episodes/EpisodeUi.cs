

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;

public class EpisodeUi : BehaviourSingleton<EpisodeUi>
{

    [Space(30)] [HideField,SerializeField] bool _s0;
    [HorizontalLine][HideField,SerializeField] bool _l0;

    [ReadOnly] public bool isPopupOpen = false;

    private List<UiPopup> popups;    
    

    protected override bool IsDontdestroy() => false;
    public override void OnAwaked()
    {   
        popups = GetComponentsInChildren<UiPopup>().ToList();
    }


    private void OnEnable() 
    {
        GlobalEvent.I.eventPopupOpened += oneventPopupOpened;
        GlobalEvent.I.eventPopupClosed += oneventPopupClosed;

        EpisodeEvent.I.eventSequenceReachedBottom += oneventSequenceReachedBottom;
    }

    private void OnDisable() 
    {
        GlobalEvent.I.eventPopupOpened -= oneventPopupOpened;
        GlobalEvent.I.eventPopupClosed -= oneventPopupClosed;

        EpisodeEvent.I.eventSequenceReachedBottom -= oneventSequenceReachedBottom;
    }


    public void oneventPopupOpened(POPUP_TYPE popuptype, object obj)
    {
        ClosePopup();

        if (popuptype == POPUP_TYPE.PUZZLE)
        {
            popups.Find(x => x is UiPopupInteraction)?.Open(obj);
        }

        isPopupOpen = true;
    }

    public void oneventPopupClosed()
    {
        ClosePopup();
    }


    private void oneventSequenceReachedBottom(bool reached, SequenceProfile profile)
    {
        if (reached)
            OpenPopup(profile);
        else
            ClosePopup();
    }




    public void OpenPopup(SequenceProfile profile)
    {
        if (isPopupOpen == true) return;

        if (profile.sequenceevent is SequenceEventEnding)
            popups.Find(x => x is UiPopupEnding)?.Open(profile);
        else if (profile.sequenceevent is SequenceEventSelect)
            popups.Find(x => x is UiPopupSelect)?.Open(profile);
        else if (profile.sequenceevent is SequenceEventNext)
            popups.Find(x => x is UiPopupNext)?.Open(profile);

        isPopupOpen = true;
    }

    public void ClosePopup()
    {
        foreach( var pop in popups )
            pop.Close();

        isPopupOpen = false;
    }

}
